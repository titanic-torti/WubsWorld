using UnityEngine;
using UnityEngine.InputSystem;

public class AnchorTossState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        // anchor._currTarget is now set within PlayerStateManager.UpdateThrowPreview()
        // anchor._currTarget = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // anchor._rb.simulated = false;
        anchor._rb.simulated = true;
        anchor._rb.gravityScale = 0;
        anchor.PlayAudio(anchor.soundAnchorThrow);
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        anchor.transform.position = Vector2.MoveTowards(anchor.transform.position, anchor._currTarget, anchor.hookTossSpeed * Time.deltaTime);
        if (WithinThrow(anchor, anchor._currTarget) || !anchor.CheckWithinMaxAnchorDist())
        {
            anchor.PlayAudio(anchor.soundAnchorMiss);
            anchor._rb.gravityScale = 1;
            anchor.SwitchState(anchor.IdleState);
        }
    }

    bool WithinThrow(AnchorStateManager anchor, Vector2 target)
    {
        return anchor.closenessBounds >= (new Vector2(anchor.transform.position.x, anchor.transform.position.y) - target).magnitude;
    }

    public override void FixedUpdateState(AnchorStateManager anchor)
    {

    }

    public override void OnCollisionEnter2D(AnchorStateManager anchor, Collision2D collision)
    {

    }

    public override void OnTriggerEnter2D(AnchorStateManager anchor, Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Anchor Point"))
        {
            anchor.transform.position = collider.transform.position;
            anchor.soundAnchorHit.Play();
            anchor._rb.gravityScale = 1;
            anchor.SwitchState(anchor.LatchedState);
        }
    }
}
