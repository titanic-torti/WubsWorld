using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private float _jumpTimer;

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.SetTrigger("jump");
        player._rb.AddForce(Vector2.up * player.initialJumpStr, ForceMode2D.Impulse);
        _jumpTimer = 0;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        float jumpInput = player._jumpAction.ReadValue<float>();
        if (jumpInput <= 0 || _jumpTimer >= player.maxJumpTime)
        {
            player.SwitchState(player.FallingState);
        }
        _jumpTimer += Time.deltaTime;
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        // apply continuous jump force
        player._rb.AddForce(Vector2.up * player.jumpStr, ForceMode2D.Force);

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
