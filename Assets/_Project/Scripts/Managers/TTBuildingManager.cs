using Lean.Touch;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class TTBuildingManager
{
    [HideInInspector]
    public Action<Vector2, TTGridElement> OnDraggedItemSpawn;
    [HideInInspector]
    public Action<Vector2> OnDraggedItemMove;
    [HideInInspector]
    public Action OnDraggedItemRelease;
    
    private TTGridElement _draggedItem;
    private Camera _mainCamera;
    private TTCell _hoveredCell;
    bool _isDraggedItemAnchored = false;

    public void OnStart()
    {
        _mainCamera = Camera.main;
    }
    
    public void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    public void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }
    
    public void SpawnGridElement(EPoolItem poolItem)
    {
        _draggedItem = TTRunManager.Instance.pool.Get(poolItem, Vector3.zero).GetComponent<TTGridElement>();
        _draggedItem.Hide();
        OnDraggedItemSpawn?.Invoke(LeanTouch.Fingers.Last().ScreenPosition, _draggedItem);
        _isDraggedItemAnchored = false;

    }

    private void OnFingerDown(LeanFinger finger)
    {
        Vector3 worldPosition = finger.GetWorldPosition(10, _mainCamera);
        TTCell cell = TTGridManager.Instance.GetCellFromWorldPosition(worldPosition);
        if (cell && cell.gridElement != null)
        {
            _draggedItem = cell.gridElement;
            _draggedItem.Hide();
            OnDraggedItemSpawn?.Invoke(LeanTouch.Fingers.Last().ScreenPosition, _draggedItem);
            _isDraggedItemAnchored = true;
        }
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        if (_draggedItem != null && finger.ScreenDelta != Vector2.zero)
        {
            OnDraggedItemMove?.Invoke(finger.ScreenPosition);
            TTCell cell = TTGridManager.Instance.GetCellFromWorldPosition(finger.GetWorldPosition(10, _mainCamera));
            if (cell != _hoveredCell)
            {
                if(_hoveredCell)
                    _hoveredCell.Unhighlight();
                
                if (cell != null)
                {
                    cell.Highlight(cell.gridElement);
                }
                _hoveredCell = cell;
            }
        }
    }
    
    private void OnFingerUp(LeanFinger finger)
    {
        if (_draggedItem != null)
        {
            if (_hoveredCell)
            {
                _hoveredCell.Unhighlight();
                if (_hoveredCell.gridElement == null)
                {
                    if (_isDraggedItemAnchored)
                    {
                        _draggedItem.ClearCells();
                        _hoveredCell.AnchorElement(_draggedItem);
                    }
                    else
                    {
                        _hoveredCell.AnchorElement(_draggedItem);
                        TTRunManager.Instance.economyManager.DeductGold(TTRunManager.Instance.economyManager.currentPrice);
                        TTRunManager.Instance.economyManager.IncreasePrice();
                    }
                }
                else
                {
                    if(_isDraggedItemAnchored)
                    {
                        _draggedItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        _draggedItem.ClearCells();
                        TTRunManager.Instance.pool.Release(_draggedItem);
                    }
                }
                _hoveredCell = null;
            }
            else if(_isDraggedItemAnchored)
            {
                _draggedItem.gameObject.SetActive(true);
            }
            else
            {
                _draggedItem.ClearCells();
                TTRunManager.Instance.pool.Release(_draggedItem);
            }
            OnDraggedItemRelease?.Invoke();
            _draggedItem = null;
        }
    }
}
