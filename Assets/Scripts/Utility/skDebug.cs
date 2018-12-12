using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class skDebug : MonoBehaviour
{
	#region  Public方法
	public static bool Assert(bool valid, string failedMessage, Object context)
	{
		if (!valid)
		{
			Debug.LogError(failedMessage, context);
#if UNITY_EDITOR
			EditorUtility.DisplayDialog("Assert Failed", failedMessage, "OK");
			Debug.Break();
#endif
		}

		return valid;
	}

	public static bool AssertFormat(bool valid, string format, Object context, params object[] arguments)
	{
		string failedMessage = string.Format(format, arguments);
		return Assert(valid, failedMessage, context);
	}

	#endregion
}
