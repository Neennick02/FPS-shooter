using UnityEngine;
using System.Collections;

public class Smoke : ThrowAble
{
    Coroutine addSmokeRoutine;
    bool isActive = false;
    protected override void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if(elapsedTime > 0.5f)
        {
            if (!isActive)
            {
                addSmokeRoutine = StartCoroutine(addSmoke());
                isActive = true;
            }

            if (elapsedTime > duration)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator addSmoke()
    {


        //smoke effect
        while(elapsedTime < duration)
        {
        explosionPrefab = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Maintain world space position
            explosionPrefab.transform.SetParent(transform,true);
            // Optional: reset rotation
            explosionPrefab.transform.rotation = Quaternion.identity; 
            Destroy(explosionPrefab, 5f);
            yield return new WaitForSeconds(1f);
        }
    }
}
