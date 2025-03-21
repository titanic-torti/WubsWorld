using UnityEngine;

public class Levitate : MonoBehaviour
{
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
}
