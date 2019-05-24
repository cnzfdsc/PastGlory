using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skUIUtility : MonoBehaviour
{
	static private Texture2D m_WhiteTexture;
	static public Texture2D GetWhiteTexture()
	{
		if (m_WhiteTexture == null)
		{
			m_WhiteTexture = new Texture2D(1, 1);
			m_WhiteTexture.SetPixel(0, 0, Color.white);
			m_WhiteTexture.Apply();
		}

		return m_WhiteTexture;
	}

	static public void DrawScreenRect(Rect rect)
	{
		GUI.DrawTexture(rect, GetWhiteTexture());
	}
}
