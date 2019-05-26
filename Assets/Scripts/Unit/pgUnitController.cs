using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制每个单位的移动/攻击/巡逻/扫荡 等
/// 
/// 目前所有的单位移动相关的逻辑都放在Controller里面, Unit在这里只提供属性
/// 
/// TODO, 攻击的逻辑还没想好, 肯定有一个pgWeapon, 不继承pgUnit, 其中包含表现, 武器所有属性, 武器开火
/// 但是控制pgWeapon的逻辑写在Controller里还是Unit里还没想好
/// </summary>
public class pgUnitController : MonoBehaviour
{
	#region  Public方法
	public void StartMove(Vector3 destination)
	{
		m_PathFinder.FindPath(m_Unit, destination, ref m_WayPoints);

		m_UnitState = UnitState.Move;
		m_CurrentWayPoint = 0;
	}

	public void Stop()
	{
		m_UnitState = UnitState.Stop;
	}
	#endregion

	#region Private方法
	#endregion

	#region  Unity消息
	protected void Start()
	{
		m_Unit = GetComponent<pgUnit>();
		skDebug.AssertFormat(m_Unit != null, "{0}有UnitController, 但是没有Unit", this, this.name);

		m_UnitState = UnitState.Stop;
		m_CurrentWayPoint = -1;
	}

	protected void Update()
	{
		switch (m_UnitState)
		{
			case UnitState.Stop:
				break;

			case UnitState.Move:
				float sqrDis = Vector3.SqrMagnitude(m_Unit.transform.position - m_WayPoints[m_CurrentWayPoint]);
				if (Vector3.SqrMagnitude(m_Unit.transform.position - m_WayPoints[m_CurrentWayPoint]) < ReachedWayPointThreshold * ReachedWayPointThreshold)
				{
					if (++m_CurrentWayPoint >= m_WayPoints.Count)
						Stop();
				}
				else
				{
					Vector3 direction = m_WayPoints[m_CurrentWayPoint] - m_Unit.transform.position;
					direction.y = 0;
					direction.Normalize();
					// TODO, 有坡地的话这里需要改
					m_Unit.transform.rotation = Quaternion.LookRotation(direction);
					m_Unit.transform.position += m_Unit.Speed * Time.deltaTime * direction;
					// TODO, 播放动画
				}
				break;

			case UnitState.Sweep:
				break;

			case UnitState.Patrol:
				break;

			default:
				skDebug.Assert(false, "wrong unit state", this);
				break;
		}
	}
	#endregion

	#region Public变量
	enum UnitState
	{
		Stop,
		Move,
		Sweep,
		Patrol,
	}
	#endregion

	#region Private变量
	[SerializeField]
	[Tooltip("距离路点多远就算到达路点了")]
	private const float ReachedWayPointThreshold = 0.1f;

	private pgUnit m_Unit;
	private UnitState m_UnitState;
	private pgPathFinder m_PathFinder = new pgPathFinder();
	private List<Vector3> m_WayPoints = new List<Vector3>();
	private int m_CurrentWayPoint;
	#endregion
}
