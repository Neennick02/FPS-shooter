using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameplaySettings : MonoBehaviour
{
    [SerializeField] private Settings _playerSettings;

    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _FovSlider;

    [SerializeField] GameObject _keyboardControls, _controllerControls;
    private bool _controlScreenOpened = false;
    private float _sensitivityValue;
    private float _fovValue;

    private void Start()
    {
        CloseControls();
        _sensitivitySlider.value = _playerSettings.Sensitivity;
        _FovSlider.value =  _playerSettings.Fov;
    }

    private void Update()
    {
        _sensitivityValue = _sensitivitySlider.value;
        _fovValue = _FovSlider.value;

        //if slider value == scriptable object value -> stop
        if (_playerSettings.Fov == _fovValue && _playerSettings.Sensitivity == _sensitivityValue)
            return;

        //update fov values in scripts
        _playerSettings.Sensitivity = _sensitivityValue;
        _playerSettings.Fov = _fovValue;
    }

    public void OpenControls()
    {
        if (_controlScreenOpened)
        {
            CloseControls();
            return;
        }
        if(Gamepad.current == null)
        {
            _keyboardControls.SetActive(true);
            _controllerControls.SetActive(false);
            _controlScreenOpened = true;
        }
        else
        {
            _keyboardControls.SetActive(false);
            _controllerControls.SetActive(true); 
            _controlScreenOpened = true;
        }
    }

    public void CloseControls()
    {
        _keyboardControls.SetActive(false);
        _controllerControls.SetActive(false);
        _controlScreenOpened = false;
    }
}
