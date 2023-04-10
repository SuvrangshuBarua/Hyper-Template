
using UnityEngine;
using MyBox;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using grimhawk.managers;

namespace grimhawk.ui
{
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler))]
    public abstract class UIScreen : GameBehavior
    {
        #region MonoBehavior
        private void Awake()
        {
            AdjustCanvasRatio();
        }
        #endregion

        #region User Defined Method
        /// <summary>
        /// Makes UI Screens work properly with every screen size
        /// </summary>
        private void AdjustCanvasRatio()
        {
            float screenRatio;
            float ratio = (float) Screen.width / (float) Screen.height;

            if (ratio < 0.56f)
            {
                screenRatio = 0f;
                //CameraManager.Camera.fieldOfView += 8;
            }
            else if (ratio >= 0.56f && ratio <= 0.624f)
                screenRatio = 0.5f;
            else
                screenRatio = 1f;
            CanvasScaler canvasScaler= GetComponent<CanvasScaler>();
            if (canvasScaler != null)
                canvasScaler.matchWidthOrHeight = screenRatio;
        }
        /*[ButtonMethod]
        public void ActivateThisScreen() => UIManager.Instance.ChangeUI(this).StartCoroutine();*/
        #endregion

        #region Abstact Method
        public abstract IEnumerator PlayInAnimation();
        public abstract IEnumerator PlayOutAnimation();
        public virtual void Reset()
        {

        }
        #endregion
        
    }
}

