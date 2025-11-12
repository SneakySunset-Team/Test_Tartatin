using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TTUpgradePageManager : MonoBehaviour
{
    [SerializeField]
    private TTUpgradeData _upgradeData;
    
    [SerializeField]
    Button _cheatResetUpgradesBtn, _closeUpgradePanelBtn;
    
    [SerializeField]
    TTUpgradeItem _upgradeItemPrefab;
    
    [SerializeField]
    RectTransform _upgradeFolder;

    [SerializeField]
    int _maxNumberOfUpgrades;

    TTUpgradeItem[] _upgrades;
    
    void Awake()
    {
        _upgrades = new TTUpgradeItem[_maxNumberOfUpgrades];
        for (int i = 0; i < _maxNumberOfUpgrades; i++)
        {
            _upgrades[i] = Instantiate(_upgradeItemPrefab, _upgradeFolder);
            _upgrades[i].gameObject.SetActive(false);
        }
        _closeUpgradePanelBtn.onClick.AddListener(()=> gameObject.SetActive(false));
        _cheatResetUpgradesBtn.onClick.AddListener(ResetUpgrades);

        InitializeUpgradePanel();
    }
    
    void OnEnable()
    {
        STTGameManager.Instance.OnDollarsChangeEvent += OnDollarChange;
        // Subscription happens after the callback on state change
        OnDollarChange();
    }

    void OnDisable()
    {
        STTGameManager.Instance.OnDollarsChangeEvent -= OnDollarChange;
    }
    
    private void OnDollarChange()
    {
        int i = 0;
        foreach (var upgradeData in _upgradeData.upgradeData)
        {
            if (i >= _maxNumberOfUpgrades)
            {
                Debug.LogWarning($"Not enough upgrade Items were instantiated ({_maxNumberOfUpgrades} <= {i})");
                i++;
                continue;
            }
            _upgrades[i].UpdateItem();
            i++;
        }
    }
    
    private void InitializeUpgradePanel()
    {
        int i = 0;
        foreach (var upgradeData in _upgradeData.upgradeData)
        {
            if (i >= _maxNumberOfUpgrades)
            {
                Debug.LogWarning($"Not enough upgrade Items were instantiated ({_maxNumberOfUpgrades} <= {i})");
                i++;
                continue;
            }
            _upgrades[i].Initialize(upgradeData.Key);
            _upgrades[i].UpdateItem();
            i++;
        }
    }
    
    private void ResetUpgrades()
    {
        int i = 0;
        foreach (var upgradeData in _upgradeData.upgradeData)
        {
            if (i >= _maxNumberOfUpgrades)
            {
                Debug.LogWarning($"Not enough upgrade Items were instantiated ({_maxNumberOfUpgrades} <= {i})");
                i++;
                continue;
            }
            _upgradeData.ResetUpgrade(upgradeData.Key);
            _upgrades[i].UpdateItem();
            i++;
        }
    }
}
