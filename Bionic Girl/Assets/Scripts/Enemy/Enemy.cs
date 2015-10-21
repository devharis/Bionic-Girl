using System;
using System.Collections;
using Assets.Scripts.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public abstract class Enemy : MonoBehaviour {

        // instances

        // stats 
        public float MovementSpeed = 2f;
        public float AttackSpeed = 1f;
        public float AttackDamage = 1f;
        public float Armor = 1f;
        public float Health = 2f;
        public float DeathAnimationTime = 0.7f;

        // sprites
        public Sprite DeadEnemy;
        public Sprite DamagedEnemy;

        // sound
        public AnimationClip DeathEnemy;
        public AudioClip[] DeathClips;

        // raycaster
        public float RayLength = 1;
        public float RayPosUnit = 1;

        public WalkDirection WalkDir;
        private Animator _spriteAnimator;

        void Start(){
            _spriteAnimator = gameObject.GetComponent<Animator>();
            WalkDir = WalkDirection.WalkLeft;
        }

        void FixedUpdate()
        {
            // If the enemy has zero or fewer hit points and isn't dead yet...
            if (Health <= 0 && !DeadEnemy)
                StartCoroutine(Death(DeathAnimationTime));
        }

        void Update(){
            Movement();
            GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localScale.x * MovementSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        public virtual void Movement(){
            switch (WalkDir)
            {
                case WalkDirection.WalkLeft:
                    // left pos is one "rayPosUnit" away from the AI's pos
                    var leftPos = transform.position;
                    leftPos.x -= RayPosUnit;

                    var hit = Physics2D.Raycast(leftPos, -transform.up, RayLength, 1 << LayerMask.NameToLayer("Ground"));
                    if (!hit)
                    {
                        // if it didn't hit anything, change direction
                        Flip();
                        WalkDir = WalkDirection.WalkRight;
                    }

                    Debug.DrawRay(leftPos, -transform.up * RayLength, Color.green);
                    Debug.DrawRay(transform.position, -transform.right * RayLength, Color.green);
                    break;

                case WalkDirection.WalkRight:
                    // right pos is one "rayPosUnit" away from the AI's pos
                    var rightPos = transform.position;
                    rightPos.x += RayPosUnit;

                    var hitRight = Physics2D.Raycast(rightPos, -transform.up, RayLength, 1 << LayerMask.NameToLayer("Ground"));

                    if (!hitRight)
                    {
                        // if it didn't hit anything, change direction
                        Flip();
                        WalkDir = WalkDirection.WalkLeft;
                    }

                    Debug.DrawRay(rightPos, -transform.up * RayLength, Color.red);
                    Debug.DrawRay(transform.position, transform.right * RayLength, Color.red);
                    break;
            }
        }

        public virtual IEnumerator Death(float deathAnimTime)
        {
            MovementSpeed = 0f;
            // Play animation
            _spriteAnimator.Play("Death");

            // Play sound

            // Check animation time...
            yield return new WaitForSeconds(deathAnimTime);

            // Remove object
            Destroy(gameObject);
        }

        public virtual void Attack()
        {

        }

        public void Flip()
        {
            // Multiply the x component of localScale by -1.
            var enemyScale = transform.localScale;
            enemyScale.x *= -1;
            transform.localScale = enemyScale;
        }

        // collision events
        void OnTriggerEnter2D(Collider2D collider){
            if (collider.gameObject.tag == "Projectile"){
                Health--;
            }        
        }
    }
}