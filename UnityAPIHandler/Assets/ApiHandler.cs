using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using TMPro; //This namespace allows you to use Text Mesh Pro related actions
using System;
using System.Net; //Provides a programming interface for modern HTTP applications, including HTTP client components that allow applications to consume web services over HTTP
using System.IO;
using UnityEngine.SceneManagement;

public class ApiHandler : MonoBehaviour
{

	public static ApiHandler apiHandler;

	void Awake()
	{
		apiHandler = this;

	}

	public void MakeRequest(ApiRequest apiRequest, System.Action<RequestType, string, bool> callBack)
	{
		if (apiRequest.apiType == ApiType.POST)
		{
			if (apiRequest._temp.Contains("N"))
			{
				StartCoroutine(NewPostMethod(apiRequest, callBack));
			}
			else
			{
				StartCoroutine(PostRequest(apiRequest, callBack));
			}
			//
		}else
			StartCoroutine(GetRequest(apiRequest, callBack));
	}


	public void PaymentWithdraw(System.Action<string, bool> callBack, string amount){
		StartCoroutine(PayementWithDraw(callBack,amount));
	}


	IEnumerator PayementWithDraw(System.Action<string, bool> callBack, string amount){
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("amount", amount);
		wWWForm.AddField("wallet_type","winning");
		UnityWebRequest uwr = UnityWebRequest.Post(Constant.Withdraw_PAYMENT_URL, wWWForm);
		uwr.SetRequestHeader("TOKEN",UserInfo.userInfo.GetUser().slug);
		uwr.SetRequestHeader("LANG", "1");
		uwr.SetRequestHeader("DEVICE-ID", SystemInfo.deviceUniqueIdentifier);
		uwr.SetRequestHeader("DEVICETYPE", "A");
		uwr.SetRequestHeader("APPINFO", "1.0.1 | 1");
		uwr.SetRequestHeader("DEVICEINFO", "Board=violet | Brand=xiaomi | Model=Redmi Note 7 Pro | Product=violet | PhoneModel=Redmi Note 7 Pro | AndroidVersion=9 | PackageName=com.imuons.addiqtion | buildNumber=1.0.1 (1)");


		yield return uwr.SendWebRequest();
		if (!uwr.isNetworkError)
		{
			callBack(uwr.downloadHandler.text, false);
		}
		else
		{
			callBack(uwr.downloadHandler.text, true);
		}
	}


	public void PayemntProceed(System.Action<string, bool> callBack,string amount){
		StartCoroutine(PayementRequest(callBack,amount));
	}


	IEnumerator PayementRequest(System.Action<string, bool> callBack,string amount){
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("amount",amount);
		UnityWebRequest uwr = UnityWebRequest.Post(Constant.ADD_PAYMENT_URL, wWWForm);
		Debug.Log(UserInfo.userInfo.GetUser().slug);
		uwr.SetRequestHeader("token",UserInfo.userInfo.GetUser().slug);
		uwr.SetRequestHeader("LANG", "1");
		uwr.SetRequestHeader("DEVICE-ID", SystemInfo.deviceUniqueIdentifier);
		uwr.SetRequestHeader("DEVICETYPE", "A");
		uwr.SetRequestHeader("APPINFO", "1.0.1 | 1");
		uwr.SetRequestHeader("DEVICEINFO", "Board=violet | Brand=xiaomi | Model=Redmi Note 7 Pro | Product=violet | PhoneModel=Redmi Note 7 Pro | AndroidVersion=9 | PackageName=com.imuons.addiqtion | buildNumber=1.0.1 (1)");


		yield return uwr.SendWebRequest();
		if (!uwr.isNetworkError)
		{
			callBack(uwr.downloadHandler.text, false);
		}
		else
		{
			callBack(uwr.downloadHandler.text, true);
		}
	}


	IEnumerator NewPostMethod(ApiRequest req, System.Action<RequestType, string, bool> callBack)
	{
		string data = "?";
		foreach (var item in req.data)
		{
			string ex = item.Key + "=" + item.Value + "&";
			data += ex;
		}
		var uwr = WebRequest.Create(Constant.API[(int)req.requestType]+ data);
		if (uwr != null)
		{
			if (req.requestType != RequestType.UpdateToken && req.requestType != RequestType.SignUp && req.requestType != RequestType.Login && req.requestType != RequestType.VerifyOTP && req.requestType != RequestType.VerifyPin && req.requestType != RequestType.Login_mobile)
			{
				uwr.Headers.Add("TOKEN", UserInfo.userInfo.GetUser().slug);
			}
			uwr.Method = "POST";

			uwr.Timeout = 10000;
			uwr.Headers.Add("LANG", "1");
			uwr.Headers.Add("DEVICE-ID", SystemInfo.deviceUniqueIdentifier);
			uwr.Headers.Add("DEVICETYPE", "A");
			uwr.Headers.Add("APPINFO", "1.0.1 | 1");
			uwr.Headers.Add("DEVICEINFO", "Board=violet | Brand=xiaomi | Model=Redmi Note 7 Pro | Product=violet | PhoneModel=Redmi Note 7 Pro | AndroidVersion=9 | PackageName=com.imuons.addiqtion | buildNumber=1.0.1 (1)");
			try
			{
				using (Stream s = uwr.GetResponse().GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(s))
					{
						var serverResponse = streamReader.ReadToEnd();
						callBack(req.requestType, serverResponse, false);
					}
				}
			}
			catch (Exception)
			{
				callBack(req.requestType, "", true);
			}
		}
		yield return 1;


	}

	public void ImageDownload()
	{
		StartCoroutine(DownloadImage());
	}

	public IEnumerator DownloadImage()
	{
		if (string.IsNullOrEmpty(UserInfo.userInfo.GetUser().profile_picURL))
			yield return null;
		UnityWebRequest request = UnityWebRequestTexture.GetTexture(UserInfo.userInfo.GetUser().profile_picURL);
		yield return request.SendWebRequest();
		if (request.isNetworkError || request.isHttpError)
		{
			Debug.Log(request.error);
		}
		else
		{
			Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
			Sprite profilePic = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
			DashboardHandler.dashboardHandler.UpdateMyPics(profilePic);
			SceneHolder.Instance.dp = profilePic;
			UserInfo.userInfo.GetUser().profilePic = profilePic;
			UserInfo.userInfo.GetUser().pic = texture;

		}
		if (!string.IsNullOrEmpty(UserInfo.userInfo.GetUser().bank_image_link))
		{
			UnityWebRequest bank_request = UnityWebRequestTexture.GetTexture(UserInfo.userInfo.GetUser().bank_image_link);
			yield return bank_request.SendWebRequest();
			if (bank_request.isNetworkError || bank_request.isHttpError)
			{
				Debug.Log(bank_request.error);
			}
			else
			{
				Texture2D texture = ((DownloadHandlerTexture)bank_request.downloadHandler).texture;
				Sprite profilePic = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
				UserInfo.userInfo.GetUser().bank_proof = profilePic;

			}
		}

		if (!string.IsNullOrEmpty(UserInfo.userInfo.GetUser().pan_image_link))
		{
			UnityWebRequest pan_request = UnityWebRequestTexture.GetTexture(UserInfo.userInfo.GetUser().pan_image_link);
			yield return pan_request.SendWebRequest();
			if (pan_request.isNetworkError || pan_request.isHttpError)
			{
				Debug.Log(request.error);
			}
			else
			{
				Texture2D texture = ((DownloadHandlerTexture)pan_request.downloadHandler).texture;
				Sprite profilePic = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
				UserInfo.userInfo.GetUser().pan_proof = profilePic;

			}
		}

	}

	IEnumerator PostRequest(ApiRequest req, System.Action<RequestType, string, bool> callBack)
	{
		UnityWebRequest uwr = UnityWebRequest.Post(Constant.API[(int)req.requestType], req.formData);
		if (req.requestType != RequestType.UpdateToken && req.requestType != RequestType.SignUp && req.requestType != RequestType.Login && req.requestType != RequestType.VerifyOTP && req.requestType != RequestType.VerifyPin && req.requestType != RequestType.Login_mobile)
		{
			uwr.SetRequestHeader("TOKEN", UserInfo.userInfo.GetUser().slug);
		}

		uwr.SetRequestHeader("LANG", "1");
		uwr.SetRequestHeader("DEVICE-ID", SystemInfo.deviceUniqueIdentifier);
		uwr.SetRequestHeader("DEVICETYPE", "A");
		uwr.SetRequestHeader("APPINFO", "1.0.1 | 1");
		uwr.SetRequestHeader("DEVICEINFO", "Board=violet | Brand=xiaomi | Model=Redmi Note 7 Pro | Product=violet | PhoneModel=Redmi Note 7 Pro | AndroidVersion=9 | PackageName=com.imuons.addiqtion | buildNumber=1.0.1 (1)");


		yield return uwr.SendWebRequest();
		if (!uwr.isNetworkError)
		{
			callBack(req.requestType, uwr.downloadHandler.text, false);
		}
		else
		{
			callBack(req.requestType, uwr.downloadHandler.text, true);
		}
	}

	IEnumerator GetRequest(ApiRequest req, System.Action<RequestType, string, bool> callBack)
	{
		UnityWebRequest uwr = UnityWebRequest.Get(Constant.API[(int)req.requestType]);

		if (req.requestType != RequestType.Login)
		{
			//uwr.SetRequestHeader("Authorization", "Bearer " + GameManager.gameManager.GetUser().token);
		}

		yield return uwr.SendWebRequest();
		if (!uwr.isNetworkError)
		{
			callBack(req.requestType, uwr.downloadHandler.text, false);
		}
		else
		{
			callBack(req.requestType, uwr.downloadHandler.text, true);
		}
	}






}

[System.Serializable]
public enum RequestType
{
	Login,
	SignUp,
	Login_mobile,
	VerifyOTP,
	SignOut,
	ForgotPassword,
	Notification,
	VerifyPin,
	Updateprofile,
	Checkappversion,
	UploadPan,
	UploadBank,
	StatFetch,
	SaveProfileData,
	UpdateToken,
	UpdateProfilePic,
	GetWalletHistory

}

public enum ApiType
{
	GET,
	POST
}



[System.Serializable]
public class ApiRequest
{
	public RequestType requestType;
	public ApiType apiType;
	public string jsonData;
	public string _temp = "O";
	public WWWForm formData;
	public Dictionary<string, string> data = new Dictionary<string, string>();
}




