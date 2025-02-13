using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;

    Rigidbody2D _rb;
    [SerializeField] JumpCheck jumpCheckScript;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // _isGrounded = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 0.05f);
        // if(_isGrounded)
        // {
        //     MovePlayer();
        //     JumpPlayer();
        // }

    }

    void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
    }

    void MovePlayer()
    {
        float moveInput = _moveAction.ReadValue<float>();
        transform.position += new Vector3(moveInput*moveSpeed , 0, 0);
    }

    void JumpPlayer()
    {
        float jumpInput = _jumpAction.ReadValue<float>();
        if (jumpInput > 0 && jumpCheckScript.IsGrounded())
        {
            _rb.AddForce(Vector2.up * jumpStr, ForceMode2D.Impulse);
        }
    }
}
