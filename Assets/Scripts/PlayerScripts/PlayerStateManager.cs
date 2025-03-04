using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // STATES
    PlayerBaseState currState;
    PlayerMoveState MoveState = new PlayerMoveState();
    PlayerJumpingState JumpingState = new PlayerJumpingState();
    PlayerFallingState FallingState = new PlayerFallingState();

    
}
