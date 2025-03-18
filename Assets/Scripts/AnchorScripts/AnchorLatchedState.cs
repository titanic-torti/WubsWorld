using UnityEngine;

public class AnchorLatchedState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._rb.simulated = true;
        anchor._rb.bodyType = RigidbodyType2D.Kinematic;
        anchor._rb.constraints = RigidbodyConstraints2D.FreezePosition;
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
        float unlatchHookInput = anchor._unlatchHook.ReadValue<float>();
        if (retrieveInput > 0)
        {
            RevertEnterStateChanges(anchor);
            anchor._latchTimer = anchor.timeRecoverFromLatch;
            anchor.playerScript._rb.AddForce(anchor.playerScript._rb.linearVelocity * anchor.unlatchMomentumBonus, ForceMode2D.Impulse);
            anchor.SwitchState(anchor.IdleState);
        }
        else if (unlatchHookInput > 0 &&
            !anchor.playerScript.jumpCheckScript.IsGrounded() &&
            anchor.playerScript.GetState() != anchor.playerScript.JumpingState)
        {
            RevertEnterStateChanges(anchor);
            anchor._latchTimer = anchor.timeRecoverFromLatch;
            anchor.playerScript._rb.AddForce(anchor.playerScript._rb.linearVelocity * anchor.unlatchMomentumBonus, ForceMode2D.Impulse);
            anchor._rb.AddForce(anchor.playerScript._rb.linearVelocity, ForceMode2D.Impulse);
            anchor.SwitchState(anchor.IdleState);
        }

        // rappel down
        if (!anchor.playerScript.jumpCheckScript.IsGrounded() && anchor._rappelDown.ReadValue<float>() > 0 && anchor._dj.distance < anchor.maxAnchorDist)
        {
            Debug.Log("rappel down");
            anchor._dj.distance += anchor.rappelSpeed * Time.deltaTime;
        }

        // rappel up
        if (anchor._rappelUp.ReadValue<float>() > 0 && anchor._dj.distance > 1)
        {
            if (anchor.playerScript.jumpCheckScript.IsGrounded()) {
                // apply small force to get player off of the ground,
                // otherwise rappelling up won't work when grounded
                Vector2 dist = anchor._rb.position - anchor.playerScript._rb.position;
                anchor.playerScript._rb.AddForce(dist.normalized * 5);
            }
            anchor._dj.distance -= anchor.rappelSpeed * Time.deltaTime;
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