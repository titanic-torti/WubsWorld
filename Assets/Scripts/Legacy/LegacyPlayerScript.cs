using UnityEngine;
using UnityEngine.InputSystem;

public class LegacyPlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _hookThrow;
    InputAction _hookRetrieve;

    PlayerHealth health;

    [Header("Fin Animation")]
    [SerializeField] Animator finAnim;
    [SerializeField] SpriteRenderer finSprite;
    [SerializeField] Vector3 finOffset;
    [SerializeField] SpriteRenderer anchorSprite;
    SpriteRenderer sprite;
    Animator anim;

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
    [SerializeField] LegacyAnchorScript hookScript;
    private LineRenderer _chainLink;
    private bool hookThrown;
    private float _hookThrownRecentlyTimer;
    [SerializeField] float hookRetrievedRecentlyAddTime;
    [SerializeField] float preventDoubleClickHookThrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        _hookRetrieve = InputSystem.actions.FindAction("HookRetrieve");
        
        _rb = gameObject.GetComponent<Rigidbody2D>();
        health = gameObject.GetComponent<PlayerHealth>();
        _chainLink = gameObject.GetComponent<LineRenderer>();
        _chainLink.enabled = false;

        hookThrown = false;
        _hookThrownRecentlyTimer = 0;
    }

    void Update()
    {
        UpdateAnchor();
        UpdateChain();
        UpdateAnimationGrounded();
        UpdateTimers();
    }

    void UpdateAnchor()
    {
        anchorSprite.flipY = sprite.flipX;
        anchorSprite.flipX = sprite.flipX;
        if (hookThrown)
        {
            anchorSprite.enabled = false;
        }
        else
        {
            anchorSprite.enabled = true;
        }
    }

    void UpdateChain()
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

    void UpdateAnimationGrounded()
    {
        if (jumpCheckScript.IsGrounded())
        {
            anim.SetBool("grounded", true);
        }
        else
        {
            anim.SetBool("grounded", false);
        }
    }

    void UpdateTimers()
    {
        if (_hookThrownRecentlyTimer > 0)
        {
            _hookThrownRecentlyTimer -= Time.deltaTime;
        }
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
        anim.SetFloat("movement", Mathf.Abs(moveInput));
        if (!hookThrown || hookScript.CheckWithinMaxHookDistance() || (hookScript.transform.position - transform.position).normalized.x * moveInput > 0 && moveInput != 0)
        {
            // flip sprite if facing wrong direction of movement
            if ((moveInput > 0 && !sprite.flipX) || (moveInput < 0 && sprite.flipX))
            {
                sprite.flipX = !sprite.flipX;
                finSprite.flipX = !finSprite.flipX;
                if (finSprite.flipX)
                {
                    finSprite.transform.position += finOffset;
                    anchorSprite.transform.position += finOffset;
                }
                else
                {
                    finSprite.transform.position -= finOffset;
                    anchorSprite.transform.position -= finOffset;
                }
            }
            _rb.AddForce(new Vector3(moveInput*moveStr - _rb.linearVelocity.x, 0, 0), ForceMode2D.Force);
            if (step.isPlaying)
            {
                step.Play();
            }
        }
        else
        {
            step.Stop();
        }
    }

    void JumpPlayer()
    {
        float jumpInput = _jumpAction.ReadValue<float>();
        if (jumpInput > 0 && jumpCheckScript.IsGrounded())
        {
            anim.SetTrigger("jump");
            _rb.AddForce(Vector2.up * jumpStr, ForceMode2D.Impulse);
        }
    }

    void ThrowHook()
    {
        float hookThrowInput = _hookThrow.ReadValue<float>();
        if (hookThrowInput > 0 && !hookThrown && _hookThrownRecentlyTimer <= 0)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            hookScript.transform.gameObject.SetActive(true);
            hookScript.transform.position = transform.position;
            hookScript.Target(mousePos);

            hookThrown = true;
            anim.SetTrigger("throw");
            finAnim.SetTrigger("throw");
            soundAnchorThrow.Play();
            _hookThrownRecentlyTimer = preventDoubleClickHookThrow;
        }

        else if (hookThrowInput > 0 && _hookThrownRecentlyTimer <= 0)
        {
            hookScript.transform.gameObject.SetActive(false);
            hookScript.transform.position = transform.position;
            hookThrown = false;
            _hookThrownRecentlyTimer = hookRetrievedRecentlyAddTime + preventDoubleClickHookThrow;
        }
    }

    void RetrieveHook()
    {
        float hookRetrieveInput = _hookRetrieve.ReadValue<float>();
        if (hookRetrieveInput > 0 && hookThrown && !hookScript.BeingThrown() && !hookScript.IsLatched())
        {
            hookScript.DrawInHook();
            if (!soundAnchorDrag.isPlaying)
            {
                soundAnchorDrag.Play();
            }
        }
        else if (hookRetrieveInput > 0 && hookThrown && !hookScript.BeingThrown() && hookScript.IsLatched())
        {
            hookScript.UnLatch();
        }
        else if (hookRetrieveInput <= 0 && hookThrown && !hookScript.BeingThrown())
        {
            hookScript.StopDrawInHook();
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
