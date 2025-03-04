using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager anchor);

    public abstract void UpdateState(PlayerStateManager anchor);

    public abstract void FixedUpdateState(PlayerStateManager anchor);

    public abstract void OnCollisionEnter2D(PlayerStateManager anchor, Collision2D collision);

    public abstract void OnTriggerEnter2D(PlayerStateManager anchor, Collider2D collider);
}
