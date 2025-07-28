using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class EnemyPath : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    //debug path draw
    public bool alwaysDrawPath;
    public bool drawNumbers;
    public Color debugColor = Color.white;
    public bool drawAsLoop;

    private void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }
    public void DrawPath()
    {
        for(int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColor;

            if (drawNumbers)
            {
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);
            }

            if(i >= 1)
            {
                //draw line between dots
                Gizmos.color = debugColor;
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }
        }

        //make loop
        if (drawAsLoop)
        {
            Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
        }
    }

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
}
