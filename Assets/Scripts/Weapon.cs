using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collide");
        if (collision.gameObject.layer == (int)Layers.ENEMY)
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            enemyScript.TakeDamage(5);
        }
    }
}
