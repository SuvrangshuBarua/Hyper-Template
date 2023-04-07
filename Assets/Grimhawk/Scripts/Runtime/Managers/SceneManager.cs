using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneMaker = UnityEngine.SceneManagement.SceneManager;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class SceneManager : GameBehavior
{
    // TODO: Implement scene loader method to load scene asynchronously
    // TODO: halt all kind of activity when editor is busy with scene loading
    // TODO: Define an enum that let you choose the loading type (Fade, CircleInOut)
    private Canvas transitionCanvas;
    [SerializeField]
    private List<Transition> transitions = new();
    private AsyncOperation levelLoadOperation;
    private BaseSceneTransitionSO activeTransition;

    private void Awake()
    {
        transitionCanvas = GetComponent<Canvas>();
        transitionCanvas.enabled = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SceneMaker.activeSceneChanged += HandleSceneChange;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        SceneMaker.activeSceneChanged -= HandleSceneChange;
    }
    public void LoadScene(string SceneName,
                          SceneTransitionMode transitionMode = SceneTransitionMode.None,
                          LoadSceneMode mode = LoadSceneMode.Single)
    {
        levelLoadOperation = SceneMaker.LoadSceneAsync(SceneName);

        Transition transition = transitions.Find((transition) => transition.mode == transitionMode);
        if (transition != null)
        {
            levelLoadOperation.allowSceneActivation = false;
            transitionCanvas.enabled = true;
            activeTransition = transition.animationSO;
            StartCoroutine(Exit());
        }
        else
        {
            Debug.LogWarning($"No transition found for" +
                $" TransitionMode {transitionMode}!" +
                $" Maybe you are misssing a configuration?");
        }
    }
    private IEnumerator Exit()
    {
        yield return StartCoroutine(activeTransition.SceneExitAnimation(transitionCanvas));
        levelLoadOperation.allowSceneActivation = true;
    }
    private IEnumerator Entry()
    {
        yield return StartCoroutine(activeTransition.SceneEntryAnimation(transitionCanvas));
        transitionCanvas.enabled = false;
        levelLoadOperation = null;
        activeTransition = null;
    }
    private void HandleSceneChange(Scene OldScene, Scene NewScene)
    {
        if (activeTransition != null)
        {
            StartCoroutine(Entry());
        }
    }
}

[System.Serializable]
public class Transition
{
    public SceneTransitionMode mode;
    public BaseSceneTransitionSO animationSO;
}
public enum SceneTransitionMode
{
    None, 
    Fade,
    Circle
}