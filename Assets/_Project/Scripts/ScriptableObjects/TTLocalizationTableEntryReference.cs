using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

[System.Serializable, InlineProperty]
public class TTLocalizationTableEntryReference
{
    [ValueDropdown("GetTableCollections", DropdownTitle = "Select Table Collection")]
    [OnValueChanged("OnTableChanged")]
    [LabelText("Table")]
    [HorizontalGroup("Split", Width = 0.5f)]
    public string tableCollectionName;
    
    [ValueDropdown("GetEntries", DropdownTitle = "Select Entry")]
    [LabelText("Entry")]
    [HorizontalGroup("Split", Width = 0.5f)]
    public long keyId;
    
    public string TableCollectionName => tableCollectionName;
    public long KeyId => keyId;

    /// <summary>
    /// Gets a LocalizedString for use with Unity's Localization system
    /// </summary>
    public LocalizedString GetLocalizedString()
    {
        if (string.IsNullOrEmpty(tableCollectionName) || keyId == 0)
        {
            return new LocalizedString();
        }

        var localizedString = new LocalizedString();
        localizedString.TableReference = tableCollectionName;
        localizedString.TableEntryReference = keyId;
        return localizedString;
    }

    /// <summary>
    /// Gets the localized string value synchronously
    /// </summary>
    public string GetStringValue()
    {
        if (string.IsNullOrEmpty(tableCollectionName) || keyId == 0)
        {
            return string.Empty;
        }

        return UnityEngine.Localization.Settings.LocalizationSettings
            .StringDatabase
            .GetLocalizedString(tableCollectionName, keyId);
    }

    /// <summary>
    /// Gets the localized string asynchronously
    /// </summary>
    public UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<string> GetStringAsync()
    {
        return UnityEngine.Localization.Settings.LocalizationSettings
            .StringDatabase
            .GetLocalizedStringAsync(tableCollectionName, keyId);
    }

#if UNITY_EDITOR
    private IEnumerable<ValueDropdownItem<string>> GetTableCollections()
    {
        var collections = LocalizationEditorSettings.GetStringTableCollections();
        if (collections == null || collections.Count == 0)
            return new[] { new ValueDropdownItem<string>("No tables found", "") };
            
        return collections.Select(c => new ValueDropdownItem<string>(c.TableCollectionName, c.TableCollectionName));
    }

    private IEnumerable<ValueDropdownItem<long>> GetEntries()
    {
        if (string.IsNullOrEmpty(tableCollectionName))
        {
            yield return new ValueDropdownItem<long>("Select a table first...", 0);
            yield break;
        }

        var collection = LocalizationEditorSettings.GetStringTableCollection(tableCollectionName);
        if (collection == null || collection.SharedData == null)
        {
            yield return new ValueDropdownItem<long>("Table not found", 0);
            yield break;
        }

        yield return new ValueDropdownItem<long>("None", 0);
        
        foreach (var entry in collection.SharedData.Entries.OrderBy(e => e.Key))
        {
            yield return new ValueDropdownItem<long>($"{entry.Key}", entry.Id);
        }
    }

    private void OnTableChanged()
    {
        keyId = 0;
    }
#endif
}