using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
public class EnemyPath : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    //debug path draw
    public bool alwaysDrawPath;
    public bool drawNumbers;
    public Color debugColor = Color.white;
    public bool drawAsLoop;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }
#endif
    public void DrawPath()
    {
        for(int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColor;

            if (drawNumbers)
            {
#if UNITY_EDITOR
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);
#endif
            }

            if(i >= 1)
            {

#if UNITY_EDITOR
                //draw line between dots
                Gizmos.color = debugColor;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
#endif
            }
        }

        //make loop
        if (drawAsLoop)
        {

#if UNITY_EDITOR
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
#endif
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (alwaysDrawPath)
        {
            return;
        }
        else
        {
            DrawPath();
        }
    }
#endif
}
