using UnityEngine;

public class AnchorTossState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._rb.simulated = false;
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        anchor.transform.position = Vector2.MoveTowards(anchor.transform.position, anchor._currTarget, anchor.hookTossSpeed * Time.deltaTime);
        if (WithinThrow(anchor, anchor._currTarget) || !CheckWithinMaxAnchorDist(anchor))
        {
            anchor.PlayAudio(anchor.soundAnchorMiss);
            anchor.SwitchState(anchor.IdleState);
        }
    
    }

    bool WithinThrow(AnchorStateManager anchor, Vector2 target)
    {
        return anchor.closenessBounds >= (new Vector2(anchor.transform.position.x, anchor.transform.position.y) - target).magnitude;
    }

    bool CheckWithinMaxAnchorDist(AnchorStateManager anchor)
    {
        return anchor.maxAnchorDist >= (anchor.playerScript.transform.position - anchor.transform.position).magnitude;
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
