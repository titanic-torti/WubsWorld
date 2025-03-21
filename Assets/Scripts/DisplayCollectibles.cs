using TMPro;
using UnityEngine;

public class DisplayCollectibles : MonoBehaviour
{
    [SerializeField] Collectibles collectibles;
    private TextMeshProUGUI text;

    void Start()
    {
        text = gameObject.transform.GetComponent<TextMeshProUGUI>();
    }

    // Another time I would probably add a listener rather than constantly running Update to check the status of collectibles
    void Update()
    {
        text.text = "Collectibles: " + collectibles.GetCollectedCollectibles().ToString() + "/" + collectibles.GetMaxCollectibleCount().ToString();
    }
}
