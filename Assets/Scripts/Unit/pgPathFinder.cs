using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pgPathFinder : MonoBehaviour
{
	#region  Public方法
	/// <summary>
	/// 
	/// </summary>
	/// <param name="unit">寻路单位</param>
	/// <param name="destination">目的地</param>
	/// <param name="wayPoints">所有路径点, 正序</param>
	/// <returns>目标是否可达</returns>
	public bool FindPath(pgUnit unit, Vector3 destination, ref List<Vector3> wayPoints)
	{
		wayPoints.Clear();
		wayPoints.Add(destination);
		return true;
	}
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
	#endregion

	#region Private变量
	#endregion
}
