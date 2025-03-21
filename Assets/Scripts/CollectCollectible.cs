using UnityEngine;

public class CollectCollectible : MonoBehaviour
{
    [SerializeField] Collectibles collectibles;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collectibles.IncrementCollectibles();
            Destroy(gameObject);
        }
    }
}
