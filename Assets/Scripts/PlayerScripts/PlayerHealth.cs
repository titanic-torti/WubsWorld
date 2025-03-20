using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Rigidbody2D rb;
    public Canvas deathScreen;

    [Header("Heart Visuals")]
    [SerializeField] Image[] hearts;             // hearts container
    [SerializeField] Sprite fullHeart;           // filled heart sprite
    [SerializeField] Sprite emptyHeart;          // empty heart sprite

    [Header("Heart Attributes")]
    [SerializeField] int currHealth;
    [SerializeField] int maxHealth;
    [SerializeField] float invulnerabilityFrame; // time player is invulnerable to damage after taking damage
    private float _invulnerabilityTimer;         // track time since last hit
    
    private Transform currCheckpoint;            // respawn location

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++){

            // display full or empty heart
            if (i < currHealth){
                hearts[i].sprite = fullHeart;
            }else{
                hearts[i].sprite = emptyHeart;
            }

            // display num hearts up to max hearts
            if (i < maxHealth){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }

            // modulate invulnerability timer
            if (_invulnerabilityTimer > 0)
            {
                _invulnerabilityTimer -= Time.deltaTime;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (_invulnerabilityTimer <= 0)
        {
            currHealth -= amount;
            _invulnerabilityTimer = invulnerabilityFrame;
        }

        if (currHealth <= 0)
        {
            Respawn();
        }
    }

    public void Heal(int amount)
    {
        currHealth += amount;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
    }

    public void FullHeal()
    {
        currHealth = maxHealth;
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        currCheckpoint = checkpoint.GetComponent<Transform>();
    }

    public void Respawn(bool instantRespawn = false)
    {
        if (!instantRespawn) {
            deathScreen.enabled = true;

            StartCoroutine(ShowDeathScreen());
            Time.timeScale = 0;
        }

        FullHeal();
        
        if (currCheckpoint != null) {
            rb.position = currCheckpoint.position;
        } else {
            // fallback to reloading the scene if a checkpoint isn't assigned for whatever reason
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator ShowDeathScreen()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        Time.timeScale = 1;
        deathScreen.enabled = false;
    }
}
