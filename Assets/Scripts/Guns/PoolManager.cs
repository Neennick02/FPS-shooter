using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] int poolSize = 8;
    [SerializeField] float duration = 1;
    List<LineRenderer> linePool;
    int index = 0;
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

        //draw bullet trails
        lr.enabled = true;
        lr.startColor = startC;
        lr.endColor = endC;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        StartCoroutine(DisableLineAfterTimer(lr, duration));
    }


    IEnumerator DisableLineAfterTimer(LineRenderer lr, float time)
    {
        yield return new WaitForSeconds(Random.Range(time - (time/2), time + (time/2)));
        lr.enabled = false;
    }
}
