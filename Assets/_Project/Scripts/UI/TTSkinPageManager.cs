using Sirenix.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TTSkinPageManager : MonoBehaviour
{
    [SerializeField]
    TTSkinData[] _availableSkins;

    [SerializeField]
    TTSkinItem _skinItemPrefab;

    [SerializeField]
    RectTransform _skinItemsFolder;

    [SerializeField]
    Button _closePanelBtn;
    
    TTSkinItem[] _skinItems;
    
    void Awake()
    {
        _closePanelBtn.onClick.AddListener(() => gameObject.SetActive(false));
        _skinItems = new TTSkinItem[_availableSkins.Length];
        ToggleGroup toggleGroup = _skinItemsFolder.GetComponent<ToggleGroup>();
        for (int i = 0; i < _availableSkins.Length; i++)
        {
            _skinItems[i] = Instantiate(_skinItemPrefab, _skinItemsFolder);
            _skinItems[i].Initialize(_availableSkins[i], toggleGroup);
        }
    }

    public Sprite GetSkin(long keyId)
    {
        if (keyId == 0)
        {
            return _availableSkins[0].skinSprite;
        }
        
        foreach (TTSkinData skinData in _availableSkins)
        {
            if (skinData.skinName.keyId == keyId)
            {
                return skinData.skinSprite;
            }
        }
        Debug.LogError($"Skin {keyId} not found");
        return null;
    }
}