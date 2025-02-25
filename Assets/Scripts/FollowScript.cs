using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioSource soundCrabStep;
    [SerializeField] float crabStepTimerSet;
    private float _crabStepTimer;

    [Header("Player Feedback")]
    [SerializeField] Transform player;      // position of player
    [SerializeField] float detectionRadius; // distance for crab to start following player

    [Header("Crab Behavior")]
    [SerializeField] float moveSpeed;       // move speed of enemy

    void Start()
    {
        _crabStepTimer = crabStepTimerSet;
    }

    void Update()
    {
        float distanceFromPlayer = (player.position-transform.position).magnitude;
        if (distanceFromPlayer <= detectionRadius)
        {
            transform.position += moveSpeed * Time.deltaTime * new Vector3((player.position-transform.position).normalized.x, 0, 0);
        }

        if (_crabStepTimer <= 0 && !soundCrabStep.isPlaying)
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
            Destroy(gameObject);
        }
    }
}
