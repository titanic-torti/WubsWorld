using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health health;

    [Header("Heart Visuals")]
    [SerializeField] Image[] hearts;             // hearts container
    [SerializeField] Sprite fullHeart;           // filled heart sprite
    [SerializeField] Sprite emptyHeart;          // empty heart sprite

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++){

            // display full or empty heart
            if (i < health.GetCurrHealth()){
                hearts[i].sprite = fullHeart;
            }else{
                hearts[i].sprite = emptyHeart;
            }

            // display num hearts up to max hearts
            if (i < health.GetMaxHealth()){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
    }
}
