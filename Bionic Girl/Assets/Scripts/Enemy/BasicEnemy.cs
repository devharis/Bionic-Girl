using System;
using System.Collections;
using Assets.Scripts.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    // stats 
    public float MovementSpeed = 2f;
    public float AttackSpeed = 1f;
    public float AttackDamage = 1f;
    public float Armor = 1f;
    public float Health = 2f;
    
    // sprites, sound
    public Sprite DeadEnemy;
    public Sprite DamagedEnemy;

    public AnimationClip DeathEnemy;
    public AudioClip[] DeathClips;

    // raycaster
    public float _rayLength = 1;
    private float _rayPosUnit = 1;

    private GameObject _enemyObject;
    private Animator _spriteAnimator;
    private bool _dead = false;
    
    private Vector3 _moveDirection;
    private WalkDirection _walkDir;

    void Start(){
        _spriteAnimator = gameObject.GetComponent<Animator>();
        _walkDir = WalkDirection.WalkLeft;
    }
    
    void Awake() {
        //_frontCheck = transform.Find("FrontCheck").transform;
    }

    void FixedUpdate()
    {
        //Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position, 1);
        //if (frontHits.Any(c => c.tag == "Projectile"))
        //    // Hit

        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (Health <= 0 && !DeadEnemy)
            StartCoroutine(Death());
    }

    void Update(){

        Movement();
        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * MovementSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    private void Movement(){
        switch (_walkDir)
        {
            case WalkDirection.WalkLeft:
                // left pos is one "rayPosUnit" away from the AI's pos
                var leftPos = transform.position;
                leftPos.x -= _rayPosUnit;

                var hit = Physics2D.Raycast(leftPos, -transform.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));
                if (!hit)
                {
                    // if it didn't hit anything, change direction
                    Flip();
                    _walkDir = WalkDirection.WalkRight;
                }
                //if (hit){
                //    _walkDir = WalkDirection.WalkRight;
                //}
                Debug.DrawRay(leftPos, -transform.up * _rayLength, Color.green);
                Debug.DrawRay(transform.position, -transform.right * _rayLength, Color.green);
                break;

            case WalkDirection.WalkRight:
                // right pos is one "rayPosUnit" away from the AI's pos
                var rightPos = transform.position;
                rightPos.x += _rayPosUnit;

                var hitRight = Physics2D.Raycast(rightPos, -transform.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));

                if (!hitRight)
                {
                    // if it didn't hit anything, change direction
                    Flip();
                    _walkDir = WalkDirection.WalkLeft;
                }
                //if (hitRight){
                //    _walkDir = WalkDirection.WalkLeft;
                //}

                Debug.DrawRay(rightPos, -transform.up * _rayLength, Color.red);
                Debug.DrawRay(transform.position, transform.right * _rayLength, Color.red);
                break;
        }
    }

    private IEnumerator Death(){
        MovementSpeed = 0f;
        // Play animation
        _spriteAnimator.Play("Death");

        // Play sound

        // Check animation time...
        yield return new WaitForSeconds(0.7f);

        Debug.Log(String.Format("Remove : {0}", gameObject));
        // Remove object
        Destroy(gameObject);
    }

    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        var enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    public void Attack(){
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Projectile"){
            Health--;
        }        
    }
}