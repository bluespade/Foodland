using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropScript : MonoBehaviour {

    public int healingAmount;

    // Use this for initialization
    void Start()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)Layers.PLAYER)
        {
            Player p = collision.gameObject.GetComponent<Player>();

            p.ApplyHealing(healingAmount);
            GameObject.Destroy(gameObject);
        }
    }
}
