using UnityEngine;

public class PauseScreenUi : MonoBehaviour
{
    [SerializeField] Settings _settings;
    [SerializeField] private GameObject _settingsMenu;
    private void Update()
    {
        if (InputManager.Instance.onFoot.Pause.triggered)
        {
            if (this.gameObject.activeInHierarchy) ControlPauseScreen(false);
            else ControlPauseScreen(true);
        }
    }

    public void ControlPauseScreen(bool active)
    {
        this.gameObject.SetActive(active);
        _settings.Paused = active;
    }


    public void Settings(bool active)
    {
        _settingsMenu.SetActive(active);    
    }

    public void QuitGame()
    {
        Debug.Log("Quit app");
        Application.Quit();
    }
}
