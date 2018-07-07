using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public List<GameObject> Items;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        
	}

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
}
