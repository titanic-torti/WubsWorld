using UnityEngine;

public class BlobFishScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private bool _direction;

    void Update()
    {
        if (_direction)
        {
            transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }
        else 
        {
            transform.position -= new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Anchor"))
        {
            Destroy(gameObject);
        }
        else
        {
            _direction = !_direction;
        }
    }
}
