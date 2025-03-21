using UnityEngine;
using UnityEngine.Events;

public class Upgrades : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] PlayerUpgrades playerUpgrades;
    [SerializeField] GameObject aura;

    [Header("Float Properties")]
    [SerializeField] float maxOffSetFromStart;
    [SerializeField] float floatSpeed;

    private bool _reachBottom;
    private Vector2 _startposition;

    void Start()
    {
        _startposition = gameObject.transform.position;
        _reachBottom = false;
    }

    void Update()
    {
        if (_reachBottom)
        {
            gameObject.transform.position -= new Vector3(0, floatSpeed, 0) * Time.deltaTime;
        }
        else
        {
            gameObject.transform.position += new Vector3(0, floatSpeed, 0) * Time.deltaTime;
        }

        if ((gameObject.transform.position.y >= _startposition.y + maxOffSetFromStart && !_reachBottom) ||
            (gameObject.transform.position.y <= _startposition.y - maxOffSetFromStart && _reachBottom))
        {
            _reachBottom = !_reachBottom;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerUpgrades.UnlockAnchor();
            playerUpgrades.AllowRappel();
            Destroy(aura);
            Destroy(gameObject);
        }
    }
}
