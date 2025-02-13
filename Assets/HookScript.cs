using UnityEngine;

public class HookScript : MonoBehaviour
{
    [SerializeField] PlayerScript player;               // reference to player script properties

    [Header("Hook Properties")]
    Rigidbody2D _rb;
    [SerializeField] float hookTossSpeed;               // how fast the hook is tossed out
    [SerializeField] float closenessBounds;             // how close hook needs to be to target click before being registered as fully thrown

    private Vector3 _currTarget;                        // the last clicked spot to target for throw
    private bool _beingThrown;                          // bool to check if the anchor is currently in process of being thrown

    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _beingThrown = false;
    }

    void Update()
    {
        BeingThrownToTarget();
    }

    void BeingThrownToTarget()
    {
        if (_beingThrown && WithinThrow(_currTarget))
        {
            _beingThrown = false;
            _rb.simulated = true;
        }
        else if (_beingThrown)
        {
            transform.position = Vector2.MoveTowards(transform.position, _currTarget, hookTossSpeed);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            player.hookThrown = false;
            gameObject.SetActive(false);
        }
    }
}
