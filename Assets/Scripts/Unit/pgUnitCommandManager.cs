using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pgUnitCommandManager : MonoBehaviour
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
		// 选择单位
		m_SelectingUnits.Clear();


		// 操作单位. 攻击/移动/扫荡
	}
	#endregion

	#region Public变量
	#endregion

	#region Private变量
	private List<pgUnitController> m_SelectingUnits = new List<pgUnitController>();
	#endregion
}
