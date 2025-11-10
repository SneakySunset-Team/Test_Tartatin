using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pooling Data")]
public class TTPoolingData : SerializedScriptableObject
{
    [field: SerializeField]
    public Dictionary<TTRunManager.EPoolItem, TTPoolingSystem<TTRunManager.EPoolItem, MonoBehaviour>.SPoolingElement> poolingElements { get; private set; }

    
    [OnInspectorInit]
    private void Init()
    {
        if (poolingElements == null)
        {
            poolingElements = new Dictionary<TTRunManager.EPoolItem, TTPoolingSystem<TTRunManager.EPoolItem, MonoBehaviour>.SPoolingElement>();
            foreach (TTRunManager.EPoolItem item in Enum.GetValues(typeof(TTRunManager.EPoolItem)))
            {
                poolingElements.Add(item, new TTPoolingSystem<TTRunManager.EPoolItem, MonoBehaviour>.SPoolingElement());
            }
        }
    }
}
