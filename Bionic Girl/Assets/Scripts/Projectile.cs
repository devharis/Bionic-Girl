using System;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed;
    public float BulletRange;

    public GameObject ProjectilePrefab;

    private Transform _projectileTransform;
    private GameObject _player;
    private Vector3 _startPosition;

	// Use this for initialization
	void Start ()
	{
        _player = GameObject.FindGameObjectWithTag("Player");
	    _startPosition = _player.transform.position;
	    _projectileTransform = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float amtToMove = ProjectileSpeed * Time.deltaTime;
        _projectileTransform.Translate(_startPosition * amtToMove);

        if (_projectileTransform.position.x >= BulletRange) {
            Debug.Log(String.Format("Bullet position: {0}", _projectileTransform.position.x));
            Destroy(gameObject);
	    }

	}
}
