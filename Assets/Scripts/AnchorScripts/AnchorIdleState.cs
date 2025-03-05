using UnityEngine;

public class AnchorIdleState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._rb.simulated = true;
        // anchor._rb.linearVelocity = Vector3.zero;
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        float retrieveInput = anchor._hookRetrieve.ReadValue<float>();
        if (retrieveInput > 0)
        {
            anchor.SwitchState(anchor.RetrieveState);
        }
    }

    public override void FixedUpdateState(AnchorStateManager anchor)
    {

    }

    public override void OnCollisionEnter2D(AnchorStateManager anchor, Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            anchor.SwitchState(anchor.HeldState);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            anchor.soundEnemyHurt.Play();
        }
    }

    public override void OnTriggerEnter2D(AnchorStateManager anchor, Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Anchor Point") && anchor._latchTimer <= 0)
        {
            anchor.transform.position = collider.transform.position;
            anchor.soundAnchorHit.Play();
            anchor.SwitchState(anchor.LatchedState);
        }
    }
}
