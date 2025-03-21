using UnityEngine;

public class DinosaurToasterScript : MonoBehaviour
{
    [Header("Player Feedback")]
    [SerializeField] Transform player;              // position of player
    [SerializeField] float detectionRadius;         // distance for lion fish to start following player

    [Header("DinosaurToaster Behavior")]
    [SerializeField] JumpCheck jumpCheckScript;     // check if lionfish on ground
    [SerializeField] float distJumpStr;             // move speed of enemy
    [SerializeField] float heightJumpStr;           // how powerfully lionfish jumps into air
    [SerializeField] float timeTillPounce;          // the wait time inbetween each pounce

    private float _pounceTimer;
    private float _pounceDirection;
    Rigidbody2D _rb;
    SpriteRenderer _sprite;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _pounceDirection = (player.position-transform.position).normalized.x;
        _pounceTimer = timeTillPounce;
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float distanceFromPlayer = (player.position-transform.position).magnitude;
        if (jumpCheckScript.IsGrounded() && distanceFromPlayer <= detectionRadius)
        {
            _pounceTimer -= Time.deltaTime;
            if (_pounceTimer <= 0)
            {
                // change sprite direction to face player
                if ((player.position-transform.position).magnitude > 0)
                {
                    _sprite.flipX = true;
                }
                else
                {
                    _sprite.flipX = false;
                }
                _pounceTimer = timeTillPounce;
                _pounceDirection = (player.position-transform.position).normalized.x;
                _rb.AddForce(Vector2.up * heightJumpStr, ForceMode2D.Impulse);
            }
        }
        else if (distanceFromPlayer <= detectionRadius)
        {
            _rb.AddForce(new Vector3(_pounceDirection, 0, 0) * distJumpStr, ForceMode2D.Force);
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Anchor"))
        {
            Destroy(gameObject);
        }
    }
}
