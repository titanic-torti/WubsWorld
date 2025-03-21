using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Scriptable Objects/Collectibles")]
public class Collectibles : ScriptableObject
{
    [SerializeField] int maxCollectibles;
    int _collectedCollectibles = 0;

    void OnDisable()
    {
        _collectedCollectibles = 0;
    }

    void OnEnable()
    {
        _collectedCollectibles = 0;
    }

    public void IncrementCollectibles()
    {
        _collectedCollectibles += 1;
        if (_collectedCollectibles > maxCollectibles)
        {
            _collectedCollectibles = maxCollectibles;
        }
    }

    public int GetMaxCollectibleCount()
    {
        return maxCollectibles;
    }

    public int GetCollectedCollectibles()
    {
        return _collectedCollectibles;
    }
}
