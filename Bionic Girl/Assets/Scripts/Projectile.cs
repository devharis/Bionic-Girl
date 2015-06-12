using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Generic;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 1;
    public float BulletRange = 1;
    public bool TravelRight;

    public GameObject ProjectilePrefab;

    private Transform _projectileTransform;
    private PlayerController _player;
    private Vector3 _startPosition;

	// Use this for initialization
	void Start ()
	{
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent("PlayerController") as PlayerController;
	    TravelRight = _player.WalkDirection == WalkDirection.WalkRight;
        
	    _projectileTransform = transform;
        // Tweak projectile spawn point
        float tempY = (_projectileTransform.position.y - 0.095f);
        _projectileTransform.position = new Vector3(_projectileTransform.position.x, tempY, _projectileTransform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (TravelRight){
            float tempX = (_projectileTransform.position.x + 0.6f);
            _projectileTransform.position = new Vector3(tempX, _projectileTransform.position.y, _projectileTransform.position.z);
            _projectileTransform.Translate(new Vector3(ProjectileSpeed * Time.deltaTime, 0, 0));
            Destroy(gameObject, BulletRange);
        }
        else if (!TravelRight)
        {
            float tempX = (_projectileTransform.position.x - 0.6f);
            _projectileTransform.position = new Vector3(tempX, _projectileTransform.position.y, _projectileTransform.position.z);
            _projectileTransform.Translate(new Vector3(-ProjectileSpeed * Time.deltaTime, 0, 0));
            Destroy(gameObject, BulletRange);
        }
	}

    void OnTriggerEnter2D(Collider2D collider){
        // Do nothing if projectile hits player
        if (collider.gameObject.tag == "Player")
            return;

        if (collider.gameObject.tag == "Ladder")
            return;

        if (collider is EdgeCollider2D && collider.gameObject.tag == "Enemy")
            return;

        // Destroy the projectile
        Destroy(gameObject);
    }
}
