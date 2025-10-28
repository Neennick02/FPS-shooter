using UnityEngine;

public class SettingsUiManager : MonoBehaviour
{

    [SerializeField] GameObject[] _tabs;

    private void Start()
    {
        //always start on first tab
        for (int i = 0; i < _tabs.Length; i++)
        {
            _tabs[i].SetActive(i == 0 ? true : false);
        }
    }

    public void Return()
    {
        this.gameObject.SetActive(false);
    }

    public void CloseAllTabs()
    {
        for (int i = 0; i < _tabs.Length; i++)
        {
            _tabs[i].SetActive(false);
        }
    }

    public void OpenGameplayTab()
    {
        CloseAllTabs();
        _tabs[0].SetActive(true);
    }

    public void OpenAudioTab()
    {
        CloseAllTabs();
        _tabs[1].SetActive(true);
    }
    public void OpenDisplayTab()
    {
        CloseAllTabs();
        _tabs[2].SetActive(true);
    }
}
