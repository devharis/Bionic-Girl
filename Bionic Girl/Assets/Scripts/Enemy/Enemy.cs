using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public MoveSpeed = 2f;
    public int Health = 2;
    public Sprite DeadEnemy;
    public Sprite DamagedEnemy;
    public AudioClip[] DeathClips;
    
    private SpriteRenderer _spriteRenderer;
    private Transform _frontCheck;
    private bool _dead = false;
    
    void Awake() {
        _spriteRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        _frontCheck = transform.Find("frontCheck").transform;
    }


}
