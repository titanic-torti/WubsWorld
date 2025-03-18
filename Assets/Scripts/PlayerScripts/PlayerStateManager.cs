using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateManager : MonoBehaviour
{
    public AnchorStateManager hookScript;               // reference to hook script properties
    [SerializeField] PlayerUpgrades playerUpgrades;     // reference to player upgrades so

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
    public LineRenderer throwPreview;                   // the line drawn showing where the anchor will land before it's thrown

    // ANIMATIONS
    [Header("Fin Animation")]
    public SpriteRenderer anchorSprite;                 // reference to sprite of Wub's anchor when in held state
    // public Animator finAnim;                            // reference to animator of Wub's hand fin
    public SpriteRenderer finSprite;                    // reference to sprite of Wub's hand fin
    public Vector3 finOffset;                           // when player changes direction, Wub is not perfectly aligned, need offset
    public Vector3 anchorOffset;                        // when player changes direction, Wub is not perfectly aligned, need offset

    // SFX
    [Header("SFX")]
    public AudioSource hurt;                            // plays audio when Wub gets hurt
    public AudioSource step;                            // plays audio when Wub walks, loops

    // COMPONENT REFERENCE
    [Header("Component Reference")]
    [HideInInspector] public Rigidbody2D _rb;           // rigidbody of Wub
    PlayerHealth health;                                // health of Wub (script)
    [SerializeField] public SpriteRenderer sprite;     // sprite reference to Wub
    [SerializeField] public Animator anim;             // main animator controller of Wub

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
        _rb = gameObject.GetComponent<Rigidbody2D>();
        health = gameObject.GetComponent<PlayerHealth>();
        _chainLink = gameObject.GetComponent<LineRenderer>();

        // connect to input actions
        _moveAction = InputSystem.actions.FindAction("XMove");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        
        // set start values
        _chainLink.enabled = false;
        throwPreview.enabled = false;
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
        UpdateThrowPreview();
        FlipSprite();
        currState.UpdateState(this);
    }

    void UpdateAnchor()
    {
        if (hookThrown || !playerUpgrades.hasAnchor)
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

    void UpdateThrowPreview()
    {
        if (Mouse.current.leftButton.isPressed && !hookThrown && playerUpgrades.hasAnchor)
        {
            Vector2 origin = _rb.position;
            Vector2 destination;

            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 diff = worldMousePos - _rb.position;
            
            RaycastHit2D raycast = Physics2D.Raycast(origin, diff.normalized, hookScript.maxAnchorDist);

            if (raycast.collider == null)
            {
                // if nothing is hit, limit preview distance to max anchor throw distance
                float distance = Math.Min(diff.magnitude, hookScript.maxAnchorDist);
                destination = _rb.position + (diff.normalized * distance);
            }
            else
            {
                if (raycast.collider.tag == "Anchor Point")
                {
                    Transform anchorPoint = raycast.collider.GetComponent<Transform>();
                    destination = anchorPoint.position;
                }
                else
                {
                    destination = raycast.point;
                }
            }

            throwPreview.SetPositions(new Vector3[] { _rb.position, destination });
            throwPreview.enabled = true;

            hookScript._currTarget = destination;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            throwPreview.enabled = false;
        }
    }

    void FixedUpdate()
    {
        // ThrowHook();
        currState.FixedUpdateState(this);
    }

    void FlipSprite()
    {
        // flip sprite if facing wrong direction of movement
        float moveInput = _moveAction.ReadValue<float>();
        anim.SetFloat("movement", Mathf.Abs(moveInput));
        if ((moveInput > 0 && sprite.flipX) || (moveInput < 0 && !sprite.flipX))
        {
            sprite.flipX = !sprite.flipX;
            finSprite.flipX = !finSprite.flipX;
            if (finSprite.flipX)
            {
                finSprite.transform.position += finOffset;
                anchorSprite.transform.position += anchorOffset;
                anchorSprite.transform.Rotate(Vector3.forward, 90);
            }
            else
            {
                finSprite.transform.position -= finOffset;
                anchorSprite.transform.position -= anchorOffset;
                anchorSprite.transform.Rotate(Vector3.forward, -90);
            }
        }
    }

    // void ThrowHook()
    // {
    //     // float hookThrowInput = _hookThrow.ReadValue<float>();
    //     if (Mouse.current.leftButton.wasReleasedThisFrame && !hookThrown)
    //     {
    //         anim.SetTrigger("throw");
    //         finAnim.SetTrigger("throw");
    //     }
    // }

    public PlayerBaseState GetState()
    {
        return currState;
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
        if (collider.gameObject.CompareTag("Checkpoint"))
        {
            health.SetCheckpoint(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag("Respawn"))
        {
            health.Respawn();
        }
        currState.OnTriggerEnter2D(this, collider);
    }
}
