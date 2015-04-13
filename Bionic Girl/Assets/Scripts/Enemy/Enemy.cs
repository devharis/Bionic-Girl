using System;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float MoveSpeed = 2f;
    public bool FacingRight;
    public bool IsCollision;
    public int Health = 2;
    public Sprite DeadEnemy;
    public Sprite DamagedEnemy;
    public AudioClip[] DeathClips;
    
    private SpriteRenderer _spriteRenderer;
    private Transform _frontCheck;
    private bool _dead = false;

    void Start() {
        _spriteRenderer = transform.Find("Army-Soldier").GetComponent<SpriteRenderer>();
    }
    
    void Awake() {
        _frontCheck = transform.Find("FrontCheck").transform;
    }

    void FixedUpdate() {
        Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position, 1);

        // Check each of the colliders.
        foreach (Collider2D c in frontHits)
        {
            Debug.Log(c.name);
            // If any of the colliders is an Obstacle...
            if (c.tag == "Obstacle")
            {
                IsCollision = true;
                Debug.Log(IsCollision);
                // ... Flip the enemy and stop checking the other colliders.
                Flip();
                break;
            }
        }

        // Set the enemy's velocity to moveSpeed in the x direction.
        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * MoveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        // If the enemy has one hit point left and has a damagedEnemy sprite...
        if (Health == 1 && DamagedEnemy != null)
            // ... set the sprite renderer's sprite to be the damagedEnemy sprite.
            _spriteRenderer.sprite = DamagedEnemy;

        // If the enemy has zero or fewer hit points and isn't dead yet...
        //if (Health <= 0 && !DeadEnemy)
            // ... call the death function.
            //Death();

        IsCollision = false;
        Debug.Log(IsCollision);
    }

    public void Flip() {
        FacingRight = !FacingRight;
        //Check if turn right 

        // Check if turn left

        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
