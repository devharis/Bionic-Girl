using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float MaxSpeed = 0f;
    public float GroundRadius = 0.2f;
    public float JumpForce = 500f;
    public Transform GroundCheck;
    public LayerMask IsGround;
    public AudioClip Audio;
    public GameObject ProjectilePrefab;

    private Animator _anim;
    private Rigidbody2D _rigidbody;
    private float _move;
    private bool _canJump;
    private bool _facingRight = true;
    private AudioSource _audio;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
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

        _anim.SetFloat("Speed", Mathf.Abs(_move));

        _rigidbody.velocity = new Vector2(_move * MaxSpeed, _rigidbody.velocity.y);

        if (_move > 0 && !_facingRight)
            Flip();
        else if (_move < 0 && _facingRight)
            Flip();
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Movement()
    {
        if (_canJump && Input.GetKeyDown(KeyCode.W))
        {
            _anim.SetBool("Ground", false);
            _rigidbody.AddForce(new Vector2(0, JumpForce));

            if (!_audio.isPlaying)
                _audio.Play();
        }
        else if (!_canJump && !_audio.isPlaying) {
            _audio.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Fire shot/projectile
            //Vector3 position = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2));
            Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        }
    }
}
