using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    GameObject player;
    Camera cam;
    Transform transitionTriggersBase;
    List<Transform> sceneTransitionTriggers;
    UICanvasController Canvas;
    public List<GameObject> Items;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        Canvas = GameObject.Find("UserInterfaceCanvas").GetComponent<UICanvasController>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        transitionTriggersBase = GameObject.Find("TransitionTriggers").transform;
	}

    #region Scene Transition Functions
    public void BeginSceneTransition(string sceneName, int destinationTriggerId)
    {
        StartCoroutine(LoadNextScene(sceneName, destinationTriggerId));
    }

    public IEnumerator LoadNextScene(string sceneName, int destinationTriggerId)
    {
        Canvas.BeginFadeToBlack();
        while (!Canvas.GetFadeStatus())
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

        Canvas.BeginFadeFromBlack();
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
                    player.transform.position = hit.point;
                } else
                {
                    player.transform.position = trigger.GetPlayerPlacementPoint();
                }
                cam.SetCameraPosition(player.transform.position);
                return;
            }
        }

        print("No destination trigger found - placing player at the origin!");
        player.transform.position = new Vector3(0f, 0f, 0f);
        cam.SetCameraPosition(player.transform.position);
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
