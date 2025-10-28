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
        if(input.onFoot.Pause.triggered)
        {
            paused = !paused;
            Time.timeScale = paused? 0 : 1;

            if(paused) uiManager.OpenPauseMenu();
        }
    }

}
