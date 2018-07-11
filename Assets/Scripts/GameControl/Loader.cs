using UnityEngine;
using System.Collections;

// This class should be put on all scenes you want to test a character in
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