using UnityEngine;
using System.Collections;

public class Smoke : ThrowAble
{
    Coroutine addSmokeRoutine;

    protected override void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if(elapsedTime > 0.5f)
        {
            addSmokeRoutine = StartCoroutine(addSmoke());

            if (elapsedTime > duration)
            {
                Destroy(gameObject);
            }
        }
    }
    protected override void Explode()
    {
        //smoke effect
        explosionPrefab = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    IEnumerator addSmoke()
    {


        //smoke effect
        while(elapsedTime < duration)
        {
            for(int i =0; i < 3; i++)
            {
                explosionPrefab = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(1f);
    }
}
