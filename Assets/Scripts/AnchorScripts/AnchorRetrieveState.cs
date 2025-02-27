using UnityEngine;

public class AnchorRetrieveState : AnchorBaseState
{
    public override void EnterState(AnchorStateManager anchor)
    {
        anchor._rb.simulated = false;
        anchor.PlayAudio(anchor.soundAnchorDrag);
    }

    public override void UpdateState(AnchorStateManager anchor)
    {
        anchor.transform.position = Vector2.MoveTowards(anchor.transform.position, anchor.playerScript.transform.position, anchor.hookRetrieveSpeed * Time.deltaTime);
        
        float retrieveInput = anchor._hookRetrieve.ReadValue<float>();
        if (retrieveInput <= 0)
        {
            anchor.soundAnchorDrag.Stop();
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
        Debug.Log("collider with " + collider.gameObject.tag);
        if (collider.gameObject.CompareTag("Player"))
        {
            anchor.soundAnchorDrag.Stop();
            anchor.SwitchState(anchor.HeldState);
        }
    }
}
