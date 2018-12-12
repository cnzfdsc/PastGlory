using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pgUnitController : MonoBehaviour
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
		
	}
	#endregion

	#region Public变量
	enum E_UnitState
	{
		Move,
		Stop,
		Sweep,
		Patrol,
	}
	#endregion

	#region Private变量
	pgUnit m_Unit;
	#endregion
}
