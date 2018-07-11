using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour {

    GameMaster GM;

    GameObject playerPlacementPoint;

    private bool active;
    public string connectedScene;
    public int id;
    public int connectedTriggerId;

	// Use this for initialization
	void Start () {
        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        playerPlacementPoint = transform.Find("PlayerPlacementPoint").gameObject;
        active = true;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
        {
            if (collision.gameObject.layer == (int)Layers.PLAYER)
            {
                active = false;
                GM.BeginSceneTransition(connectedScene, connectedTriggerId);
            }
        }  
    }

    public int GetId()
    {
        return id;
    }

    public Vector3 GetPlayerPlacementPoint()
    {
        return playerPlacementPoint.transform.position;
    }
}
