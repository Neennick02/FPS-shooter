using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DisplaySettings : MonoBehaviour
{
    [SerializeField] private Settings _playerSettings;

    [SerializeField] private Slider _frameRateSlider, _brightnesSlider;
    private int _frameRate = 60, _brightness;

    [SerializeField] private Toggle fullScreen;
    private bool isFullScreen;
    [SerializeField] private TextMeshProUGUI _rate, _brightnessProcent;



    //some slider still need to be set up
    //values need to go into settings SO 

    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = _frameRate;
        _frameRateSlider.value = _frameRate;
    }
    private void Update()
    {
        if (_frameRate == _frameRateSlider.value &&
            _brightness == _brightnesSlider.value) return;

        _frameRate = (int)_frameRateSlider.value;
        _brightness = (int)_brightnesSlider.value;


        _rate.text = _frameRate.ToString();
        _brightnessProcent.text = _brightness.ToString() + " %";
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

    public void Vsync(bool isTrue)
    {
        if (isTrue)
        {
            QualitySettings.vSyncCount = 1;  //default
        }
        else
        {
            QualitySettings.vSyncCount = 0;  // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
            Application.targetFrameRate = _frameRate;
        }
    }

    public void SetQuality(int index)
    {
      /*  if(index == 0)
        {
            QualityLevel.Fastest;
        }
        else if(index == 1)
        {
            QualityLevel.Fast;
        }
        else
        {
            QualityLevel.Simple;
        }*/
    }
}
