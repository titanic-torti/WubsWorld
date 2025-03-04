using UnityEngine;

public class PlayerFallingState : PlayerBaseState 
{
    public override void EnterState(PlayerStateManager anchor)
    {
        
    }

    public override void UpdateState(PlayerStateManager anchor)
    {
        // switch states
        if (anchor.jumpCheckScript.IsGrounded())
        {
            anchor.SwitchState(anchor.MoveState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager anchor)
    {
        float moveInput = anchor._moveAction.ReadValue<float>();
        anchor.anim.SetFloat("movement", Mathf.Abs(moveInput));
        if (!anchor.hookThrown || anchor.hookScript.CheckWithinMaxAnchorDist() || (anchor.hookScript.transform.position - anchor.transform.position).normalized.x * moveInput > 0)
        {
            // flip sprite if facing wrong direction of movement
            if ((moveInput > 0 && !anchor.sprite.flipX) || (moveInput < 0 && anchor.sprite.flipX))
            {
                anchor.sprite.flipX = !anchor.sprite.flipX;
                anchor.finSprite.flipX = !anchor.finSprite.flipX;
                if (anchor.finSprite.flipX)
                {
                    anchor.finSprite.transform.position += anchor.finOffset;
                    anchor.anchorSprite.transform.position += anchor.finOffset;
                }
                else
                {
                    anchor.finSprite.transform.position -= anchor.finOffset;
                    anchor.anchorSprite.transform.position -= anchor.finOffset;
                }
            }

            // apply movement force
            anchor._rb.AddForce(new Vector3(moveInput*anchor.moveStr - anchor._rb.linearVelocity.x, 0, 0), ForceMode2D.Force);

            // play movement sound if moving and not already playing
            if (anchor.step.isPlaying)
            {
                anchor.step.Play();
            }
        }
        else
        {
            // stop movement sound if not moving
            anchor.step.Stop();
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager anchor, Collider2D collider)
    {

    }
}
