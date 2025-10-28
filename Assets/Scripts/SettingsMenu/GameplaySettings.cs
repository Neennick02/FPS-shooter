using UnityEngine;
using UnityEngine.UI;
public class GameplaySettings : MonoBehaviour
{
    InputManager input;

    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _FovSlider;

    [SerializeField] float _sensitivityMultiplier = 100 ;
    [SerializeField] float _FovMultiplier = 100;
    private float _value0;
    private float _value1;

    private void Start()
    {
        if (InputManager.Instance == null) return;

        input = InputManager.Instance;

        PlayerLook look = input.GetComponent<PlayerLook>();
        _sensitivitySlider.value = look.ReturnSensitivity()/ _sensitivityMultiplier;
        if(look.ReturnFov() != null) _FovSlider.value = look.ReturnFov() / _FovMultiplier;
    }

    private void Update()
    {

        PlayerLook lookScript = input.gameObject.GetComponent<PlayerLook>();

          _value0 = Mathf.Clamp(_sensitivitySlider.value * _sensitivityMultiplier, 0.1f, 100);
          _value1 = Mathf.Clamp(_FovSlider.value * _FovMultiplier, 0.1f, 100);

        lookScript.UpdateLookSensitivity(_value0);
        lookScript.UpdateFOV(_value1);
    }
}
