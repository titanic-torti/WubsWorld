using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    public AnchorStateManager hookScript;         // reference to hook script properties

    // STATES
    PlayerBaseState currState;
    public PlayerMoveState MoveState = new PlayerMoveState();
    public PlayerJumpingState JumpingState = new PlayerJumpingState();
    public PlayerFallingState FallingState = new PlayerFallingState();

    // PLAYER PROPERTIES
    [Header("Player Movement")]
    public JumpCheck jumpCheckScript;                   // reference to script that checks if player is touching ground
    public float moveStr;                               // strength of player movement left and right
    public float jumpStr;                               // how high the player jumps
    public float initialJumpStr;                        // strength of initial jump impulse
    public float maxJumpTime;                           // how long player can rise while holding jump button

    // ANIMATIONS
    [Header("Fin Animation")]
    public SpriteRenderer anchorSprite;                 // reference to sprite of Wub's anchor when in held state
    public Animator finAnim;                            // reference to animator of Wub's hand fin
    public SpriteRenderer finSprite;                    // reference to sprite of Wub's hand fin
    public Vector3 finOffset;                           // when player changes direction, Wub is not perfectly aligned, need offset

    // SFX
    [Header("SFX")]
    public AudioSource hurt;                            // plays audio when Wub gets hurt
    public AudioSource step;                            // plays audio when Wub walks, loops

    // COMPONENT REFERENCE
    [HideInInspector] public Rigidbody2D _rb;           // rigidbody of Wub
    PlayerHealth health;                                // health of Wub (script)
    [HideInInspector] public SpriteRenderer sprite;     // sprite reference to Wub
    [HideInInspector] public Animator anim;             // main animator controller of Wub

    [HideInInspector] public InputAction _moveAction;   // checks for move input
    [HideInInspector] public InputAction _jumpAction;   // checks for jump input
    [HideInInspector] public InputAction _hookThrow;    // checks for hook throw input

    LineRenderer _chainLink;                            // reference to line that visually connects Wub and anchor
    [HideInInspector] public bool hookThrown;           // bool to determine if hook is thrown or not

    // ----------------------------------------------------------------------------------------
    // FUNCTIONS
    void Start()
    {
        // get components
        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

        _rb = gameObject.GetComponent<Rigidbody2D>();
        health = gameObject.GetComponent<PlayerHealth>();
        _chainLink = gameObject.GetComponent<LineRenderer>();

        // connect to input actions
        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        
        // set start values
        _chainLink.enabled = false;
        hookThrown = false;

        // set current state
        currState = MoveState;
        currState.EnterState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currState = state;
        currState.EnterState(this);
    }

    void Update()
    {
        UpdateAnchor();
        UpdateChain();
        UpdateAnimationGrounded();
        currState.UpdateState(this);
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
        currState.FixedUpdateState(this);
    }

    void MovePlayer()
    {
        
    }

    void JumpPlayer()
    {
        
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
        currState.OnCollisionEnter2D(this, collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        currState.OnTriggerEnter2D(this, collider);
    }
}
