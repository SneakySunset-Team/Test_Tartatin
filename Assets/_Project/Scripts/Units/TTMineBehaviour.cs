using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TTMineBehaviour : MonoBehaviour
{
    [SerializeField]
    private TTUpgradeData _upgradeData;
    
    [SerializeField, HideInPlayMode]
    private ParticleSystem _miningParticle;
    
    [SerializeField]
    Texture2D[] _miningParticleTextures;

    [SerializeField]
    FMODUnity.EventReference _fmodMining;
    
    private Renderer _miningParticleRenderer;
    TTGridElement _gridElement;
    bool _isHidden;
    WaitUntil _waitUntilNotHidden;
    IEnumerator _miningRoutine;
    WaitForSeconds _miningWait = new WaitForSeconds(1f);
    
    void OnEnable()
    {
        _miningRoutine = MiningCoroutine();
        _waitUntilNotHidden = new WaitUntil(() => !_isHidden);
        StartCoroutine(_miningRoutine);
        
        _miningParticleRenderer = _miningParticle.GetComponent<Renderer>();
        _gridElement = GetComponent<TTGridElement>();
        _gridElement.OnHideEvent += OnHide;
        _gridElement.OnShowEvent += OnShow;
    }

    void OnDisable()
    {
        StopCoroutine(_miningRoutine);
        _gridElement.OnHideEvent -= OnHide;
        _gridElement.OnShowEvent -= OnShow;
    }

    IEnumerator MiningCoroutine()
    {
        while (true)
        {
            yield return _waitUntilNotHidden;
            yield return _miningWait;
            STTRunManager.Instance.runEconomyManager.AddGold(_upgradeData.GetStatValue(EUpgradeType.MineGoldPerTickAmount));
            SpawnParticle();
            FMODUnity.RuntimeManager.PlayOneShot(_fmodMining, transform.position);
        }
    }

    public void SpawnParticle()
    {
        int goldPerTick = _upgradeData.GetStatValue(EUpgradeType.MineGoldPerTickAmount);
        if (_miningParticleTextures.Length <= goldPerTick - 1)
        {
            Debug.LogError($"No texture for index {goldPerTick - 1}", this);
            return;
        }
        Texture2D bakedTexture = _miningParticleTextures[goldPerTick - 1];
        
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetTexture("_MainTex", bakedTexture);
    
        _miningParticleRenderer.SetPropertyBlock(mpb);
        _miningParticle.Play();
    }

    private void OnHide()
    {
        _isHidden = true;
    }

    private void OnShow()
    {
        _isHidden = false;
    }
}