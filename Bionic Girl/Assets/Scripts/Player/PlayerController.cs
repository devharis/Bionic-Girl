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
    public LayerMask IsLadder;

    public AudioClip JumpClip;
    public AudioClip ShootClip;
    public GameObject ProjectilePrefab;

    private Animator _anim;
    private Rigidbody2D _rigidbody;
    private float _move;
    private float _vertMove;
    private bool _canJump;
    private bool _canClimb;
    private bool _isClimbing;
    private AudioSource _audio;
    public WalkDirection WalkDirection { get; private set; }
    public ClimbDirection ClimbDirection { get; private set; }

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
        _canClimb = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, IsLadder);

        _anim.SetBool("Ground", _canJump);
        _anim.SetBool("Climb", _isClimbing);
        _anim.SetFloat("vSpeed", _rigidbody.velocity.y);

        _move = Input.GetAxis("Horizontal");
        _vertMove = Input.GetAxis("Vertical");

        _anim.SetFloat("Speed", Mathf.Abs(_move));

        _isClimbing = _canJump ? false : true;

        if (_canClimb)
        {
            _rigidbody.velocity = new Vector2(_move * MaxSpeed, _vertMove * ClimbForce);
            _canJump = false;
            _isClimbing = true;
        }
        else
        {
            _rigidbody.velocity = new Vector2(_move * MaxSpeed, _rigidbody.velocity.y);
           
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
        CheckWalking();

        CheckClimbing();
     
        CheckJumping();

        CheckShooting();
    }

    public void CheckClimbing()
    {
        if (_vertMove > 0 && _canClimb)
        {
            ClimbDirection = ClimbDirection.ClimbUp;
            _rigidbody.gravityScale = 0;
        }
        else if (_vertMove < 0 && _canClimb)
        {
            ClimbDirection = ClimbDirection.ClimbDown;
            _rigidbody.gravityScale = 0;
        }
    }

    private void CheckWalking()
    {
        if (_move > 0 && WalkDirection == WalkDirection.WalkLeft)
        {
            WalkDirection = WalkDirection.WalkRight;
            Flip();
        }
        else if (_move < 0 && WalkDirection == WalkDirection.WalkRight)
        {
            WalkDirection = WalkDirection.WalkLeft;
            Flip();
        }
    }

    private void CheckJumping()
    {
        if (_canJump && Input.GetButtonDown("Jump"))
        {
            _anim.SetBool("Ground", false);

            _rigidbody.AddForce(new Vector2(0, JumpForce));

            _audio.clip = JumpClip;
            if (!_audio.isPlaying)
                _audio.Play();
        }
        else if (!_canJump && !_audio.isPlaying)
        {
            _audio.Stop();
        }
    }

    private void CheckShooting()
    {
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
            _canClimb = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll){
        if (coll.collider.tag == "Ladder")
        {
            // Reset when exiting ladder or touching ground
            _canClimb = false;
            _isClimbing = false;
        }
    }
}
