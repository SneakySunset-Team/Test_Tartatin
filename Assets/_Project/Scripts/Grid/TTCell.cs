using Sirenix.OdinInspector;
using System;
using UnityEngine;

[System.Serializable]
public class TTCell : MonoBehaviour
{
    [SerializeField, ReadOnly]
    public Vector2Int coordinates;

    [field : SerializeField, ReadOnly, HideInEditorMode]
    public TTGridElement gridElement { get; private set; }

    public void AnchorElement(TTGridElement element)
    {
        gridElement = element;
        gridElement.isAnchored = true;
        gridElement.cells.Add(this);
    }

    public void ClearGridElement()
    {
        gridElement = null;
    }
}