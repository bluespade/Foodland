using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    GameMaster GM;

	// Use this for initialization
	void Start () {
        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}

    public void LoadScene2()
    {
        GM.BeginGame("Scene2", 0);
    }
}
