using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    GameMaster GM;

    public GameObject targetObj;

    public float horizontalOffset;

    private Vector3 targetPos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        //Destroy this instance if one already exists
        if (GM.GetMainCameraInstance() != null)
        {
            Destroy(gameObject);
        }
        targetObj = GM.GetPlayerInstance();

        targetPos = targetObj.transform.position;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        targetPos = new Vector3(targetObj.transform.position.x, targetObj.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }

    public void SetCameraPosition(Vector3 vec)
    {
        transform.position = new Vector3(vec.x, vec.y, -10f);
    }
}