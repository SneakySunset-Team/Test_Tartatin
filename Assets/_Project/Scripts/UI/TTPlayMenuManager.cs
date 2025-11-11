using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TTPlayMenuManager : MonoBehaviour
{
    [SerializeField]
    TTButton _spawnMineButton, _spawnTurretButton;

    [SerializeField]
    Button _pauseBtn;

    [SerializeField]
    TextMeshProUGUI _minePriceTxt, _turretPriceTxt, _currentGoldTxt, _dollarsGainTxt;

    [SerializeField]
    Image _draggedItemImg;

    void Awake()
    {
        _pauseBtn.onClick.AddListener(()=>STTMenuManager.Instance.ChangeState(EMenuState.Pause));
    }

    void OnEnable()
    {
        STTGameManager.Instance.OnRunFinishedEvent += OnRunFinished;
        STTGameManager.Instance.OnRunStartedEvent += OnRunStarted;
    }

    void OnDisable()
    {
        STTGameManager.Instance.OnRunFinishedEvent -= OnRunFinished;
        STTGameManager.Instance.OnRunStartedEvent -= OnRunStarted;
    }

    void OnRunStarted()
    {
        STTRunManager runManager = STTRunManager.Instance;
        _spawnMineButton.OnPressEvent.AddListener(()=>STTRunManager.Instance.buildingManager.SpawnGridElement(EPoolItem.Mine));
        _spawnTurretButton.OnPressEvent.AddListener(()=>STTRunManager.Instance.buildingManager.SpawnGridElement(EPoolItem.Turret));
        runManager.runEconomyManager.OnGoldChangeEvent += OnGoldOrPriceChange;
        runManager.runEconomyManager.OnPriceChangeEvent += OnGoldOrPriceChange;
        runManager.buildingManager.OnDraggedItemSpawn += OnDraggedItemActivate;
        runManager.buildingManager.OnDraggedItemMove += OnDraggedItemMove;
        runManager.buildingManager.OnDraggedItemRelease += OnDraggedItemDeactivate;
        runManager.runEconomyManager.OnDollarsGainEvent += OnDollarsGainEvent;
    }

    void OnRunFinished()
    {
        STTRunManager runManager = STTRunManager.Instance;
        _spawnMineButton.OnPressEvent.RemoveAllListeners();
        _spawnTurretButton.OnPressEvent.RemoveAllListeners();
        runManager.runEconomyManager.OnGoldChangeEvent -= OnGoldOrPriceChange;
        runManager.runEconomyManager.OnPriceChangeEvent -= OnGoldOrPriceChange;
        runManager.buildingManager.OnDraggedItemSpawn -= OnDraggedItemActivate;
        runManager.buildingManager.OnDraggedItemMove -= OnDraggedItemMove;
        runManager.buildingManager.OnDraggedItemRelease -= OnDraggedItemDeactivate;
        runManager.runEconomyManager.OnDollarsGainEvent -= OnDollarsGainEvent;
    }

    private void OnGoldOrPriceChange()
    {
        STTRunManager runManager = STTRunManager.Instance;

        bool canBuyCondition = runManager.runEconomyManager.currentGold >= runManager.runEconomyManager.currentPrice;
        _spawnTurretButton.interactable = canBuyCondition;
        _spawnMineButton.interactable = canBuyCondition;
        _minePriceTxt.text = runManager.runEconomyManager.currentPrice.ToString();
        _turretPriceTxt.text = runManager.runEconomyManager.currentPrice.ToString();
        _currentGoldTxt.text = runManager.runEconomyManager.currentGold.ToString();
    }

    private void OnDollarsGainEvent()
    {
        _dollarsGainTxt.text = STTRunManager.Instance.runEconomyManager.runDollarsGain.ToString();
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
