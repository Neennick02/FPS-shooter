using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private Settings _playerSettings;
    [Header(" ")]
    [SerializeField] private Slider _frameRateSlider, _brightnesSlider;
    private int _frameRate = 60;
    private float _brightness;

    [SerializeField] private Toggle fullScreen;
    private bool isFullScreen;
    private bool vSyncEnabled;
    [Header(" ")]
    [SerializeField] private TextMeshProUGUI _rate, _brightnessProcent;

    [Header(" ")]
    [SerializeField] private TMP_Dropdown _qualityDropDown;

    [Header(" ")]
    [SerializeField] Volume _postProcessingVolume;
    ColorAdjustments ColorAdjustments;
    //some slider still need to be set up
    //values need to go into settings SO 

    private void Start()
    {
        //assign volume profile and set brightness
        VolumeProfile profile = _postProcessingVolume.profile;
        if(profile.TryGet<ColorAdjustments>(out ColorAdjustments))
        {
            ColorAdjustments.postExposure.value = _playerSettings.Brightness;
        }

        //set quality
        _qualityDropDown.value = (int)_playerSettings.QualityState;
        
        //set frameRate
        _frameRate = _playerSettings.FrameRate;
        _frameRateSlider.value = _frameRate;

        //set brightness slider value
        _brightness = _playerSettings.Brightness;
        _brightnesSlider.value = _brightness;

        
        vSyncEnabled = _playerSettings.Vsync;

        if (vSyncEnabled)
        {
            // Enable VSync — Unity locks to monitor refresh rate
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1; // -1 lets Unity pick automatically
            _playerSettings.FrameRate = -1;
            Debug.Log("VSync ON — Frame rate follows monitor refresh.");
        }
        else
        {
            // Disable VSync — use manual FPS cap
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _frameRate;
            _playerSettings.FrameRate = _frameRate;
            Debug.Log($"VSync OFF — Target FPS = {_frameRate}");
        }
    }
    private void Update()
    {
        //check if values have changed
        if (_frameRate == _frameRateSlider.value &&
            _brightness == _brightnesSlider.value) return;

        //set new value
        if(!vSyncEnabled) _frameRate = (int)_frameRateSlider.value;
        _brightness = _brightnesSlider.value;

        //set new framerate
        Application.targetFrameRate = _frameRate;          
        _playerSettings.FrameRate = _frameRate;
        _rate.text = _frameRate.ToString();

        //set new brightness
        _playerSettings.Brightness = _brightness;
        ColorAdjustments.postExposure.value = _playerSettings.Brightness;
        float brightnessProcent = (_brightness / 3) * 100;
        _brightnessProcent.text = brightnessProcent + " %";
    }   

            
                              /*FullScreenMode.ExclusiveFullScreen   // true fullscreen (takes over monitor)
                                FullScreenMode.FullScreenWindow      // borderless fullscreen
                                FullScreenMode.MaximizedWindow       // maximized window
                                FullScreenMode.Windowed              // normal windowed mode*/
    public void ToogleFullScreen()
    {
        Debug.Log("fullscreen = " + isFullScreen.ToString());
        isFullScreen = !isFullScreen;
        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;
        RefreshRate refresh = Screen.currentResolution.refreshRateRatio;

        Screen.SetResolution(width, height,
            fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed, refresh);
    }

    public void Vsync()
    {
        vSyncEnabled = !vSyncEnabled;
        if (vSyncEnabled)
        {
            // Enable VSync — Unity locks to monitor refresh rate
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1; // -1 lets Unity pick automatically
            _playerSettings.FrameRate = -1;
            Debug.Log("VSync ON — Frame rate follows monitor refresh.");

        }
        else
        {
            // Disable VSync — use manual FPS cap
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _frameRate;
            _playerSettings.FrameRate = _frameRate;
            Debug.Log($"VSync OFF — Target FPS = {_frameRate}");
        }
    }

    public void SetQuality(int index)
    {
        if(index == 0)
        {
            _playerSettings.QualityState = Settings.Quality.Low;
            QualitySettings.SetQualityLevel(index);
        }
        else if(index == 1)
        {
            _playerSettings.QualityState = Settings.Quality.Mid;
            QualitySettings.SetQualityLevel(index);
        }
        else
        {
            _playerSettings.QualityState = Settings.Quality.High;
            QualitySettings.SetQualityLevel(index);
        }
    }
}
