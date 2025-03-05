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
            // apply movement force
            anchor._rb.AddForce(new Vector3(moveInput*anchor.moveStr - anchor._rb.linearVelocity.x, 0, 0), ForceMode2D.Force);

            // play movement sound if moving and not already playing
            if (anchor.step.isPlaying)
            {
                anchor.step.Play();
            }
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager anchor, Collider2D collider)
    {

    }
}
