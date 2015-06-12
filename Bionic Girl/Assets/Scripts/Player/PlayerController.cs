using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Generic;

public class PlayerController : MonoBehaviour {

    public float MaxSpeed = 0f;
    public float GroundRadius = 0.2f;
    public float JumpForce = 500f;
    public float ClimbForce = 0f;
    public bool FacingRight = true;
    
    public Transform GroundCheck;
    public LayerMask IsGround;

    public AudioClip JumpClip;
    public AudioClip ShootClip;
    public GameObject ProjectilePrefab;

    private Animator _anim;
    private Rigidbody2D _rigidbody;
    private float _move;
    private float _vertMove;
    private bool _canJump;
    private bool _isClimbing;
    private AudioSource _audio;
    public WalkDirection WalkDirection { get; private set; }


    void Start()
    {
        _anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
        WalkDirection = WalkDirection.WalkRight;
    }

    void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        _canJump = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, IsGround);
        _anim.SetBool("Ground", _canJump);

        _anim.SetFloat("vSpeed", _rigidbody.velocity.y);

        _move = Input.GetAxis("Horizontal");
        _vertMove = Input.GetAxis("Vertical");

        _anim.SetFloat("Speed", Mathf.Abs(_move));

        _rigidbody.velocity = new Vector2(_move * MaxSpeed, _rigidbody.velocity.y);
        if (_isClimbing)
        {
            _rigidbody.velocity = new Vector2(_move * MaxSpeed, _vertMove * ClimbForce);
        }

        if (_move > 0 && WalkDirection == WalkDirection.WalkLeft){
            WalkDirection = WalkDirection.WalkRight;
            Flip();
        }
        else if (_move < 0 && WalkDirection == WalkDirection.WalkRight){
            WalkDirection = WalkDirection.WalkLeft;
            Flip();
        }
    }

    private void Flip()
    {
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Movement()
    {
        if (_isClimbing && Input.GetAxis("Vertical") > 0)
        {
            // Set animation
            _anim.SetBool("Climb", true);
            _rigidbody.velocity = new Vector2(_move * MaxSpeed, Input.GetAxis("Vertical") * ClimbForce);
            Debug.Log(new Vector2(_move * MaxSpeed, Input.GetAxis("Vertical") * ClimbForce));
        }

        if (_canJump && Input.GetButtonDown("Jump"))
        {
            _anim.SetBool("Ground", false);

            _rigidbody.AddForce(new Vector2(0, JumpForce));
            
            _audio.clip = JumpClip;
            if (!_audio.isPlaying)
                _audio.Play();
        }
        else if (!_canJump && !_audio.isPlaying) {
            _audio.Stop();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // Fire shot/projectile
            Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
            _audio.clip = ShootClip;
            _audio.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ladder"){
            _rigidbody.gravityScale = 0;
        }
    }

    void OnCollisionStay2D(Collision2D coll){
        if (coll.collider.tag == "Ladder")
        {
            _isClimbing = true;
            // Climb

            Debug.Log(_isClimbing);
        }

        if (coll.collider.tag == "OneWayPlatform"){
            //Physics2D.IgnoreCollision(coll.collider, _rigidbody.GetComponent<Collider2D>(), false);
        }
    }

    void OnCollisionExit2D(Collision2D coll){
        if (coll.collider.tag == "Ladder")
        {
            // Change animation
            _rigidbody.gravityScale = 1;
            _isClimbing = false;
        }

        if (coll.collider.tag == "OneWayPlatform")
        {
            //Physics2D.IgnoreCollision(coll.collider, _rigidbody.GetComponent<Collider2D>(), true);

        }
    }
}
