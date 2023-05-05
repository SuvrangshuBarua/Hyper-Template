using UnityEngine;


namespace grimhawk.managers
{
    public class InputManager : GameBehavior
    {
        [Header("Universal Settings", "#CC99FF",default)]

        public float pixelThreshold = 15;

        [Header("Gesture Settings", "#CC99FF", default)]

        #region Gesture-Public-Variables
        public bool enableGesture = false;
        public event System.Action<Dir> onSwipe;

        #endregion
        #region Gesture-Private-Variables
        private float relativeMinimalPixelCount;
        private const float effectiveWidth = 900;
        private Gesture currentGesture = null;

        #endregion

        [Header("Drag Settings", "#CC99FF", default)]

        #region  Drag-Public-Variables
        public bool enableDrag = false;
        public event System.Action<Vector2> onDragStart;
        public event System.Action<Vector2> onDrag_total;
        public event System.Action<Vector2> onDrag_delta;
        public event System.Action<Vector2> onDragEnd;
        public event System.Action<Vector3> onTap;

        #endregion

        #region  Drag-Private-Variables
        private Drag currentDrag = null;

        #endregion



        public static InputManager instance;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            relativeMinimalPixelCount = (pixelThreshold / effectiveWidth) * Screen.width;
        }

        private void Update()
        {
            if (enableGesture)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentGesture = new Gesture(new Vector2(Input.mousePosition.x, Input.mousePosition.y), relativeMinimalPixelCount);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    currentGesture = null;
                }

                if (currentGesture != null && currentGesture.type == GestureType.PROCESSING)
                {
                    if (currentGesture.ProcessInputAndReturn_IsReady(new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
                    {
                        switch (currentGesture.type)
                        {
                            case GestureType.SWIPE_UP:
                                if (onSwipe != null) onSwipe(Dir.UP);
                                break;
                            case GestureType.SWIPE_DOWN:
                                if (onSwipe != null) onSwipe(Dir.DOWN);
                                break;
                            case GestureType.SWIPE_LEFT:
                                if (onSwipe != null) onSwipe(Dir.LEFT);
                                break;
                            case GestureType.SWIPE_RIGHT:
                                if (onSwipe != null) onSwipe(Dir.RIGHT);
                                break;
                            default:
                                Debug.LogWarning("UnrecognizedInput");
                                break;
                        }
                    }
                }
            }

        }
        void D_Start()
        {
            currentDrag = new Drag(Input.mousePosition, relativeMinimalPixelCount, onDragStart);
        }
        void D_Update()
        {
            if (currentDrag != null)
            {
                //bool dragWasPending = !currentDrag.dragConfrimed;
                //Vector2 v = currentDrag.GetDragAmount(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                //if (!dragWasPending) onDrag?.Invoke(v);
                Vector2 totalTravel;
                Vector2 deltaTravel;
                if (currentDrag.GetDragAmount(new Vector2(Input.mousePosition.x, Input.mousePosition.y), out totalTravel, out deltaTravel))
                {
                    onDrag_total?.Invoke(totalTravel);
                    onDrag_delta?.Invoke(deltaTravel);
                }
            }
        }
        void D_End()
        {
            if (currentDrag != null)
            {
                if (currentDrag.dragConfrimed)
                {
                    onDragEnd?.Invoke(Input.mousePosition);
                }
                else
                {
                    onTap?.Invoke(Input.mousePosition);
                }
            }
            currentDrag = null;
        }
        private void FixedUpdate()
        {
            if (enableDrag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    D_Start();
                }
                else if (Input.GetMouseButton(0))
                {
                    if (currentDrag == null)
                    {
                        D_Start();
                    }
                    else
                    {
                        D_Update();
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    D_End();
                }
                else
                {
                    if (currentDrag != null)
                    {
                        D_End();
                    }
                }
            }

        }
    }
    public class Drag
    {
        event System.Action<Vector2> onDragConfirmed;
        Vector2 startPos;
        Vector2 lastPos;
        public bool dragConfrimed { get; private set; }
        public Vector2 travelVec { get { return lastPos - startPos; } }
        float startTime;
        float minPixel = 100;

        public bool didntDragFarEnough
        {
            get
            {
                return travelVec.magnitude < minPixel;
            }
        }
        public Drag(Vector2 startPosition, float minPix, System.Action<Vector2> onDragConfirmed)
        {
            this.minPixel = minPix;
            startTime = Time.time;
            startPos = startPosition;
            lastPos = startPosition;
            if (minPix < 0)
            {
                dragConfrimed = true;
                onDragConfirmed?.Invoke(startPos);

            }
            else
            {
                this.onDragConfirmed = onDragConfirmed;
            }

        }
        //public Vector2 GetDragAmount(Vector2 currentPosition)
        //{
        //    lastPos = currentPosition;

        //    if (didntDragFarEnough)
        //    {
        //        return Vector2.zero;
        //    }
        //    else
        //    {
        //        if(!dragConfrimed)
        //        {
        //            dragConfrimed = true;
        //            onDragConfirmed?.Invoke(startPos);
        //        }
        //        return travelVec;
        //    }
        //}

        public bool GetDragAmount(Vector2 currentPosition, out Vector2 totalTravel, out Vector2 deltaTravel)
        {
            deltaTravel = currentPosition - lastPos;
            lastPos = currentPosition;
            totalTravel = travelVec;
            //Debug.Log(travelVec);

            if (didntDragFarEnough)
            {
                return false;
            }
            else
            {
                if (!dragConfrimed)
                {
                    dragConfrimed = true;
                    onDragConfirmed?.Invoke(startPos);
                    return false;
                }
                return true;
            }
        }
    }
    public class Gesture
    {
        const float maxTime = 0.4f;
        public GestureType type = GestureType.PROCESSING;
        Vector2 startPos;
        float startTime;
        float maxPixels = 100;
        public Gesture(Vector2 startPosition, float maxPixels)
        {
            this.maxPixels = maxPixels;
            startTime = Time.time;
            startPos = startPosition;
        }

        public bool ProcessInputAndReturn_IsReady(Vector2 currentPosition)
        {
            if (type != GestureType.PROCESSING)
                return false;

            float dist = (currentPosition - startPos).magnitude;

            //'maxPixels' is the minimum amount of pixel, greater than which we will consider a swipe
            if (dist >= maxPixels)
            {
                float dX = currentPosition.x - startPos.x;
                float dY = currentPosition.y - startPos.y;
                if (Mathf.Abs(dX) > Mathf.Abs(dY))
                {
                    if (dX > 0)
                        type = GestureType.SWIPE_RIGHT;
                    else
                        type = GestureType.SWIPE_LEFT;
                }
                else
                {
                    if (dY > 0)
                        type = GestureType.SWIPE_UP;
                    else
                        type = GestureType.SWIPE_DOWN;
                }
                return true;
            }

            if (Time.time - startTime > maxTime)
            {
                type = GestureType.UNRECOGNIZED;
                return true;
            }
            else return false;
        }
    }
    public enum GestureType
    {
        UNRECOGNIZED = -1,
        PROCESSING = 0,
        SWIPE_UP,
        SWIPE_DOWN,
        SWIPE_LEFT,
        SWIPE_RIGHT,
    }
    public enum Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
}
