using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Canvas deathScreen;
    [SerializeField] Health health;
    [SerializeField] Rigidbody2D anchorRb;

    [SerializeField] float invulnerabilityFrame;    // time player is invulnerable to damage after taking damage
    float _invulnerabilityTimer;                    // track time since last hit
    
    Rigidbody2D _rb;
    Transform currCheckpoint;                       // respawn location

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        // modulate invulnerability timer
        if (_invulnerabilityTimer > 0)
        {
            _invulnerabilityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int amount)
    {
        if (_invulnerabilityTimer <= 0)
        {
            health.UpdateHealth(health.GetCurrHealth() - amount);
            _invulnerabilityTimer = invulnerabilityFrame;
        }

        if (health.GetCurrHealth() <= 0)
        {
            Respawn();
        }
    }

    public void Heal(int amount)
    {
        health.UpdateHealth(health.GetCurrHealth() + amount);
    }

    public void FullHeal()
    {
        health.UpdateHealth(health.GetMaxHealth());
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
            _rb.position = currCheckpoint.position;
            anchorRb.position = currCheckpoint.position;
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
