using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _hookThrow;
    InputAction _hookRetrieve;

    // [Header("Camera Position")]
    // [SerializeField] Camera cam;

    [Header("Player Movement")]
    Rigidbody2D _rb;
    [SerializeField] JumpCheck jumpCheckScript;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStr;

    [Header("Hook Reference")]
    [SerializeField] HookScript hookScript;
    public bool hookThrown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        _hookRetrieve = InputSystem.actions.FindAction("HookRetrieve");
        
        _rb = gameObject.GetComponent<Rigidbody2D>();

        hookThrown = false;
    }

    void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
        ThrowHook();
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

    void ThrowHook()
    {
        float hookThrowInput = _hookThrow.ReadValue<float>();
        if (hookThrowInput > 0 && !hookThrown)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            hookScript.transform.gameObject.SetActive(true);
            hookScript.transform.position = transform.position + new Vector3(1, 1, 0);
            hookScript.Target(mousePos);

            hookThrown = true;
        }
    }

    void RetrieveHook()
    {
        float hookRetrieveInput = _hookRetrieve.ReadValue<float>();
    }
}
