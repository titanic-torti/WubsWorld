using UnityEngine;

public abstract class AnchorBaseState
{
    public abstract void EnterState(AnchorStateManager anchor);

    public abstract void UpdateState(AnchorStateManager anchor);

    public abstract void FixedUpdateState(AnchorStateManager anchor);

    public abstract void OnCollisionEnter2D(AnchorStateManager anchor, Collision2D collision);
}
