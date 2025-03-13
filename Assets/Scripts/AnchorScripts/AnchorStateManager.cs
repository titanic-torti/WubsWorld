using UnityEngine;
using UnityEngine.InputSystem;

public class AnchorStateManager : MonoBehaviour
{
    public PlayerStateManager playerScript;         // reference to player script properties
    public PlayerUpgrades playerUpgrades;           // reference to player upgrades so

    // ANCHOR PROPERTIES
    [Header("Anchor Properties")]
    public float hookTossSpeed;                     // how fast the hook is tossed out
    public float hookRetrieveSpeed;                 // how fast the hook is pulled back in
    public float maxAnchorDist;                     // furthest distance anchor can be from player
    public float closenessBounds;                   // how close hook needs to be to target click before being registered as fully thrown
    public float timeRecoverFromLatch;              // time till anchor checks to latch to any new anchor points
    public float rappelSpeed;                       // speed at which Wub can rappel up and down while latched
    public float unlatchMomentumBonus;              // the boost to movement right after unlatching from anchor point

    // STATES
    AnchorBaseState currState;
    public AnchorHeldState HeldState = new AnchorHeldState();
    public AnchorIdleState IdleState = new AnchorIdleState();
    public AnchorTossState TossState = new AnchorTossState();
    public AnchorRetrieveState RetrieveState = new AnchorRetrieveState();
    public AnchorLatchedState LatchedState = new AnchorLatchedState();

    // SFX
    [Header("SFX")]
    public AudioSource soundAnchorHit;    
    public AudioSource soundAnchorMiss;
    public AudioSource soundEnemyHurt;
    public AudioSource soundAnchorDrag;
    public AudioSource soundAnchorThrow;

    // COMPONENT REFERENCE
    [HideInInspector] public InputAction _hookThrow;
    [HideInInspector] public InputAction _hookRetrieve;
    [HideInInspector] public InputAction _unlatchHook;
    [HideInInspector] public InputAction _rappelUp;
    [HideInInspector] public InputAction _rappelDown;

    [HideInInspector] public Rigidbody2D _rb;
    [HideInInspector] public DistanceJoint2D _dj;
    [HideInInspector] public SpriteRenderer _sr;

    [HideInInspector] public Vector3 _currTarget;           // the last clicked spot to target for throw
    [HideInInspector] public float _latchTimer;             // tracks time since last latch to an anchor point

    // ----------------------------------------------------------------------------------------
    // FUNCTIONS
    void Awake()
    {
        _hookThrow = InputSystem.actions.FindAction("HookThrow");
        _hookRetrieve = InputSystem.actions.FindAction("HookRetrieve");
        _unlatchHook = InputSystem.actions.FindAction("Jump");
        _rappelUp = InputSystem.actions.FindAction("RappelUp");
        _rappelDown = InputSystem.actions.FindAction("RappelDown");

        _rb = gameObject.GetComponent<Rigidbody2D>();
        _dj = gameObject.GetComponent<DistanceJoint2D>();
        _sr = gameObject.GetComponent<SpriteRenderer>();

        _latchTimer = 0;
    }

    void Start()
    {
        currState = HeldState;
        currState.EnterState(this);
    }

    public void SwitchState(AnchorBaseState state)
    {
        currState = state;
        currState.EnterState(this);
    }

    void Update()
    {
        // print("I am currently in the " + currState);
        UpdateTimers();
        currState.UpdateState(this);
    }

    void UpdateTimers()
    {
        if (_latchTimer > 0)
        {
            _latchTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        currState.FixedUpdateState(this);
    }

    public void PlayAudio(AudioSource audio)
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }

    public bool CheckWithinMaxAnchorDist()
    {
        return maxAnchorDist >= (playerScript.transform.position - transform.position).magnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currState.OnCollisionEnter2D(this, collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        currState.OnTriggerEnter2D(this, collider);
    }
}
