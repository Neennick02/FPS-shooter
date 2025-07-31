using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class Sniper : GunScript
{
    [SerializeField] Volume volume;
    Vignette vignette;
    LensDistortion distorition;

    [SerializeField] Image scopeImage;
    [SerializeField] GameObject crossHair;
    [SerializeField] float fadeDuration;
    float fadeTimer ;
    Coroutine currentFadeCoroutine;
    bool showEffect = false;
    protected override void Start()
    {
        
        base.Start();
        zoomFOV = normalFOV / 4;
        VolumeProfile profile = volume.profile;

        //find post processing effects
        profile.TryGet<Vignette>(out vignette);
        profile.TryGet<LensDistortion>(out distorition);
        //disable effects
        DisablePostEffects();

        //disable scope image
        SetScopeAlpha(0);
        ShowGun();
    }

    protected override void Aim()
    {
        base.Aim();

        bool closeToADSpos = Vector3.Distance(transform.localPosition, ADSPos) < .06f;
        crossHair.SetActive(!closeToADSpos);

        if (closeToADSpos && !showEffect)
        {
            showEffect = true;
            EnablePostEffects(.6f, .5f);

            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            currentFadeCoroutine = StartCoroutine(FadeInImage());
            HideGun();
        }
        else if(!closeToADSpos && showEffect)
         {
                showEffect = false;
                DisablePostEffects();

                if (currentFadeCoroutine != null)
                {
                    StopCoroutine(currentFadeCoroutine);
                }
                currentFadeCoroutine = StartCoroutine(FadeOutImage());
                ShowGun();
        }
    }

    public IEnumerator FadeInImage()
    {
        Color imgColor = scopeImage.color;
        SetScopeAlpha(0);

        fadeTimer = 0f;

        while (fadeTimer < fadeDuration)
        {
            //fade image in
            fadeTimer += Time.deltaTime;
            imgColor.a = Mathf.Lerp(0, 1, fadeTimer / fadeDuration);
            scopeImage.color = imgColor;
            yield return null;
        }
        //make sure alpha is full at the end
        SetScopeAlpha(1);
    }

    public IEnumerator FadeOutImage()
    {
        Color imgColor = scopeImage.color;
        SetScopeAlpha(1);

        fadeTimer = 0f;

        while (fadeTimer < fadeDuration)
        {
            //fade image in
            fadeTimer += Time.deltaTime;
            imgColor.a = Mathf.Lerp(1, 0, fadeTimer / fadeDuration);
            scopeImage.color = imgColor;
            yield return null;
        }
        //make sure alpha is full at the end
        SetScopeAlpha(0);
    }

    void HideGun()
    {
        MeshRenderer mainMesh = GetComponent<MeshRenderer>();
        mainMesh.enabled = false;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;  // Disable rendering
        }
    }

    // To show the gun again
    void ShowGun()
    {
        MeshRenderer mainMesh = GetComponent<MeshRenderer>();
        mainMesh.enabled = true;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = true;  // Enable rendering
        }
    }

    void SetScopeAlpha(float alpha)
    {
        Color imgColor = scopeImage.color;
        imgColor.a = alpha;
        scopeImage.color = imgColor;
    }

    void EnablePostEffects(float distortionAmount, float vignetteAmount)
    {
        //makes it possible to override
        distorition.intensity.overrideState = true;
        vignette.intensity.overrideState = true;

        //override intensity values
        distorition.intensity.value = distortionAmount;
        vignette.intensity.value = vignetteAmount;
    }

    void DisablePostEffects()
    {

        //disable both effects
        distorition.intensity.value = 0;
        vignette.intensity.value = 0;
    }
}
