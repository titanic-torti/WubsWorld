using UnityEngine;

public class AnchorLatchedState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._rb.simulated = true;
        anchor._rb.bodyType = RigidbodyType2D.Kinematic;
        anchor._rb.constraints = RigidbodyConstraints2D.FreezePosition;
        anchor._dj.enabled = true;
    }

    void RevertEnterStateChanges(AnchorStateManager anchor)
    {
        anchor._rb.bodyType = RigidbodyType2D.Dynamic;
        anchor._rb.constraints = RigidbodyConstraints2D.None;
        anchor._dj.enabled = false;
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        float retrieveInput = anchor._hookRetrieve.ReadValue<float>();
        if (retrieveInput <= 0)
        {
            RevertEnterStateChanges(anchor);
            anchor._latchTimer = anchor.timeRecoverFromLatch;
            anchor.SwitchState(anchor.IdleState);
        }
    }

    public override void FixedUpdateState(AnchorStateManager anchor)
    {

    }

    public override void OnCollisionEnter2D(AnchorStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(AnchorStateManager anchor, Collider2D collider)
    {

    }
}