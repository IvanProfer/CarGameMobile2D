using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Tool.Bundles.Examples
{
    internal class AssetBundleViewBase : MonoBehaviour
    {
        private const string UrlAssetBundleBackgrounds = "https://drive.google.com/uc?export=download&id=1K_NlFXDyVL17pvIOALOh3DeLnqmy0g5J";
        private const string UrlAssetBundleSprites = "https://drive.google.com/uc?export=download&id=1345im6twC2xs1PuM6z7QfWxP-HxODuYN";
        private const string UrlAssetBundleAudio = "https://drive.google.com/uc?export=download&id=1V0RqwpG9t2m2dp8x-tOIhT7Kw1h2FPR6";

        [SerializeField] private DataSpriteBundle[] _dataSpriteBundles;
        [SerializeField] private DataAudioBundle[] _dataAudioBundles;
        [SerializeField] private DataSpriteBundle[] _dataBackgroundBundles;
        [SerializeField] private DataSpriteBundle[] _dataButtonBackgroundBundles;

        private AssetBundle _spritesAssetBundle;
        private AssetBundle _audioAssetBundle;
        private AssetBundle _backgroundsAssetBundle;
        private AssetBundle _buttonBackgroundsAssetBundle;


        protected IEnumerator DownloadAndSetAssetBundles()
        {
            yield return GetSpritesAssetBundle();
            yield return GetAudioAssetBundle();

            if (_spritesAssetBundle != null)
                SetSpriteAssets(_spritesAssetBundle);
            else
                Debug.LogError($"AssetBundle {nameof(_spritesAssetBundle)} failed to load");

            if (_audioAssetBundle != null)
                SetAudioAssets(_audioAssetBundle);
            else
                Debug.LogError($"AssetBundle {nameof(_audioAssetBundle)} failed to load");
        }

        protected IEnumerator DownloadAndSetBackgroundAssetBundle()                                 
        {                                                                                           
            yield return GetBackgroundAssetBundle();                                                
                                                                                                     
            if (_backgroundsAssetBundle != null)                                                     
                SetBackgroundAssets(_backgroundsAssetBundle);                                        
            else                                                                                     
                Debug.LogError($"AssetBundle {nameof(_backgroundsAssetBundle)} failed to load");     
        }

        protected IEnumerator DownloadAndSetButtonBackgroundAssetBundle()
        {
            yield return GetButtonBackgroundAssetBundle();

            if (_buttonBackgroundsAssetBundle != null)
                SetButtonBackgroundAssets(_buttonBackgroundsAssetBundle);
            else
                Debug.LogError($"AssetBundle {nameof(_buttonBackgroundsAssetBundle)} failed to load");
        }

        private IEnumerator GetBackgroundAssetBundle()                                                            
        {                                                                                                          
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleBackgrounds);        
                                                                                                                   
            yield return request.SendWebRequest();                                                                 
                                                                                                                   
            while (!request.isDone)                                                                                
                yield return null;                                                                                 
                                                                                                                   
            StateRequest(request, out _backgroundsAssetBundle);                                                    
        }

        private IEnumerator GetButtonBackgroundAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleBackgrounds);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _buttonBackgroundsAssetBundle);
        }

        private IEnumerator GetSpritesAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleSprites);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _spritesAssetBundle);
        }

        private IEnumerator GetAudioAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleAudio);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _audioAssetBundle);
        }

        private void StateRequest(UnityWebRequest request, out AssetBundle assetBundle)
        {
            if (request.error == null)
            {
                assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                Debug.Log("Complete");
            }
            else
            {
                assetBundle = null;
                Debug.LogError(request.error);
            }
        }

                                                                                                  
        private void SetSpriteAssets(AssetBundle assetBundle)
        {
            foreach (DataSpriteBundle data in _dataSpriteBundles)
                data.Image.sprite = assetBundle.LoadAsset<Sprite>(data.NameAssetBundle);
        }

        private void SetAudioAssets(AssetBundle assetBundle)
        {
            foreach (DataAudioBundle data in _dataAudioBundles)
            {
                data.AudioSource.clip = assetBundle.LoadAsset<AudioClip>(data.NameAssetBundle);
                data.AudioSource.Play();
            }
        }

        private void SetBackgroundAssets(AssetBundle assetBundle)                                 
        {                                                                                         
            foreach (DataSpriteBundle data in _dataBackgroundBundles)                             
                data.Image.sprite = assetBundle.LoadAsset<Sprite>(data.NameAssetBundle);          
        }

        private void SetButtonBackgroundAssets(AssetBundle assetBundle)
        {
            foreach (DataSpriteBundle data in _dataButtonBackgroundBundles)
                data.Image.sprite = assetBundle.LoadAsset<Sprite>(data.NameAssetBundle);
        }
    }
}
