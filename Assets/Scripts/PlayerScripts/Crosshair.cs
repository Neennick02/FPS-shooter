using UnityEngine;
using System.Collections;
public class Crosshair : MonoBehaviour
{
    [SerializeField] RectTransform top, bottom, left, right;
    [SerializeField] float thickness = 2;

    private void Start()
    {
        SetCrossHairSize(5);
    }
    public void SetCrossHairSize(float spacing)
    {
        //move lines outward
        top.anchoredPosition = new Vector2(0, spacing);
        bottom.anchoredPosition = new Vector2(0, -spacing);
        left.anchoredPosition = new Vector2(-spacing, 0);
        right.anchoredPosition = new Vector2(spacing, 0);

        // Keep thickness constant
        top.sizeDelta = new Vector2(thickness, top.sizeDelta.y);
        bottom.sizeDelta = new Vector2(thickness, bottom.sizeDelta.y);
        left.sizeDelta = new Vector2(left.sizeDelta.x, thickness);
        right.sizeDelta = new Vector2(right.sizeDelta.x, thickness);
    }
}
