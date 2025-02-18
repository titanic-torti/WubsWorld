using UnityEngine;

public class LionfishScript : MonoBehaviour
{
    [SerializeField] Transform player;              // position of player
    [SerializeField] float distJumpStr;             // move speed of enemy
    [SerializeField] float heightJumpStr;           // how powerfully lionfish jumps into air
    [SerializeField] JumpCheck jumpCheckScript;     // check if lionfish on ground
    [SerializeField] float timeTillPounce;          // the wait time inbetween each pounce

    private float _pounceTimer;
    private float _pounceDirection;
    Rigidbody2D _rb;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _pounceDirection = (player.position-transform.position).normalized.x;
        _pounceTimer = timeTillPounce;
    }

    void FixedUpdate()
    {
        if (jumpCheckScript.IsGrounded())
        {
            _pounceTimer -= Time.deltaTime;
            if (_pounceTimer <= 0)
            {
                _pounceTimer = timeTillPounce;
                _pounceDirection = (player.position-transform.position).normalized.x;
                _rb.AddForce(Vector2.up * heightJumpStr, ForceMode2D.Impulse);
            }
        }
        else
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
