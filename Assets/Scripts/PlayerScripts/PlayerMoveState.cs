using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {

    }

    public override void UpdateState(PlayerStateManager player)
    {
        // check for state changes
        float jumpInput = player._jumpAction.ReadValue<float>();
        if (jumpInput > 0 && player.jumpCheckScript.IsGrounded())
        {
            player.step.Stop();
            player.SwitchState(player.JumpingState);
        }
        else if (!player.jumpCheckScript.IsGrounded())
        {
            player.step.Stop();
            player.SwitchState(player.FallingState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        float moveInput = player._moveAction.ReadValue<float>();
        // player.anim.SetFloat("movement", Mathf.Abs(moveInput));
        if (!player.hookThrown || player.hookScript.CheckWithinMaxAnchorDist() || (player.hookScript.transform.position - player.transform.position).normalized.x * moveInput > 0)
        {
            // apply movement force
            player._rb.AddForce(new Vector3(moveInput*player.moveStr - player._rb.linearVelocity.x, 0, 0), ForceMode2D.Force);

            // play movement sound if moving and not already playing
            if (player.step.isPlaying)
            {
                player.step.Play();
            }
        }
        else
        {
            // stop movement sound if not moving
            player.step.Stop();
        }
    }

    public override void OnCollisionEnter2D(PlayerStateManager player, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager player, Collider2D collider)
    {

    }
}
