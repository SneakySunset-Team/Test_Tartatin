using Lean.Touch;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TTGridManager : TTSingleton<TTGridManager>
{
    [field : SerializeField, ReadOnly, BoxGroup("Grid")]
    public TTCell[] grid { get; private set; }
    
    [field : SerializeField, BoxGroup("Grid")]
    public int rows { get; private set; }
    
    [field : SerializeField, BoxGroup("Grid")]
    public int columns { get; private set; }

    [SerializeField, FoldoutGroup("Components")]
    Transform _gridRoot;

    [SerializeField, FoldoutGroup("Components")]
    TTCell _cellPrefab;

    [SerializeField, FoldoutGroup("Placement")]
    float _size, _spacing;

    public TTCell GetCellFromWorldPosition(Vector2 worldPosition)
    {
        float cellSize = _size + _spacing;
    
        Vector2 localPosition = worldPosition - (Vector2)_gridRoot.position;
    
        int column = Mathf.RoundToInt(
            (localPosition.x + (float)(columns - 1) / 2f * cellSize) / cellSize);
        int row = Mathf.RoundToInt(
            (localPosition.y + (float)(rows - 1) / 2f * cellSize) / cellSize);
    
        if (row < 0 || row >= rows || column < 0 || column >= columns) return null;
        return grid[row * columns + column];
    }
    
    #if UNITY_EDITOR
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void GenerateGrid()
    {
        ClearGrid();

        grid = new TTCell[rows * columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                grid [row * columns + column] = CreateCell(row, column);
            }
        }
    }
    
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void ClearGrid()
    {
        for (int i = _gridRoot.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(_gridRoot.GetChild(i).gameObject);
        }
        grid = null;
    }

    private TTCell CreateCell(int row, int column)
    {
        TTCell cell = PrefabUtility.InstantiatePrefab(_cellPrefab, _gridRoot) as TTCell;
        if (!cell)
        {
            Debug.LogError($"Cell {row}, {column} was not created");
            return null;
        }
        cell.coordinates = new Vector2Int(row, column);
        cell.transform.localPosition = new Vector3(
            column * (_size + _spacing) - (float)(columns - 1) / (float)2 * (_size + _spacing), 
            row * (_size + _spacing) - (float)(rows - 1) / (float)2 * (_size + _spacing),
            0);
        
        cell.transform.localScale = Vector3.one * _size;
        cell.name = $"Cell {row}, {column}";
        return cell;
    }
    #endif
}