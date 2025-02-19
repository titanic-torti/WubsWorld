using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _hookThrow;
    InputAction _hookRetrieve;

    PlayerHealth health;

    [Header("SFX")]
    [SerializeField] AudioSource soundAnchorDrag;   
    [SerializeField] AudioSource soundAnchorThrow;  
    [SerializeField] AudioSource hurt;              
    [SerializeField] AudioSource step;              

    [Header("Player Movement")]
    Rigidbody2D _rb;
    [SerializeField] JumpCheck jumpCheckScript;
    [SerializeField] float moveStr;
    [SerializeField] float jumpStr;

    [Header("Hook Reference")]
    [SerializeField] HookScript hookScript;
    private LineRenderer _chainLink;
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
        _chainLink = gameObject.GetComponent<LineRenderer>();
        _chainLink.enabled = false;

        hookThrown = false;
    }

    void Update()
    {
        if (hookThrown)
        {
            _chainLink.enabled = true;
        }
        else
        {
            _chainLink.enabled = false;
        }
        _chainLink.SetPositions(new Vector3[] {gameObject.transform.position, hookScript.transform.position});
    }

    void FixedUpdate()
    {
        MovePlayer(); 
        JumpPlayer();
        ThrowHook();
        RetrieveHook();
    }

    void MovePlayer()
    {
        float moveInput = _moveAction.ReadValue<float>();
        if (!hookThrown || hookScript.CheckWithinMaxHookDistance() || (hookScript.transform.position - transform.position).normalized.x * moveInput > 0 && moveInput != 0)
        {
            _rb.AddForce(new Vector3(moveInput*moveStr, 0, 0), ForceMode2D.Force);
            step.Play();
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
            soundAnchorThrow.Play();
        }
    }

    void RetrieveHook()
    {
        float hookRetrieveInput = _hookRetrieve.ReadValue<float>();
        if (hookRetrieveInput > 0 && hookThrown && !hookScript.BeingThrown() && !hookScript.IsLatched())
        {
            hookScript.DrawInHook();
            soundAnchorDrag.Play();
        }
        else if (hookRetrieveInput > 0 && hookThrown && !hookScript.BeingThrown() && hookScript.IsLatched())
        {
            hookScript.UnLatch();
        }
        else if (hookRetrieveInput < 0)
        {
            soundAnchorDrag.Stop();
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
            hurt.Play();
        }
        else if (collision.gameObject.CompareTag("Health"))
        {
            health.Heal(1);
        }
    }
}
