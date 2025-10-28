using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] GameObject _player;
    CharacterController _characterController;

    InputManager input;
    UI_Manager uiManager;
    bool paused = false;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) Debug.Log("GameManager not found");

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        input = FindFirstObjectByType<InputManager>();
        uiManager = GetComponent<UI_Manager>();
    }

    private void Update()
    {
        if (input.onFoot.Pause.triggered)
        {
            paused = !paused;
            input.BlockInput(paused);
            
            PauseMenu(paused);
        }
    }

    public void PauseMenu(bool paused)
    {
        uiManager.OpenPauseMenu();
        ActivateMouse(true);
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
