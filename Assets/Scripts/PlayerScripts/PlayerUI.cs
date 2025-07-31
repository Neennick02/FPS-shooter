using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] TextMeshProUGUI ammouCounter;
    [SerializeField] Image reloadBar;
    [SerializeField] Image hitMarker;
    Coroutine currentCoroutine;
    void Start()
    {
        hitMarker.enabled = false;
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

    public void ShowHitMarker(float waitTime)
    {
        hitMarker.enabled = true;

        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(DisableHitMarker(waitTime));
    }

    IEnumerator DisableHitMarker(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hitMarker.enabled = false;
    }


}
