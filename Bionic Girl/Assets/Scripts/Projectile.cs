using System;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed;
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
        TravelRight = _player.FacingRight;
	    _startPosition = _player.transform.position;
	    _projectileTransform = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (TravelRight){
            _projectileTransform.Translate(new Vector3(ProjectileSpeed * Time.deltaTime, 0, 0));
            Destroy(gameObject, BulletRange);
        }
        else if (!TravelRight)
        {
            _projectileTransform.Translate(new Vector3(-ProjectileSpeed * Time.deltaTime, 0, 0));
            Destroy(gameObject, BulletRange);
        }
	}
}
