using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject mainCameraPrefab;
    public GameObject canvasPrefab;

    public static GameObject Player;
    public static GameObject MainCamera;
    public static GameObject Canvas;

    Camera camScript;
    Transform transitionTriggersBase;
    List<Transform> sceneTransitionTriggers;
    UICanvasController canvasController;
    public List<GameObject> Items;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        //Player = GameObject.Find("Player");
        //canvasController = GameObject.Find("UserInterfacecanvasController").GetComponent<UICanvasController>();
        //cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //transitionTriggersBase = GameObject.Find("TransitionTriggers").transform;
	}

    public void BeginGame(string sceneName, int startingTriggerId)
    {
        SetUpEssentialGameObjects();
        BeginSceneTransition(sceneName, startingTriggerId);
    }

    public void SetUpEssentialGameObjects()
    {
        Player = Instantiate(playerPrefab);
        MainCamera = Instantiate(mainCameraPrefab);
        Canvas = Instantiate(canvasPrefab);

        camScript = MainCamera.GetComponent<Camera>();
        canvasController = Canvas.GetComponent<UICanvasController>();
    }

    public GameObject GetPlayerInstance()
    {
        return Player;
    }

    public GameObject GetMainCameraInstance()
    {
        return MainCamera;
    }

    public GameObject GetCanvasInstance()
    {
        return Canvas;
    }

    #region Scene Transition Functions
    public void BeginSceneTransition(string sceneName, int destinationTriggerId)
    {
        StartCoroutine(LoadNextScene(sceneName, destinationTriggerId));
    }

    public IEnumerator LoadNextScene(string sceneName, int destinationTriggerId)
    {
        canvasController.BeginFadeToBlack();
        while (!canvasController.GetFadeStatus())
        {
            yield return null;
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);

        while (!ao.isDone)
        {
            yield return null;
        }

        //Place the player at the proper location in the new scene
        GetSceneTransitionTriggers();
        PlacePlayerAtDestination(destinationTriggerId);

        canvasController.BeginFadeFromBlack();
    }

    void GetSceneTransitionTriggers()
    {
        sceneTransitionTriggers = new List<Transform>();
        transitionTriggersBase = GameObject.Find("TransitionTriggers").transform;

        foreach (Transform child in transitionTriggersBase)
        {
            sceneTransitionTriggers.Add(child);
        }
    }

    void PlacePlayerAtDestination(int destinationTriggerId)
    {
        SceneTransitionTrigger trigger;
        foreach (Transform t in sceneTransitionTriggers)
        {
            trigger = t.GetComponent<SceneTransitionTrigger>();
            if (trigger.GetId() == destinationTriggerId)
            {
                RaycastHit2D hit = Physics2D.Raycast(trigger.GetPlayerPlacementPoint(), Vector2.down, 1000f);
                if (hit)
                {
                    Player.transform.position = hit.point;
                } else
                {
                    Player.transform.position = trigger.GetPlayerPlacementPoint();
                }
                camScript.SetCameraPosition(Player.transform.position);
                return;
            }
        }

        print("No destination trigger found - placing player at the origin!");
        Player.transform.position = new Vector3(0f, 0f, 0f);
        camScript.SetCameraPosition(Player.transform.position);
    }

    #endregion

    #region Item Functions
    int ItemStringToIndex(string str)
    {
        //DummyItem is always at index 0
        if (str.Equals("HealthDrop_Small"))
        {
            return 1;
        }

        return 0;
    }

    public GameObject GetItem(string str)
    { 
        return Items[ItemStringToIndex(str)];
    }
    #endregion
}
