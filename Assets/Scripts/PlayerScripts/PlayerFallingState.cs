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
        // apply movement force
        float moveInput = anchor._moveAction.ReadValue<float>();
        if (!anchor.hookThrown || anchor.hookScript.CheckWithinMaxAnchorDist() || (anchor.hookScript.transform.position - anchor.transform.position).normalized.x * moveInput > 0)
        {
            anchor._rb.AddForce(new Vector3(moveInput*anchor.moveStr - anchor._rb.linearVelocity.x, 0, 0), ForceMode2D.Force);    
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager anchor, Collider2D collider)
    {

    }
}
