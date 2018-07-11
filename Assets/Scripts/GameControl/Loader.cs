using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour
{
    public GameObject gameMaster;


    void Awake()
    {
        if (GameMaster.instance == null)
        {
            Instantiate(gameMaster);
        }

        gameMaster.GetComponent<GameMaster>().BeginGame(this.gameObject.scene.name, 0);
    }
}