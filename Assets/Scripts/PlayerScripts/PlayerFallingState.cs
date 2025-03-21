using UnityEngine;

public class PlayerFallingState : PlayerBaseState 
{
    public override void EnterState(PlayerStateManager player)
    {
        
    }

    public override void UpdateState(PlayerStateManager player)
    {
        // switch states
        if (player.jumpCheckScript.IsGrounded())
        {
            player.SwitchState(player.MoveState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        // apply movement force
        float moveInput = player._moveAction.ReadValue<float>();
        if (!player.hookThrown || player.hookScript.CheckWithinMaxAnchorDist() || (player.hookScript.transform.position - player.transform.position).normalized.x * moveInput > 0)
        {
            player._rb.AddForce(new Vector3(moveInput*player.moveStr - player._rb.linearVelocity.x, 0, 0), ForceMode2D.Force);    
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collider)
    {

    }
}
