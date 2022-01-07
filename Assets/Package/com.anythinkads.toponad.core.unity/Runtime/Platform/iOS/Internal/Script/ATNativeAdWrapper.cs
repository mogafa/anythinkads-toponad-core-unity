using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AOT;
using AnyThinkAds.ThirdParty.LitJson;
using AnyThinkAds.iOS;

public class ATNativeAdWrapper:ATAdWrapper {
    static private Dictionary<string, ATNativeAdClient> clients;
    static private string CMessageReceiverClass = "ATNativeAdWrapper";

    static public new void InvokeCallback(JsonData jsonData) {
        Debug.Log("Unity: ATNativeAdWrapper::InvokeCallback()");
        string extraJson = "";
        string callback = (string)jsonData["callback"];
        Dictionary<string, object> msgDict = JsonMapper.ToObject<Dictionary<string, object>>(jsonData["msg"].ToJson());
        JsonData msgJsonData = jsonData["msg"];
        IDictionary idic = (System.Collections.IDictionary)msgJsonData;

        if (idic.Contains("extra")) { 
            JsonData extraJsonDate = msgJsonData["extra"];
            if (extraJsonDate != null) {
                extraJson = msgJsonData["extra"].ToJson();
            }
        }

        if (callback.Equals("OnNativeAdLoaded")) {
    		OnNativeAdLoaded((string)msgDict["placement_id"]);
    	} else if (callback.Equals("OnNativeAdLoadingFailure")) {
    		Dictionary<string, object> errorDict = new Dictionary<string, object>();
            Dictionary<string, object> errorMsg = JsonMapper.ToObject<Dictionary<string, object>>(msgJsonData["error"].ToJson());
    		if (errorMsg.ContainsKey("code")) { errorDict.Add("code", errorMsg["code"]); }
            if (errorMsg.ContainsKey("reason")) { errorDict.Add("message", errorMsg["reason"]); }
    		OnNativeAdLoadingFailure((string)msgDict["placement_id"], errorDict);
    	} else if (callback.Equals("OnNaitveAdShow")) {
    		OnNaitveAdShow((string)msgDict["placement_id"], extraJson);
    	} else if (callback.Equals("OnNativeAdClick")) {
    		OnNativeAdClick((string)msgDict["placement_id"], extraJson);
    	} else if (callback.Equals("OnNativeAdVideoStart")) {
    		OnNativeAdVideoStart((string)msgDict["placement_id"]);
    	} else if (callback.Equals("OnNativeAdVideoEnd")) {
    		OnNativeAdVideoEnd((string)msgDict["placement_id"]);
        } else if (callback.Equals("OnNativeAdCloseButtonClick")) {
            OnNativeAdCloseButtonClick((string)msgDict["placement_id"], extraJson);
        }
    }

    //Public method(s)
    static public void setClientForPlacementID(string placementID, ATNativeAdClient client) {
        if (clients == null) clients = new Dictionary<string, ATNativeAdClient>();
        if (clients.ContainsKey(placementID)) clients.Remove(placementID);
        clients.Add(placementID, client);
    }

    static public void loadNativeAd(string placementID, string customData) {
    	Debug.Log("Unity: ATNativeAdWrapper::loadNativeAd(" + placementID + ")");
    	ATUnityCBridge.SendMessageToC(CMessageReceiverClass, "loadNativeAdWithPlacementID:customDataJSONString:callback:", new object[]{placementID, customData != null ? customData : ""}, true);
    }

    static public bool isNativeAdReady(string placementID) {
        Debug.Log("Unity: ATNativeAdWrapper::isNativeAdReady(" + placementID + ")");
    	return ATUnityCBridge.SendMessageToC(CMessageReceiverClass, "isNativeAdReadyForPlacementID:", new object[]{placementID});
    }

    static public string checkAdStatus(string placementID) {
        Debug.Log("Unity: ATNativeAdWrapper::checkAdStatus(" + placementID + ")");
        return ATUnityCBridge.GetStringMessageFromC(CMessageReceiverClass, "checkAdStatus:", new object[]{placementID});
    }

    static public string getValidAdCaches(string placementID)
    {
        Debug.Log("Unity: ATNativeAdWrapper::getValidAdCaches(" + placementID + ")");
        return ATUnityCBridge.GetStringMessageFromC(CMessageReceiverClass, "getValidAdCaches:", new object[] { placementID });
    }

    static public void showNativeAd(string placementID, string metrics) {
	    Debug.Log("Unity: ATNativeAdWrapper::showNativeAd(" + placementID + ")");
    	ATUnityCBridge.SendMessageToC(CMessageReceiverClass, "showNativeAdWithPlacementID:metricsJSONString:extraJsonString:", new object[]{placementID, metrics, null});
    }

    static public void showNativeAd(string placementID, string metrics, string mapJson) {
        Debug.Log("Unity: ATNativeAdWrapper::showNativeAd(" + placementID + ")");
        ATUnityCBridge.SendMessageToC(CMessageReceiverClass, "showNativeAdWithPlacementID:metricsJSONString:extraJsonString:", new object[]{placementID, metrics, mapJson});
    }

    static public void removeNativeAdView(string placementID) {
        Debug.Log("Unity: ATNativeAdWrapper::removeNativeAdView(" + placementID + ")");
        ATUnityCBridge.SendMessageToC(CMessageReceiverClass, "removeNativeAdViewWithPlacementID:", new object[]{placementID});
    }

    static public void clearCache() {
        Debug.Log("Unity: ATNativeAdWrapper::clearCache()");
    	ATUnityCBridge.SendMessageToC(CMessageReceiverClass, "clearCache", null);
    }

	//Callbacks
	static private void OnNativeAdLoaded(string placementID) {
		Debug.Log("Unity: ATNativeAdWrapper::OnNativeAdLoaded(" + placementID + ")");
        if (clients[placementID] != null) clients[placementID].onNativeAdLoaded(placementID);
	}

	static private void OnNativeAdLoadingFailure(string placementID, Dictionary<string, object> errorDict) {
		Debug.Log("Unity: ATNativeAdWrapper::OnNativeAdLoadingFailure()");
        if (clients[placementID] != null) clients[placementID].onNativeAdLoadFail(placementID, (string)errorDict["code"], (string)errorDict["message"]);
	}

    static private void OnNaitveAdShow(string placementID, string callbackJson) {
        if (clients[placementID] != null) clients[placementID].onAdImpressed(placementID, callbackJson);
		Debug.Log("Unity: ATNativeAdWrapper::OnNaitveAdShow(" + placementID + ")");
	}

    static private void OnNativeAdClick(string placementID, string callbackJson) {
        if (clients[placementID] != null) clients[placementID].onAdClicked(placementID, callbackJson);
		Debug.Log("Unity: ATNativeAdWrapper::OnNativeAdClick(" + placementID + ")");
	}

	static private void OnNativeAdVideoStart(string placementID) {
        if (clients[placementID] != null) clients[placementID].onAdVideoStart(placementID);
		Debug.Log("Unity: ATNativeAdWrapper::OnNativeAdVideoStart(" + placementID + ")");
	}

	static private void OnNativeAdVideoEnd(string placementID) {
        if (clients[placementID] != null) clients[placementID].onAdVideoEnd(placementID);
		Debug.Log("Unity: ATNativeAdWrapper::OnNativeAdVideoEnd(" + placementID + ")");
	}

    static private void OnNativeAdCloseButtonClick(string placementID, string callbackJson)
    {
        if (clients[placementID] != null) clients[placementID].onAdCloseButtonClicked(placementID, callbackJson);
        Debug.Log("Unity: ATNativeAdWrapper::OnNativeAdCloseButtonClick(" + placementID + ")");
    }
}
