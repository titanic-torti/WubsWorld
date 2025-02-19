using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioSource soundCrabHurt;
    [SerializeField] AudioSource soundCrabStep;
    [SerializeField] float crabStepTimerSet;
    private float _crabStepTimer;

    [SerializeField] Transform player;      // position of player
    [SerializeField] float moveSpeed;       // move speed of enemy

    void Start()
    {
        _crabStepTimer = crabStepTimerSet;
    }

    void Update()
    {
        transform.position += new Vector3((player.position-transform.position).normalized.x, 0, 0) * moveSpeed * Time.deltaTime;

        if (_crabStepTimer <= 0)
        {
            soundCrabStep.Play();
            _crabStepTimer = crabStepTimerSet;
        }
        else
        {
            _crabStepTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Anchor"))
        {
            soundCrabHurt.Play();
            Destroy(gameObject);
        }
    }
}
