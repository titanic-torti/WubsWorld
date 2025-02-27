using UnityEngine;

public class AnchorHeldState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._sr.enabled = false;
        anchor._rb.simulated = false;
        anchor.playerScript.hookThrown = false;
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        anchor.transform.position = anchor.playerScript.transform.position;
        float throwInput = anchor._hookThrow.ReadValue<float>();
        if (throwInput > 0)
        {
            anchor._sr.enabled = true;
            anchor.playerScript.hookThrown = true;
            anchor.SwitchState(anchor.TossState);
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
