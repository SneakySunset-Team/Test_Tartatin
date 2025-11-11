using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pooling Data")]
public class TTPoolingData : SerializedScriptableObject
{
    [field: SerializeField]
    public Dictionary<EPoolItem, TTPoolingSystem<EPoolItem, MonoBehaviour>.SPoolingElement> poolingElements { get; private set; }

    
    [OnInspectorInit]
    private void Init()
    {
        if (poolingElements == null)
        {
            poolingElements = new Dictionary<EPoolItem, TTPoolingSystem<EPoolItem, MonoBehaviour>.SPoolingElement>();
            foreach (EPoolItem item in Enum.GetValues(typeof(EPoolItem)))
            {
                poolingElements.Add(item, new TTPoolingSystem<EPoolItem, MonoBehaviour>.SPoolingElement());
            }
        }
    }
}
