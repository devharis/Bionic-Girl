using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class BasicEnemy : MonoBehaviour {

    // raycaster
    public float RayLength = 1;
    public float RayPosUnit = 1;

    public float MaxSpeed = 2f;
    private int _health = 2;

    private bool _dead = false;

    public Sprite DeadEnemy;
    public Sprite DamagedEnemy;
    public AudioClip[] DeathClips;
    private GameObject _enemyObject;
    private SpriteRenderer _spriteRenderer;
    private Transform _frontCheck;
    
    private Vector3 _moveDirection;
    private WalkDirection _walkDir;

    void Start(){
        _enemyObject = GameObject.Find("Shield-Soldier").gameObject;
        _spriteRenderer = GameObject.Find("Shield-Soldier").GetComponent<SpriteRenderer>();
        _walkDir = WalkDirection.WalkLeft;
    }
    
    void Awake() {
        _frontCheck = transform.Find("FrontCheck").transform;
    }

    void FixedUpdate()
    {
        //Collider2D[] frontHits = Physics2D.OverlapPointAll(_frontCheck.position, 1);
        //if (frontHits.Any(c => c.tag == "Projectile"))
        //    // Hit

        // If the enemy has one hit point left and has a damagedEnemy sprite...
        if (_health == 1 && DamagedEnemy != null)
            _spriteRenderer.sprite = DamagedEnemy;

        // If the enemy has zero or fewer hit points and isn't dead yet...
        if (_health <= 0 && !DeadEnemy)
            Death();

        GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * MaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
    }

    protected void Update(){
        // left pos is one "rayPosUnit" away from the AI's pos
        var leftPos = transform.position;
        leftPos.x -= RayPosUnit;

        // right pos is one "rayPosUnit" away from the AI's pos
        var rightPos = transform.position;
        rightPos.x += RayPosUnit;

        switch (_walkDir){
            case WalkDirection.WalkLeft:
                var hit = Physics2D.Raycast(leftPos, -transform.up, RayLength, LayerMask.NameToLayer("Ground"));
                if (!hit)
                {
                    // if it didn't hit anything, change direction
                    Flip();
                    _walkDir = WalkDirection.WalkRight;
                } 
                Debug.DrawRay(leftPos, -transform.up * RayLength, Color.green);

                if (hit && hit.collider.gameObject.tag == "Ground")
                {
                    _moveDirection.x = -MaxSpeed * Time.deltaTime;
                    _walkDir = WalkDirection.WalkRight;
                } 
                Debug.DrawRay(transform.position, -transform.right * RayLength, Color.green);
                break;
            case WalkDirection.WalkRight:
                var hitRight = Physics2D.Raycast(rightPos, -transform.up, RayLength, LayerMask.NameToLayer("Ground"));
                if (!hitRight){
                    // if it didn't hit anything, change direction
                    Flip();
                    _walkDir = WalkDirection.WalkLeft;
                } 
                Debug.DrawRay(rightPos, -transform.up * RayLength, Color.green);
                if (hitRight && hitRight.collider.gameObject.tag == "Ground")
                {
                    _moveDirection.x = MaxSpeed * Time.deltaTime;
                    _walkDir = WalkDirection.WalkLeft;
                } 
                Debug.DrawRay(transform.position, transform.right * RayLength, Color.green);

                
                break;
        }

        transform.Translate(_moveDirection);
    }

    private void Death(){
        Destroy(_enemyObject);
    }

    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        var enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Projectile"){
            _health--;
        }        
    }
}

public enum WalkDirection{
    WalkLeft,
    WalkRight
}