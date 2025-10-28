using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class UI_Manager : MonoBehaviour
{
    [SerializeField] PauseScreenUi _pauseMenu;
    bool isPaused = false;

    /*    [Header("Setting_Menu")]
        [SerializeField] GameObject settings_menu_GameObject;
        [SerializeField] Button gameplay_Button;
        [SerializeField] Button audio_Button;
        [SerializeField] Button display_Button;


        [Header("Gameplay_Settings")]
        [SerializeField] Slider fov_slider;
        [SerializeField] Slider sensitivity_slider;
        [SerializeField] Button controls_button;
        [SerializeField] GameObject keyboard_Controls;
        [SerializeField] GameObject gamepad_Controls;
    */
    private void Start()
    {
        _pauseMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        Debug.Log(isPaused);

        if (InputManager.Instance.onFoot.Pause.triggered)
        {

                OpenAndClose_PauseScreen(!isPaused);
        }
    }
    public void OpenAndClose_PauseScreen(bool active)
    {
        _pauseMenu.gameObject.SetActive(active);
        ActivateMouse(active);
        isPaused = active;
        InputManager.Instance.BlockInput(active);
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
