using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _hookThrow;
    InputAction _hookRetrieve;

    PlayerHealth health;

    [Header("Player Movement")]
    Rigidbody2D _rb;
    [SerializeField] JumpCheck jumpCheckScript;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpStr;

    [Header("Hook Reference")]
    [SerializeField] HookScript hookScript;
    private bool hookThrown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        _hookRetrieve = InputSystem.actions.FindAction("HookRetrieve");
        
        _rb = gameObject.GetComponent<Rigidbody2D>();
        health = gameObject.GetComponent<PlayerHealth>();

        hookThrown = false;
    }

    void Update()
    {
        MovePlayer();   
    }

    void FixedUpdate()
    {
        JumpPlayer();
        ThrowHook();
        RetrieveHook();
    }

    void MovePlayer()
    {
        float moveInput = _moveAction.ReadValue<float>();
        if (!hookThrown || hookScript.CheckWithinMaxHookDistance() || (hookScript.transform.position - transform.position).normalized.x * moveInput > 0)
        {
            transform.position += new Vector3(moveInput*moveSpeed*Time.deltaTime, 0, 0);
        }
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
            hookScript.transform.position = transform.position;
            hookScript.Target(mousePos);

            hookThrown = true;
        }
    }

    void RetrieveHook()
    {
        float hookRetrieveInput = _hookRetrieve.ReadValue<float>();
        if (hookRetrieveInput > 0 && hookThrown && !hookScript.BeingThrown())
        {
            hookScript.DrawInHook();
        }
    }

    public void SetHookThrown(bool isHookThrown)
    {
        hookThrown = isHookThrown;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("Health"))
        {
            health.Heal(1);
        }
    }
}
