using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] TextMeshProUGUI ammouCounter;
    [SerializeField] Image reloadBar;
    void Start()
    {
        
    }

  
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void UpdateAmmoCounter(int ammoAmount, int magAmount)
    {
        ammouCounter.text = ammoAmount.ToString() + " / " + magAmount.ToString();
    }

    public void ReloadBar(float timer, float max)
    {
        reloadBar.fillAmount = timer / max;
    }
}
