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

    [SerializeField, BoxGroup("Highlighting")]
    SpriteRenderer _spriteRenderer;
    
    [SerializeField, BoxGroup("Highlighting")]
    Color _highlightColor;
    
    [SerializeField, BoxGroup("Highlighting")]
    Color _highlightNegationColor;

    Color _originalColor;

    void Start()
    {
        _originalColor = _spriteRenderer.color;
    }

    public void AnchorElement(TTGridElement element)
    {
        gridElement = element;
        gridElement.cells.Add(this);
        gridElement.transform.position =  transform.position;
        gridElement.Show();
    }

    public void ClearGridElement()
    {
        gridElement = null;
    }

    public void Highlight(bool isNegation)
    {
        if(isNegation)
            _spriteRenderer.color = _highlightNegationColor;
        else 
            _spriteRenderer.color = _highlightColor;
    }

    public void Unhighlight()
    {
        _spriteRenderer.color = _originalColor;
    }
}