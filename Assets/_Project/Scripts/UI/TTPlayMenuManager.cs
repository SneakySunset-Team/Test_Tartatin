using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TTPlayMenuManager : MonoBehaviour
{
    [SerializeField]
    TTButton _spawnMineButton, _spawnTurretButton;

    [SerializeField]
    TextMeshProUGUI _minePriceTxt, _turretPriceTxt, _currentGoldTxt;

    [SerializeField]
    Image _draggedItemImg;
    
    void OnEnable()
    {
        TTGameManager.Instance.OnRunStartedEvent += OnRunStarted;
        TTGameManager.Instance.OnRunFinishedEvent += OnRunFinished;
    }

    void OnDisable()
    {
        TTGameManager.Instance.OnRunStartedEvent -= OnRunStarted;
        TTGameManager.Instance.OnRunFinishedEvent -= OnRunFinished;
    }

    void OnRunStarted()
    {
        TTRunManager runManager = TTRunManager.Instance;
        _spawnMineButton.OnPressEvent.AddListener(()=>TTRunManager.Instance.buildingManager.SpawnGridElement(EPoolItem.Mine));
        _spawnTurretButton.OnPressEvent.AddListener(()=>TTRunManager.Instance.buildingManager.SpawnGridElement(EPoolItem.Turret));
        runManager.economyManager.OnGoldChangeEvent += OnGoldOrPriceChange;
        runManager.economyManager.OnPriceChangeEvent += OnGoldOrPriceChange;
        runManager.buildingManager.OnDraggedItemSpawn += OnDraggedItemActivate;
        runManager.buildingManager.OnDraggedItemMove += OnDraggedItemMove;
        runManager.buildingManager.OnDraggedItemRelease += OnDraggedItemDeactivate;
    }

    void OnRunFinished()
    {
        TTRunManager runManager = TTRunManager.Instance;

        _spawnMineButton.OnPressEvent.RemoveAllListeners();
        _spawnTurretButton.OnPressEvent.RemoveAllListeners();
        runManager.economyManager.OnGoldChangeEvent -= OnGoldOrPriceChange;
        runManager.economyManager.OnPriceChangeEvent -= OnGoldOrPriceChange;
        runManager.buildingManager.OnDraggedItemSpawn -= OnDraggedItemActivate;
        runManager.buildingManager.OnDraggedItemMove -= OnDraggedItemMove;
        runManager.buildingManager.OnDraggedItemRelease -= OnDraggedItemDeactivate;
    }

    private void OnGoldOrPriceChange()
    {
        TTRunManager runManager = TTRunManager.Instance;

        bool canBuyCondition = runManager.economyManager.currentGold >= runManager.economyManager.currentPrice;
        _spawnTurretButton.interactable = canBuyCondition;
        _spawnMineButton.interactable = canBuyCondition;
        _minePriceTxt.text = runManager.economyManager.currentPrice.ToString();
        _turretPriceTxt.text = runManager.economyManager.currentPrice.ToString();
        _currentGoldTxt.text = runManager.economyManager.currentGold.ToString();

    }

    private void OnDraggedItemActivate(Vector2 position, TTGridElement element)
    {
        _draggedItemImg.gameObject.SetActive(true);
        _draggedItemImg.sprite = element.GetSprite();
        _draggedItemImg.color = element.GetColor();
        _draggedItemImg.rectTransform.anchoredPosition = position;
    }

    private void OnDraggedItemMove(Vector2 newPosition)
    {
        _draggedItemImg.rectTransform.anchoredPosition = newPosition;
    }

    private void OnDraggedItemDeactivate()
    {
        _draggedItemImg.gameObject.SetActive(false);
    }
}
