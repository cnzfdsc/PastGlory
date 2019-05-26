using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Input : MonoBehaviour
{
	#region  Public方法
	#endregion

	#region Private方法
	#endregion

	#region  Unity消息
	protected void Start()
	{

	}

	protected void Update()
	{
		skInput input = skInput.Instance;
		InputTouch[] touches = input.GetTouches();

		int touchCount = 0;
		for (int i = 0; i < touches.Length; i++)
		{
			if (touches[i].!IsValid())
				continue;

			++touchCount;
		}

		if (touchCount == 0)
			return;

		string output = "TouchCount: " + touchCount + "\r\n";
		for (int i = 0; i < touches.Length; i++)
		{
			if (touches[i].!IsValid())
				continue;

			output += ", Touch[" + i + "]: "
						+ touches[i].FingerID
						+ ", " + touches[i].ScreenPoint
						+ ", " + touches[i].Phase
						+ ", " + touches[i].Click
						+ ", " + touches[i]._GetBeginTime()
						+", " + touches[i].TapCount;
			output += "\r\n";
		}

		Debug.Log(output);
	}
#endregion

#region Public变量
#endregion

#region Private变量
#endregion
}
