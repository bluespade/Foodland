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
            GameMaster.instance.SetUpEssentialGameObjects();
            GameMaster.instance.GetCanvasInstance().GetComponent<UICanvasController>().BeginFadeFromBlack();
        }
    }
}