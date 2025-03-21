using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrades", menuName = "Scriptable Objects/PlayerUpgrades")]
public class PlayerUpgrades : ScriptableObject
{
    public bool hasAnchor;
    public bool extendRange;
    public bool allowRappel;

    void OnDisable()
    {
        hasAnchor = false;
        allowRappel = false;
        extendRange = false;
    }

    void OnEnable()
    {
        hasAnchor = false;
        allowRappel = false;
        extendRange = false;
    }

    public void UnlockAnchor()
    {
        hasAnchor = true;
    }

    public void ExtendRange()
    {
        extendRange = true;
    }

    public void AllowRappel()
    {
        allowRappel = true;
    }

    public void ResetUpgrades()
    {
        hasAnchor = false;
        allowRappel = false;
        extendRange = false;
    }
}
