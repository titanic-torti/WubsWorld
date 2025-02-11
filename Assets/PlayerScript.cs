using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;

    [SerializeField] float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveInput = _moveAction.ReadValue<float>();
        transform.position += new Vector3(moveInput*moveSpeed*Time.deltaTime, 0, 0);
    }
}
