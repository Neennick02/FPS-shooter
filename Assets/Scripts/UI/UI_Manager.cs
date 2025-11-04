using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class UI_Manager : MonoBehaviour
{
    [SerializeField] PauseScreenUi _pauseMenu;
    [SerializeField] Settings _settings;

    private void Start()
    {
        _pauseMenu.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (InputManager.Instance.onFoot.Pause.triggered)
        {

                OpenAndClose_PauseScreen(!_settings.Paused);
        }
    }
    public void OpenAndClose_PauseScreen(bool active)
    {
        _pauseMenu.gameObject.SetActive(active);
        _pauseMenu.Settings(false);
        ActivateMouse(active);
        _settings.Paused = active;
        InputManager.Instance.BlockInput(active);

        if (active) Time.timeScale = 0;
        else Time.timeScale = 1;

    }
    private void ActivateMouse(bool active)
    {
        //makes cursor invisible during gameplay
        Cursor.visible = active;
        if (!active)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
