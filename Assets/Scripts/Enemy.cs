using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameMaster GM;

    public int maxHealth = 10;
    private int health;
    // (itemName, dropChance)
    public Dictionary<string, float> itemDrops;

    // Use this for initialization
    void Start () {
        GM = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        health = maxHealth;
        InitializeItemDrops();
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
        DropItems();
        GameObject.Destroy(gameObject);
    }

    public virtual void InitializeItemDrops()
    {
        itemDrops = new Dictionary<string, float>();
    }

    public void DropItems()
    {
        float rand = 2f;
        List<string> keys   = new List<string>();
        List<float> values  = new List<float>();
        foreach (string key in itemDrops.Keys)
        {
            keys.Add(key);
        }
        foreach (float value in itemDrops.Values)
        {
            values.Add(value);
        }

        for (int i = 0; i < itemDrops.Count; i++)
        {
            rand = Random.Range(0f, 1f);
            print("Rolled a " + rand + "    Drop chance: " + values[i]);
            if (rand <= values[i])
            {
                print("Spawning item");
                GameObject newObj = Instantiate(GM.GetItem(keys[i]), transform.position, transform.rotation) as GameObject;
            }
        }
    }

}
