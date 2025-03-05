using UnityEngine;

public class AnchorLatchedState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._rb.simulated = true;
        anchor._rb.bodyType = RigidbodyType2D.Kinematic;
        anchor._rb.constraints = RigidbodyConstraints2D.FreezePosition;
        //anchor._dj.enabled = true;
    }

    void RevertEnterStateChanges(AnchorStateManager anchor)
    {
        anchor._rb.bodyType = RigidbodyType2D.Dynamic;
        anchor._rb.constraints = RigidbodyConstraints2D.None;
        anchor._dj.enabled = false;
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        // Detaching anchor
        float retrieveInput = anchor._hookRetrieve.ReadValue<float>();
        if (retrieveInput > 0)
        {
            RevertEnterStateChanges(anchor);
            anchor._latchTimer = anchor.timeRecoverFromLatch;
            anchor.SwitchState(anchor.IdleState);
        }

        if (!anchor.playerScript.jumpCheckScript.IsGrounded())
        {
            const float ADJUSTMENT_SPEED = 0.01f;

            // Are these keys okay? Do they feel "natural"?
            const KeyCode EXTEND_KEY = KeyCode.S;
            const KeyCode RETRACT_KEY = KeyCode.W;

            // Extend
            if (Input.GetKey(EXTEND_KEY) && anchor._dj.distance < anchor.maxAnchorDist)
            {
                anchor._dj.distance += ADJUSTMENT_SPEED;
            }

            // Retract
            if (Input.GetKey(RETRACT_KEY) && anchor._dj.distance > 1)
            {
                anchor._dj.distance -= ADJUSTMENT_SPEED;
            }
        }
    }

    public override void FixedUpdateState(AnchorStateManager anchor)
    {
        if (anchor.playerScript.jumpCheckScript.IsGrounded() && anchor._dj.enabled) {
            anchor._dj.enabled = false;
        }

        if (!anchor.playerScript.jumpCheckScript.IsGrounded() && !anchor._dj.enabled) {
            anchor._dj.enabled = true;
        }
    }

    public override void OnCollisionEnter2D(AnchorStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(AnchorStateManager anchor, Collider2D collider)
    {

    }
}