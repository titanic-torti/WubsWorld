using UnityEngine;

public class Thundercloud : MonoBehaviour
{
    [Header("Player Feedback")]
    [SerializeField] Transform player;      // position of player
    [SerializeField] float detectionRadius; // distance for crab to start following player

    [Header("Thundercloud Behavior")]
    [SerializeField] float moveSpeed;       // move speed of enemy


    void Update()
    {
        float distanceFromPlayer = (player.position-transform.position).magnitude;
        if (distanceFromPlayer <= detectionRadius)
        {
            transform.position += moveSpeed * Time.deltaTime * new Vector3((player.position-transform.position).normalized.x, 0, 0);
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
