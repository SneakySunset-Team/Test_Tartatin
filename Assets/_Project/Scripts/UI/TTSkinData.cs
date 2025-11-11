using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Skin", fileName = "Skin")]
public class TTSkinData : SerializedScriptableObject
{
    [field : SerializeField]
    public TTLocalizationTableEntryReference skinName { get; private set; }
    
    [field : SerializeField]
    public Sprite skinSprite { get; private set; }
}
