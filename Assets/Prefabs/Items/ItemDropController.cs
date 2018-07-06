using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour {

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(2.2f, 2.4f)) * 150f);
	}
}
