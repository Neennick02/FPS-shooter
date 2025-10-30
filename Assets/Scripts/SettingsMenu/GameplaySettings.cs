using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameplaySettings : MonoBehaviour
{
    [SerializeField] private Settings _playerSettings;

    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _FovSlider;

    [SerializeField] float _sensitivityMultiplier = 100 ;
    [SerializeField] float _FovMultiplier = 100;

    [SerializeField] GameObject _keyboardControls, _controllerControls;
    private bool _controlScreenOpened = false;
    private float _value0;
    private float _value1;

    private void Start()
    {
        CloseControls();
        _sensitivitySlider.value = _playerSettings.Sensitivity / 100;
        _FovSlider.value =  _playerSettings.Fov/ _FovMultiplier;
    }

    private void Update()
    {
          _value0 = Mathf.Clamp(_sensitivitySlider.value * _sensitivityMultiplier, 0.1f, 100);
          _value1 = Mathf.Clamp(_FovSlider.value * _FovMultiplier, 0.1f, 100);


         //if slider value == fov value stop
        if (_playerSettings.Fov == _value1) return;

        
        //update fov values in scripts
        _playerSettings.Sensitivity = _value0;
        _playerSettings.Fov = _value1;
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
