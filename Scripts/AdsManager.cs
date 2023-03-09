using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    string gameId;
    [SerializeField] bool _testMode = true;

    [SerializeField] string androidInterAdUnit = "Interstitial_Android";
    [SerializeField] string iosInterAdUnit = "Interstitial_iOS";
    [SerializeField] string androidRewardAdUnit = "Rewarded_Android";
    [SerializeField] string iosRewardAdUnit = "Rewarded_iOS";
    string interAdUnitId;
    string rewardAdUnitId;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeAds();
        interAdUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosInterAdUnit
            : androidInterAdUnit;

        rewardAdUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iosRewardAdUnit
            : androidRewardAdUnit;
    }

    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        Advertisement.Initialize(gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    void LoadAds()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + interAdUnitId);
        Advertisement.Load(interAdUnitId, this);
        Advertisement.Load(rewardAdUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowInterAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + interAdUnitId);
        Advertisement.Show(interAdUnitId, this);
    }

    public void ShowRewardAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + rewardAdUnitId);
        Advertisement.Show(rewardAdUnitId, this);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(rewardAdUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            FindObjectOfType<MainMenuSrc>().coinCnt += 100;
            FindObjectOfType<MainMenuSrc>().SetPrice();
            Advertisement.Load(rewardAdUnitId, this);
        }
    }
}