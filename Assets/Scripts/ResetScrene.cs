using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetScrene : MonoBehaviour
{
    [SerializeField] PlayerUpgrades playerUpgrades;

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (collider.gameObject.CompareTag("Player"))
    //     {
    //         playerUpgrades.ResetUpgrades();
    //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     }
    // }
}
