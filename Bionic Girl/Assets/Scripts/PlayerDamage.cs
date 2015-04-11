using System;
using UnityEngine;
using System.Collections;

public class PlayerDamage : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider) {
        //collider.gameObject.GetComponent<Projectile>();
        Debug.Log(String.Format("Hit: {0}", collider.gameObject.name));
    }

    void OnCollisionEnter2D(Collision2D collider){
        Debug.Log(String.Format("Enter collision: {0}", collider.gameObject.name));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
