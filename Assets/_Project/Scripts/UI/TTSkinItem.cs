using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TTSkinItem : MonoBehaviour
{
    TTSkinData _skinData;
    [SerializeField]
    Image _iconImg;

    [SerializeField]
    LocalizeStringEvent _nameLocalizeString;
    Toggle _toggle;
    
    public void Initialize(TTSkinData skinData, ToggleGroup toggleGroup)
    {
        _toggle = GetComponent<Toggle>();
        _toggle.group = toggleGroup;
        _skinData = skinData;
        _iconImg.sprite = _skinData.skinSprite;
        _toggle.onValueChanged.AddListener((bool b) => STTGameManager.Instance.SetSkinSprite(_skinData.skinSprite, skinData.skinName.keyId));
        
        var localizedString = _skinData.skinName.GetLocalizedString();
        _nameLocalizeString.StringReference = localizedString;
        _nameLocalizeString.RefreshString();
    }
}
