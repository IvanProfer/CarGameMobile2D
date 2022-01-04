using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Tool.Bundles.Examples
{
    internal class LoadWindowView : AssetBundleViewBase
    {
        [Header("Asset Bundles")]
        [SerializeField] private Button _loadAsssetsButton;
        [SerializeField] private Button _changeBackgroundButton;
        [SerializeField] private Button _backgroundButton;

        [Header("Addressables")]
        [SerializeField] private AssetReference _spawningButtonPrefab;
        [SerializeField] private RectTransform _spawnedButtonsContainer;
        [SerializeField] private Button _spawnAssetButton;

        private readonly List<AsyncOperationHandle<GameObject>> _addressablePrefabs =
            new List<AsyncOperationHandle<GameObject>>();


        private void Start()
        {
            _loadAsssetsButton.onClick.AddListener(LoadAsset);
            _spawnAssetButton.onClick.AddListener(SpawnPrefab);
            _changeBackgroundButton.onClick.AddListener(ChangeBackground);
            _backgroundButton.onClick.AddListener(BackgroundButton);
        }

        private void OnDestroy()
        {
            _loadAsssetsButton.onClick.RemoveAllListeners();
            _spawnAssetButton.onClick.RemoveAllListeners();
            _changeBackgroundButton.onClick.RemoveAllListeners();
            _backgroundButton.onClick.RemoveAllListeners();

            foreach (AsyncOperationHandle<GameObject> addressablePrefab in _addressablePrefabs)
                Addressables.ReleaseInstance(addressablePrefab);

            _addressablePrefabs.Clear();
        }

        private void LoadAsset()
        {
            _loadAsssetsButton.interactable = false;
            StartCoroutine(DownloadAndSetAssetBundles());
        }

        private void ChangeBackground()                                    
        {                                                                  
            _changeBackgroundButton.interactable = false;                 
            StartCoroutine(DownloadAndSetBackgroundAssetBundle());        
        }

        private void BackgroundButton()
        {
            _backgroundButton.interactable = false;
            StartCoroutine(DownloadAndSetButtonBackgroundAssetBundle());
        }

        private void SpawnPrefab()
        {
            AsyncOperationHandle<GameObject> addressablePrefab =
                Addressables.InstantiateAsync(_spawningButtonPrefab, _spawnedButtonsContainer);

            _addressablePrefabs.Add(addressablePrefab);
        }

    }
}
