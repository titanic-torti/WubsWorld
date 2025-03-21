// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerScript : MonoBehaviour
// {
//     [SerializeField] AnchorStateManager hookScript;         // reference to hook script properties

//     // PLAYER PROPERTIES
//     [Header("Player Movement")]
//     [SerializeField] JumpCheck jumpCheckScript;             // reference to script that checks if player is touching ground
//     [SerializeField] float moveStr;                         // strength of player movement left and right
//     [SerializeField] float jumpStr;                         // how high the player jumps

//     // ANIMATIONS
//     [Header("Fin Animation")]
//     [SerializeField] SpriteRenderer anchorSprite;           // reference to sprite of Wub's anchor when in held state
//     [SerializeField] Animator finAnim;                      // reference to animator of Wub's hand fin
//     [SerializeField] SpriteRenderer finSprite;              // reference to sprite of Wub's hand fin
//     [SerializeField] Vector3 finOffset;                     // when player changes direction, Wub is not perfectly aligned, need offset

//     // SFX
//     [Header("SFX")]
//     [SerializeField] AudioSource hurt;                      // plays audio when Wub gets hurt
//     [SerializeField] AudioSource step;                      // plays audio when Wub walks, loops

//     // COMPONENT REFERENCE
//     Rigidbody2D _rb;                                        // rigidbody of Wub
//     PlayerHealth health;                                    // health of Wub (script)
//     SpriteRenderer sprite;                                  // sprite reference to Wub
//     Animator anim;                                          // main animator controller of Wub

//     InputAction _moveAction;                                // checks for move input
//     InputAction _jumpAction;                                // checks for jump input
//     InputAction _hookThrow;                                 // checks for hook throw input

//     LineRenderer _chainLink;                                // reference to line that visually connects Wub and anchor
//     [HideInInspector] public bool hookThrown;               // bool to determine if hook is thrown or not

//     // ----------------------------------------------------------------------------------------
//     // FUNCTIONS
//     void Start()
//     {
//         sprite = gameObject.GetComponent<SpriteRenderer>();
//         anim = gameObject.GetComponent<Animator>();

//         _moveAction = InputSystem.actions.FindAction("XMove");
//         _jumpAction = InputSystem.actions.FindAction("Jump");
//         _hookThrow = InputSystem.actions.FindAction("HookThrow");
        
//         _rb = gameObject.GetComponent<Rigidbody2D>();
//         health = gameObject.GetComponent<PlayerHealth>();
//         health.rb = _rb;
//         _chainLink = gameObject.GetComponent<LineRenderer>();
//         _chainLink.enabled = false;

//         hookThrown = false;
//     }

//     void Update()
//     {
//         UpdateAnchor();
//         UpdateChain();
//         UpdateAnimationGrounded();
//     }

//     void UpdateAnchor()
//     {
//         anchorSprite.flipY = sprite.flipX;
//         anchorSprite.flipX = sprite.flipX;
//         if (hookThrown)
//         {
//             anchorSprite.enabled = false;
//         }
//         else
//         {
//             anchorSprite.enabled = true;
//         }
//     }

//     void UpdateChain()
//     {
//         if (hookThrown)
//         {
//             _chainLink.enabled = true;
//         }
//         else
//         {
//             _chainLink.enabled = false;
//         }
//         _chainLink.SetPositions(new Vector3[] {gameObject.transform.position, hookScript.transform.position});
//     }

//     void UpdateAnimationGrounded()
//     {
//         if (jumpCheckScript.IsGrounded())
//         {
//             anim.SetBool("grounded", true);
//         }
//         else
//         {
//             anim.SetBool("grounded", false);
//         }
//     }

//     void FixedUpdate()
//     {
//         MovePlayer(); 
//         JumpPlayer();
//         ThrowHook();
//     }

//     void MovePlayer()
//     {
//         float moveInput = _moveAction.ReadValue<float>();
//         anim.SetFloat("movement", Mathf.Abs(moveInput));
//         if (!hookThrown || hookScript.CheckWithinMaxAnchorDist() || (hookScript.transform.position - transform.position).normalized.x * moveInput > 0)
//         {
//             // flip sprite if facing wrong direction of movement
//             if ((moveInput > 0 && !sprite.flipX) || (moveInput < 0 && sprite.flipX))
//             {
//                 sprite.flipX = !sprite.flipX;
//                 finSprite.flipX = !finSprite.flipX;
//                 if (finSprite.flipX)
//                 {
//                     finSprite.transform.position += finOffset;
//                     anchorSprite.transform.position += finOffset;
//                 }
//                 else
//                 {
//                     finSprite.transform.position -= finOffset;
//                     anchorSprite.transform.position -= finOffset;
//                 }
//             }
//             _rb.AddForce(new Vector3(moveInput*moveStr - _rb.linearVelocity.x, 0, 0), ForceMode2D.Force);
//             if (step.isPlaying)
//             {
//                 step.Play();
//             }
//         }
//         else
//         {
//             step.Stop();
//         }
//     }

//     void JumpPlayer()
//     {
//         float jumpInput = _jumpAction.ReadValue<float>();
//         if (jumpInput > 0 && jumpCheckScript.IsGrounded())
//         {
//             anim.SetTrigger("jump");
//             _rb.AddForce(Vector2.up * jumpStr, ForceMode2D.Impulse);
//         }
//     }

//     void ThrowHook()
//     {
//         float hookThrowInput = _hookThrow.ReadValue<float>();
//         if (hookThrowInput > 0 && !hookThrown)
//         {
//             anim.SetTrigger("throw");
//             finAnim.SetTrigger("throw");
//         }
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Enemy"))
//         {
//             health.TakeDamage(1);
//             hurt.Play();
//         }
//         else if (collision.gameObject.CompareTag("Health"))
//         {
//             health.Heal(1);
//         }
//     }

//     void OnTriggerEnter2D(Collider2D collider)
//     {
//         if (collider.gameObject.CompareTag("Respawn"))
//         {
//             health.Respawn();
//         }
//     } 
// }
