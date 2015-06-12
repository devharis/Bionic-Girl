using UnityEngine;
using System.Collections;
using Assets.Scripts.Generic;

public class FlyingEnemy : BasicEnemy{

    private float _rayPosUnit;
    private float _rayLength = 1;

    public FlyingEnemy()
    {
        _rayLength = 10;
    }

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
                leftPos.x -= _rayPosUnit;

                var hit = Physics2D.Raycast(leftPos, -transform.up, _rayLength, 1 << LayerMask.NameToLayer("Ground"));
                if (!hit)
                {
                    // if it didn't hit anything, change direction
                    Flip();
                    WalkDir = WalkDirection.WalkRight;
                }

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
                    WalkDir = WalkDirection.WalkLeft;
                }

                Debug.DrawRay(rightPos, -transform.up * _rayLength, Color.red);
                Debug.DrawRay(transform.position, transform.right * _rayLength, Color.red);
                break;
        }
    }

    public override IEnumerator Death()
    {
        return base.Death();
    }
}
