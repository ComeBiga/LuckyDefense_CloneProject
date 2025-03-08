using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpriteRendererSetter : MonoBehaviour
{
    public Color color;
    public string sortingLayerName;
    public int orderInLayer;

    [ContextMenu("Set Color")]
    public void SetColor()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach(var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = color;
        }

        EditorUtility.SetDirty(this);
    }

    [ContextMenu("Set SortingLayer")]
    public void SetSortingLayer()
    {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
            spriteRenderer.sortingOrder = orderInLayer;
        }

        EditorUtility.SetDirty(this);
    }
}
