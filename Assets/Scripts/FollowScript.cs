using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [SerializeField] Transform player;      // position of player
    [SerializeField] float moveSpeed;       // move speed of enemy

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed);
    }
}
