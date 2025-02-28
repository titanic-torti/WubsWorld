using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _hookThrow;

    PlayerHealth health;

    [Header("Fin Animation")]
    [SerializeField] Animator finAnim;
    [SerializeField] SpriteRenderer finSprite;
    [SerializeField] Vector3 finOffset;
    [SerializeField] SpriteRenderer anchorSprite;
    SpriteRenderer sprite;
    Animator anim;

    [Header("SFX")]
    [SerializeField] AudioSource hurt;              
    [SerializeField] AudioSource step;              

    [Header("Player Movement")]
    Rigidbody2D _rb;
    [SerializeField] JumpCheck jumpCheckScript;
    [SerializeField] float moveStr;
    [SerializeField] float jumpStr;

    [Header("Hook Reference")]
    [SerializeField] AnchorStateManager hookScript;
    private LineRenderer _chainLink;
    [HideInInspector] public bool hookThrown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        
        _rb = gameObject.GetComponent<Rigidbody2D>();
        health = gameObject.GetComponent<PlayerHealth>();
        _chainLink = gameObject.GetComponent<LineRenderer>();
        _chainLink.enabled = false;

        hookThrown = false;
    }

    void Update()
    {
        UpdateAnchor();
        UpdateChain();
        UpdateAnimationGrounded();
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

    void FixedUpdate()
    {
        MovePlayer(); 
        JumpPlayer();
        ThrowHook();
    }

    void MovePlayer()
    {
        float moveInput = _moveAction.ReadValue<float>();
        anim.SetFloat("movement", Mathf.Abs(moveInput));
        if (!hookThrown || hookScript.CheckWithinMaxAnchorDist() || (hookScript.transform.position - transform.position).normalized.x * moveInput > 0)
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
        if (hookThrowInput > 0 && !hookThrown)
        {
            anim.SetTrigger("throw");
            finAnim.SetTrigger("throw");
        }
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
