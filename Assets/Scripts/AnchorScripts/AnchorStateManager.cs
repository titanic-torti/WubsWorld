using UnityEngine;

public class AnchorStateManager : MonoBehaviour
{
    AnchorBaseState currState;
    AnchorHeldState HeldState = new AnchorHeldState();
    AnchorIdleState IdleState = new AnchorIdleState();
    AnchorTossState TossState = new AnchorTossState();
    AnchorRetrieveState RetrieveState = new AnchorRetrieveState();

    void Start()
    {
        currState = HeldState;
        currState.EnterState(this);
    }

    void Update()
    {
        currState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currState.FixedUpdateState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currState.OnCollisionEnter2D(this, collision);
    }
}
