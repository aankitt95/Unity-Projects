using System.Collections;
using UnityEngine;
using UnityEngine.Experimental;
using UnityEngine.Networking;

public class UnityWebRequestUsage : MonoBehaviour
{
	void Start()
	{
		StartCoroutine(GetText());
	}

	IEnumerator GetText()
	{
		using (UnityWebRequest request = UnityWebRequest.Get("http://unity3d.com/"))
		{
			yield return request.Send();

			if (request.isError) // Error
			{
				Debug.Log(request.error);
			}
			else // Success
			{
				Debug.Log(request.downloadHandler.text);
			}
		}
	}
}