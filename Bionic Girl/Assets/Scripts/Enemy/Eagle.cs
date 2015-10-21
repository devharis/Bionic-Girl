using System;
using System.Collections;
using Assets.Scripts.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Eagle : Enemy {


        public override void Attack()
        {
            base.Attack();
        }

        public override void Movement()
        {
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

        public override IEnumerator Death(float deathAnimTime)
        {
            base.Death(deathAnimTime);
            return null;
        }
    }
}