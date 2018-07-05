using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int maxHealth = 10;
    private int health;

	// Use this for initialization
	void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            print("i am dead");
            Die();
        }
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }
}
