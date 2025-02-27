using UnityEngine;

public class HookScript : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;         // reference to player script properties

    [Header("SFX")]
    [SerializeField] AudioSource soundAnchorHit;    
    [SerializeField] AudioSource soundAnchorMiss;
    [SerializeField] AudioSource soundEnemyHurt;

    [Header("Hook Properties")]
    Rigidbody2D _rb;
    DistanceJoint2D _dj;
    [SerializeField] float hookTossSpeed;               // how fast the hook is tossed out
    [SerializeField] float hookRetrieveSpeed;           // how fast the hook is pulled back in
    [SerializeField] float maxAnchorDist;               // furthest distance anchor can be from player
    [SerializeField] float closenessBounds;             // how close hook needs to be to target click before being registered as fully thrown
    [SerializeField] float timeRecoverFromLatch;        // time till anchor checks to latch to any new anchor points

    private Vector3 _currTarget;                        // the last clicked spot to target for throw
    private bool _beingThrown;                          // bool to check if the anchor is currently in process of being thrown
    private bool _latched;                              // bool to check if anchor is latched to anchor point
    private float _latchTimer;                          // tracks time since last latch to an anchor point
    

    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _dj = gameObject.GetComponent<DistanceJoint2D>();
        _dj.enabled = false;
        _beingThrown = false;
        _latched = false;
    }

    void Update()
    {
        BeingThrownToTarget();

        if (_latchTimer > 0)
        {
            _latchTimer -= Time.deltaTime;
        }
    }

    void BeingThrownToTarget()
    {
        if ((_beingThrown && WithinThrow(_currTarget)) || !CheckWithinMaxHookDistance())
        {
            _beingThrown = false;
            _rb.simulated = true;
            soundAnchorMiss.Play();
        }
        else if (_beingThrown)
        {
            transform.position = Vector2.MoveTowards(transform.position, _currTarget, hookTossSpeed * Time.deltaTime);
        }
    }

    bool WithinThrow(Vector2 target)
    {
        return closenessBounds >= (new Vector2(transform.position.x, transform.position.y) - target).magnitude;
    }

    public void Target(Vector2 target)
    {
        _rb.simulated = false;
        _beingThrown = true;
        _currTarget = target;
    }

    public void DrawInHook()
    {
        _rb.simulated = false;
        transform.position = Vector2.MoveTowards(transform.position, playerScript.transform.position, hookRetrieveSpeed * Time.deltaTime);
    }

    public void StopDrawInHook()
    {
        _rb.simulated = true;
    }

    public bool CheckWithinMaxHookDistance()
    {
        return maxAnchorDist >= (playerScript.transform.position - transform.position).magnitude;
    }

    public bool BeingThrown()
    {
        return _beingThrown;
    }

    public bool IsLatched()
    {
        return _latched;
    }

    public void Latch()
    {
        print("latched");
        _latched = true;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        _dj.enabled = true;
    }

    public void UnLatch()
    {
        print("unlatched");
        _latched = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.constraints = RigidbodyConstraints2D.None;
        _dj.enabled = false;
        _latchTimer = timeRecoverFromLatch;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerScript.SetHookThrown(false);
            gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            print("play enemy hurt audio");
            soundEnemyHurt.Play();
        }
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Anchor Point") && _latchTimer <= 0)
        {
            transform.position = collider.transform.position;
            Latch();
            soundAnchorHit.Play();
        }
    }
}
