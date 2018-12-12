using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pgUnit : MonoBehaviour
{
	#region  Public方法
	/// <summary>
	/// 移动到指定位置.
	/// 单位总是沿着地表移动, 就算地形有高低起伏, 也是移动到某一位置后, 从地形信息中获取当前位置的高度
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	public void MoveTo(float x, float z)
	{
		m_Destination = new Vector3(x, 0, z);
		m_Direction = (m_Destination - transform.position).normalized;
		// todo: 有坡地的话这里需要改
		transform.rotation.SetLookRotation(m_Direction);
	}
	#endregion

	#region Private方法
	#endregion

	#region  Unity消息
	protected void Start()
	{
		m_Direction = Vector3.forward;
	}

	protected void Update()
	{
		#region 单位移动
		transform.localPosition += m_Speed * m_Direction;
		#endregion
	}
	#endregion

	#region Public变量
	#endregion

	#region Private变量

	/// <summary>
	/// 移动的目的地
	/// </summary>
	private Vector3 m_Destination;

	/// <summary>
	/// 单位朝向. 指定目的地时改变这个值
	/// </summary>
	private Vector3 m_Direction;

	/// <summary>
	/// 单位移动速度. 暂时没有加速度
	/// </summary>
	private float m_Speed;

	// TODO, TweakableProperty
	#endregion
}
