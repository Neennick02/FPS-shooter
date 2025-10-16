using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject pause_menu_GameObject;


    [Header("Setting_Menu")]
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

    // [Header("Audio_Settings")]

    //  [Header("Display_Settings")]
    public void QuitGame()
    {
        Debug.Log("Quit app");
        Application.Quit();
    }

    public void OpenPauseMenu()
    {
        if(pause_menu_GameObject != null) pause_menu_GameObject.SetActive(!pause_menu_GameObject.activeInHierarchy);
    }
    public void OpenSettingsMenu()
    {
        if(settings_menu_GameObject != null) settings_menu_GameObject.SetActive(!settings_menu_GameObject.activeInHierarchy);
        if(pause_menu_GameObject.activeInHierarchy) pause_menu_GameObject.SetActive(false);
    }

    public void OpenControls()
    {
        if (Gamepad.current == null) keyboard_Controls.SetActive(!keyboard_Controls.activeInHierarchy);
        else gamepad_Controls.SetActive(!gamepad_Controls.activeInHierarchy);
    }


}
