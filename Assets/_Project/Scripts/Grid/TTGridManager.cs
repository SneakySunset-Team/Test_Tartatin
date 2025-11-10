using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public partial class TTGridManager : TTSingleton<TTGridManager>
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
    float _size, spacing;
    
    #if UNITY_EDITOR
    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void GenerateGrid()
    {
        ClearGrid();

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                CreateCell(row, column);
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

    private void CreateCell(int row, int column)
    {
        TTCell cell = PrefabUtility.InstantiatePrefab(_cellPrefab, _gridRoot) as TTCell;
        if (!cell)
        {
            Debug.LogError($"Cell {row}, {column} was not created");
            return;
        }
        cell.coordinates = new Vector2Int(row, column);
        cell.transform.position = new Vector3(
            column * (_size + spacing) - (float)(columns - 1) / (float)2 * (_size + spacing), 
            row * (_size + spacing) - (float)(rows - 1) / (float)2 * (_size + spacing),
            0);
        
        cell.transform.localScale = Vector3.one * _size;
    }
    #endif
}