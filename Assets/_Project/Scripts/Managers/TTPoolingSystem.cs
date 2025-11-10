using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor.Rendering;
using UnityEngine;



[System.Serializable]
public class TTPoolingSystem<J, T> where T : MonoBehaviour
{
    public struct SPoolingElement
    {
        public T prefab;
        public int baseNumber;
    }
    
    private Dictionary<J, List<T>> _PoolDictionary = new Dictionary<J, List<T>>();
    private Transform _inactivePoolFolder;
    private Transform _activePoolFolder;
    private Dictionary<J, T> _poolPrefabs;

    public TTPoolingSystem(
        [Tooltip("J = enum of element | T = type of element | int = Base number of elements in pool ")]
        Dictionary<J, SPoolingElement> poolPrefabs, 
        Transform inactivePoolFolder, Transform activePoolFolder)
    {
        _inactivePoolFolder = inactivePoolFolder;
        _activePoolFolder = activePoolFolder;
        _poolPrefabs = new Dictionary<J, T>();
        foreach (var element in poolPrefabs) _poolPrefabs.Add(element.Key, element.Value.prefab);
        foreach(var pair in poolPrefabs)
        {
            _PoolDictionary.Add(pair.Key, new List<T>());
            for (int i = 0; i < pair.Value.baseNumber; i++)
            {
                _PoolDictionary[pair.Key].Add(UnityEngine.Object.Instantiate(poolPrefabs[pair.Key].prefab, _inactivePoolFolder));
                _PoolDictionary[pair.Key][i].gameObject.SetActive(false);
            }
        }
    }

    public T Get(J type, Vector3 position)
    {
        foreach(var item in _PoolDictionary[type])
        {
            if (!item.gameObject.activeInHierarchy)
            {
                item.transform.position = position;
                item.gameObject.SetActive(true);
                item.transform.parent = _activePoolFolder.transform;
                return item;
            }
        }
        _PoolDictionary[type].Add(UnityEngine.Object.Instantiate(_poolPrefabs[type],position, Quaternion.identity, _activePoolFolder));
        return _PoolDictionary[type].Last();
    }

    public void Release(T item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = _inactivePoolFolder.transform;
    }

    
}
