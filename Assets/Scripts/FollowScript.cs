using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [SerializeField] Transform player;      // position of player
    [SerializeField] float moveSpeed;       // move speed of enemy

    void Update()
    {
        transform.position += new Vector3((player.position-transform.position).normalized.x, 0, 0) * moveSpeed * Time.deltaTime;
    }
}
