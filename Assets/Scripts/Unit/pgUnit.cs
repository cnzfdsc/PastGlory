using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pgUnit : MonoBehaviour
{
	#region  Public方法
	public float Speed
	{
		get
		{
			return m_Speed;
		}
	}
	#endregion

	#region Private方法
	#endregion

	#region  Unity消息
	protected void Start()
	{
	}
	
	protected void LateUpdate()
	{
	}
	#endregion

	#region Public变量
	#endregion

	#region Private变量

	/////////////放到Controller里面

	/// <summary>
	/// 移动的目的地
	/// 不存这个
	/// </summary>
	private Vector3 m_Destination;

	/// <summary>
	/// 单位朝向. 指定目的地时改变这个值.
	/// TODO, 先不缓存这个
	/// </summary>
	private Vector3 m_Direction;

	/// <summary>
	/// 单位移动速度. 暂时没有加速度
	/// </summary>
	[SerializeField]
	private float m_Speed;

	/////////////放到Controller里面

	// TODO, TweakableProperty
	#endregion
}
