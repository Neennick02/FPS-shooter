using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] int poolSize = 8;
    [SerializeField] float duration = .2f;
    [SerializeField] GunScript gunScript;
    List<LineRenderer> linePool;
    int index = 0;

    private Dictionary<LineRenderer, Coroutine> activeCoroutines = new Dictionary<LineRenderer, Coroutine>();

    private void Awake()
    {
        linePool = new List<LineRenderer>();

        for(int i =0;i < poolSize; i++)
        {
            GameObject obj = Instantiate(lineRendererPrefab, transform);
            LineRenderer lr = obj.GetComponent<LineRenderer>();
            lr.enabled = false;
            linePool.Add(lr);
        }
    }
    public void DrawPelletTrail(Vector3 start, Vector3 end, Color startC, Color endC)
    {
        
        LineRenderer lr = linePool[index];
        index = (index + 1) % poolSize;

        // If this line is already fading, stop that fade immediately
        if (activeCoroutines.TryGetValue(lr, out Coroutine running))
        {
            StopCoroutine(running);
            activeCoroutines.Remove(lr);
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            //assign colors for start and end
            new GradientColorKey[]
            {
                new GradientColorKey(startC, 0.0f),
                new GradientColorKey(endC, 1.0f)
            },

            new GradientAlphaKey[]
            {
                 new GradientAlphaKey(startC.a, 0.0f),
                 new GradientAlphaKey(endC.a, 1.0f)
            });
        lr.colorGradient = gradient;

        //draw bullet trails
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        //start fade and store it
        Coroutine c = StartCoroutine(FadeLine(lr, duration));
        activeCoroutines[lr] = c;

    }


    IEnumerator FadeLine(LineRenderer lr, float time)
    {
        yield return new WaitForSeconds(Random.Range(time - (time/4), time + (time/4)));

        Gradient originalGradient = lr.colorGradient;
        Gradient gradientCopy = new Gradient();
        gradientCopy.SetKeys(originalGradient.colorKeys, originalGradient.alphaKeys);

        float timer = 0;

        while (timer < 2)
        {
            timer += Time.deltaTime;
            float alphaMultiplier = Mathf.Lerp(1f, 0f, timer / .5f);

            //create new alpha
            GradientAlphaKey[] originalAlphaKeys = gradientCopy.alphaKeys;
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[originalAlphaKeys.Length];


            for(int i=0; i < alphaKeys.Length; i++)
            {
                alphaKeys[i] = new GradientAlphaKey(originalAlphaKeys[i].alpha * alphaMultiplier, originalAlphaKeys[i].time);
            }

            //apply new gradient
            Gradient newGradient = new Gradient();
            newGradient.SetKeys(originalGradient.colorKeys, alphaKeys);
            lr.colorGradient = newGradient;



            yield return null;
        }
        lr.enabled = false;

        //Cleanup dictionary entry so it's reusable
        if (activeCoroutines.ContainsKey(lr))
            activeCoroutines.Remove(lr);
    }
}
