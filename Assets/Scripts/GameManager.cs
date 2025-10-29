using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] GameObject _player;
    CharacterController _characterController;

    UI_Manager uiManager;
    InputManager _input;
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
        _input = InputManager.Instance;
    }
}
