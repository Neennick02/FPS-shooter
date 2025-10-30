using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameplaySettings : MonoBehaviour
{

    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _FovSlider;

    [SerializeField] float _sensitivityMultiplier = 100 ;
    [SerializeField] float _FovMultiplier = 100;
    private float _value0;
    private float _value1;

    InputManager _input;
    GunScript _gunScript;
    PlayerLook _lookScript;
    WeaponManager _weaponManager;
    private List<GunScript> _gunArray = new List<GunScript>();



    private void Start()
    {
        if (InputManager.Instance == null) return;

        _input = InputManager.Instance;

        _gunScript = _input.GetComponentInChildren<GunScript>();
        _lookScript = _input.GetComponent<PlayerLook>();
        _weaponManager = _input.GetComponent<WeaponManager>();
        _sensitivitySlider.value = _lookScript.ReturnSensitivity()/ _sensitivityMultiplier;
        _FovSlider.value = _lookScript.ReturnFov() / _FovMultiplier;
    }

    private void Update()
    {
          _value0 = Mathf.Clamp(_sensitivitySlider.value * _sensitivityMultiplier, 0.1f, 100);
          _value1 = Mathf.Clamp(_FovSlider.value * _FovMultiplier, 0.1f, 100);

        _lookScript.UpdateLookSensitivity(_value0);

        //if slider value == fov value stop
        if (_lookScript.ReturnFov() == _value1) return;


        //update fov values in scripts

        _gunArray = _weaponManager.ReturnAllGuns();
        for (int i = 0; i < _gunArray.Count; i++)
        {
            _gunArray[i].SetFov(_value1);
        }
        _gunScript.SetFov(_value1);
        _lookScript.setFov(_value1);
    }
}
