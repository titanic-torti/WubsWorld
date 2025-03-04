using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private float _jumpTimer;

    public override void EnterState(PlayerStateManager anchor)
    {
        anchor.anim.SetTrigger("jump");
        anchor._rb.AddForce(Vector2.up * anchor.initialJumpStr, ForceMode2D.Impulse);
        _jumpTimer = 0;
    }

    public override void UpdateState(PlayerStateManager anchor)
    {
        float jumpInput = anchor._jumpAction.ReadValue<float>();
        if (jumpInput <= 0 || _jumpTimer >= anchor.maxJumpTime)
        {
            anchor.SwitchState(anchor.FallingState);
        }
        _jumpTimer += Time.deltaTime;
    }

    public override void FixedUpdateState(PlayerStateManager anchor)
    {
        // apply continuous jump force
        anchor._rb.AddForce(Vector2.up * anchor.jumpStr, ForceMode2D.Force);

        // flip sprite if facing wrong direction of movement
        float moveInput = anchor._moveAction.ReadValue<float>();
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
    }

    public override void OnCollisionEnter2D(PlayerStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(PlayerStateManager anchor, Collider2D collider)
    {

    }
}
