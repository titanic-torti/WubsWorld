using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "Scriptable Objects/Health")]
public class Health : ScriptableObject
{
    [SerializeField] int maxHealth;
    [SerializeField] int health;

    void OnEnable()
    {
        health = maxHealth;
    }

    void OnDisable()
    {
        health = maxHealth;
    }

    public int GetCurrHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void UpdateHealth(int newHealth)
    {
        if (newHealth > maxHealth)
        {
            health = maxHealth;
        }
        else if (newHealth < 0)
        {
            health = 0;
        }
        else
        {
            health = newHealth;
        }
    }
}
