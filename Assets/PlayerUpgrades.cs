using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrades", menuName = "Scriptable Objects/PlayerUpgrades")]
public class PlayerUpgrades : ScriptableObject
{
    public bool hasAnchor;

    void OnDisable()
    {
        hasAnchor = false;
    }

    void OnEnable()
    {
        hasAnchor = false;
    }
}
