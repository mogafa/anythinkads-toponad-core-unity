using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AnyThinkAds.Common;
using AnyThinkAds.Api;
namespace AnyThinkAds.Android
{
    public class ATRewardedVideoAdClient : AndroidJavaProxy,IATRewardedVideoAdClient
    {

        private Dictionary<string, AndroidJavaObject> videoHelperMap = new Dictionary<string, AndroidJavaObject>();

		//private  AndroidJavaObject videoHelper;
        private  ATRewardedVideoListener anyThinkListener;

        public ATRewardedVideoAdClient() : base("com.anythink.unitybridge.videoad.VideoListener")
        {
            
        }


        public void loadVideoAd(string placementId, string mapJson)
        {

            if(!videoHelperMap.ContainsKey(placementId))
            {
                AndroidJavaObject videoHelper = new AndroidJavaObject(
                    "com.anythink.unitybridge.videoad.VideoHelper", this);
                videoHelper.Call("initVideo", placementId);
                videoHelperMap.Add(placementId, videoHelper);
                Debug.Log("ATRewardedVideoAdClient : no exit helper ,create helper ");
            }

            try
            {
                Debug.Log("ATRewardedVideoAdClient : loadVideoAd ");
                videoHelperMap[placementId].Call("fillVideo", mapJson);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATRewardedVideoAdClient :  error."+e.Message);
            }


        }

        public void setListener(ATRewardedVideoListener listener)
        {
            anyThinkListener = listener;
        }

        public bool hasAdReady(string placementId)
        {
			bool isready = false;
			Debug.Log ("ATRewardedVideoAdClient : hasAdReady....");
			try{
                if (videoHelperMap.ContainsKey(placementId)) {
                    isready = videoHelperMap[placementId].Call<bool> ("isAdReady");
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATRewardedVideoAdClient :  error."+e.Message);
			}
			return isready; 
        }

        public string checkAdStatus(string placementId)
        {
            string adStatusJsonString = "";
            Debug.Log("ATRewardedVideoAdClient : checkAdStatus....");
            try
            {
                if (videoHelperMap.ContainsKey(placementId))
                {
                    adStatusJsonString = videoHelperMap[placementId].Call<string>("checkAdStatus");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATRewardedVideoAdClient :  error." + e.Message);
            }

            return adStatusJsonString;
        }

        public string getValidAdCaches(string placementId)
        {
            string validAdCachesString = "";
            Debug.Log("ATNativeAdClient : getValidAdCaches....");
            try
            {
                if (videoHelperMap.ContainsKey(placementId))
                {
                    validAdCachesString = videoHelperMap[placementId].Call<string>("getValidAdCaches");
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Exception caught: {0}", e);
                Debug.Log("ATNativeAdClient :  error." + e.Message);
            }

            return validAdCachesString;
        }

        public void showAd(string placementId, string scenario)
        {
			Debug.Log("ATRewardedVideoAdClient : showAd " );

			try{
                if (videoHelperMap.ContainsKey(placementId)) {
                    this.videoHelperMap[placementId].Call ("showVideo", scenario);
				}
			}catch(System.Exception e){
				System.Console.WriteLine("Exception caught: {0}", e);
				Debug.Log ("ATRewardedVideoAdClient :  error."+e.Message);

			}
        }



        public void onRewardedVideoAdLoaded(string placementId)
        {
            Debug.Log("onRewardedVideoAdLoaded...unity3d.");
            if(anyThinkListener != null){
                anyThinkListener.onRewardedVideoAdLoaded(placementId);
            }
        }


        public void onRewardedVideoAdFailed(string placementId,string code, string error)
        {
            Debug.Log("onRewardedVideoAdFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdLoadFail(placementId, code, error);
            }
        }


        public void onRewardedVideoAdPlayStart(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdPlayStart...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayStart(placementId, new ATCallbackInfo(callbackJson));
            }
        }


        public void onRewardedVideoAdPlayEnd(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdPlayEnd...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayEnd(placementId, new ATCallbackInfo(callbackJson));
            }
        }


        public void onRewardedVideoAdPlayFailed(string placementId,string code, string error)
        {
            Debug.Log("onRewardedVideoAdPlayFailed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayFail(placementId, code, error);
            }
        }

        public void onRewardedVideoAdClosed(string placementId,bool isRewarded, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdClosed...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayClosed(placementId,isRewarded, new ATCallbackInfo(callbackJson));
            }
        }

        public void onRewardedVideoAdPlayClicked(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdPlayClicked...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onRewardedVideoAdPlayClicked(placementId, new ATCallbackInfo(callbackJson));
            }
        }


        public void onReward(string placementId, string callbackJson)
        {
            Debug.Log("onReward...unity3d.");
            if (anyThinkListener != null)
            {
                anyThinkListener.onReward(placementId, new ATCallbackInfo(callbackJson));
            }
        }


        public void onRewardedVideoAdAgainPlayStart(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdAgainPlayStart...unity3d.");
            if (anyThinkListener != null && anyThinkListener is ATRewardedVideoExListener)
            {
                ((ATRewardedVideoExListener)anyThinkListener).onRewardedVideoAdAgainPlayStart(placementId, new ATCallbackInfo(callbackJson));
            }
        }

        public void onRewardedVideoAdAgainPlayEnd(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdAgainPlayEnd...unity3d.");
            if (anyThinkListener != null && anyThinkListener is ATRewardedVideoExListener)
            {
                ((ATRewardedVideoExListener)anyThinkListener).onRewardedVideoAdAgainPlayEnd(placementId, new ATCallbackInfo(callbackJson));
            }
        }


        public void onRewardedVideoAdAgainPlayFailed(string placementId, string code, string error)
        {
            Debug.Log("onRewardedVideoAdAgainPlayFailed...unity3d.");
            if (anyThinkListener != null && anyThinkListener is ATRewardedVideoExListener)
            {
                ((ATRewardedVideoExListener)anyThinkListener).onRewardedVideoAdAgainPlayFail(placementId, code, error);
            }
        }


        public void onRewardedVideoAdAgainPlayClicked(string placementId, string callbackJson)
        {
            Debug.Log("onRewardedVideoAdAgainPlayClicked...unity3d.");
            if (anyThinkListener != null && anyThinkListener is ATRewardedVideoExListener)
            {
                ((ATRewardedVideoExListener)anyThinkListener).onRewardedVideoAdAgainPlayClicked(placementId, new ATCallbackInfo(callbackJson));
            }
        }


        public void onAgainReward(string placementId, string callbackJson)
        {
            Debug.Log("onAgainReward...unity3d.");
            if (anyThinkListener != null && anyThinkListener is ATRewardedVideoExListener)
            {
                ((ATRewardedVideoExListener)anyThinkListener).onAgainReward(placementId, new ATCallbackInfo(callbackJson));
            }
        }


    }
}
