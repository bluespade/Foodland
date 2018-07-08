using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject targetObj;

    public float horizontalOffset;

    private Vector3 targetPos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        targetPos = targetObj.transform.position;
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