using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择单位. 包括单击选择, 双击选择所有同类单位, 框选等
/// 操作当前选中的单位. 包括移动, 攻击, 巡逻, 扫荡等
/// TODO, 在手机上选中英雄时我想单独启用一套操作方式, 使用虚拟摇杆和技能按键
/// </summary>
public class pgUnitCommandManager : MonoBehaviour
{
	#region  Public方法
	#endregion

	#region Private方法
	#endregion

	#region  Unity消息
	protected void Start()
	{
		if (UnitDestinationEffect != null)
		{
			m_DestinationEffect = GameObject.Instantiate(UnitDestinationEffect, Vector3.zero, Quaternion.identity) as GameObject;
			m_DestinationEffect.SetActive(false);
			if (m_DestinationEffect == null)
				Debug.LogError("Failed to create DestinationEffect", this);
		}
		else
		{
			skDebug.AssertFormat(false, "{0} Missing UnitDestinationEffect", this, this.name);
		}
	}

	private bool m_SelectingByRect = false;
	private Rect m_SelectingRect = new Rect();

	void Update()
	{
		// 选择单位
		// 单击选择
		skInput.InputTouch touch = null;
		if (skInput.Instance.GetLeftClick(out touch))
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(touch.ScreenPoint);
			RaycastHit hit;
			if (Physics.Raycast(mouseRay, out hit, 5000.0f, 1 << skConstant.Layer_Unit))
			{
				Transform unitTransform = hit.transform;
				pgUnitController controller = unitTransform.GetComponent<pgUnitController>();
				if (controller != null)
				{
					m_SelectingUnits.Clear();
					m_SelectingUnits.Add(controller);
					// 选中单位的一帧不判断其他命令
					return;
				}
				else
				{
					Debug.LogError(string.Format("Unit({0}) miss controller", unitTransform.name), unitTransform.gameObject);
				}
			}
		}

		// 框选
		if (skInput.Instance.GetLeftButtonDown(out touch))
		{
			if (!m_SelectingByRect)
			{
				// TODO, 开始画框 

				m_SelectingRect.min = touch.ScreenPoint;
				m_SelectingRect.max = touch.ScreenPoint;
				Debug.Log("开始框选, Rect开始坐标: " + m_SelectingRect.min);
				m_SelectingByRect = true;
			}
			else
			{
				m_SelectingRect.max = touch.ScreenPoint;
				Debug.Log("修改框选, Rect: " + m_SelectingRect);
			}
		}
		
		if (skInput.Instance.GetLeftButtonUp(out touch))
		{
			if (m_SelectingByRect)
			{
				// TODO, 获取框选到的单位

				m_SelectingRect = Rect.zero;
				m_SelectingByRect = false;
				Debug.Log("结束框选, Rect结束坐标: " + m_SelectingRect.max);
			}
		}

		// 操作单位. 攻击/移动/扫荡
		// 移动
		if (skInput.Instance.GetLeftClick(out touch))
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(touch.ScreenPoint);
			RaycastHit hit;
			if (Physics.Raycast(mouseRay, out hit, 5000.0f, 1 << skConstant.Layer_Terrain))
			{
				// 播放特效
				m_DestinationEffect.SetActive(true);
				m_DestinationEffect.transform.position = hit.point;
				ParticleSystem[] destinationEffects = m_DestinationEffect.GetComponentsInChildren<ParticleSystem>();
				if (destinationEffects.Length != 0)
				{
					for (int i = 0; i < destinationEffects.Length; i++)
					{
						destinationEffects[i].Clear();
						destinationEffects[i].Play();
					}
				}
				else
				{
					Debug.LogError("missing ParticleSystem on destinationEffect", m_DestinationEffect);
				}

				// 单位移动
				Vector3 destination = hit.point;
				for (int i = 0; i < m_SelectingUnits.Count; i++)
				{
					// TODO, 选中多个单位移动时, 不同单位路径怎么分配? 这事应该CommandManager管, 还是PathFinder管?
					m_SelectingUnits[i].Move(destination);
				}
			}
		}
	}

	private void OnGUI()
	{
		if (m_SelectingByRect)
		{
			skUIUtility.DrawScreenRect(m_SelectingRect);
		}
	}
	#endregion

	#region Public变量
	// UNDONE, 加一个EffectManager
	public GameObject UnitDestinationEffect;
	#endregion

	#region Private变量
	private List<pgUnitController> m_SelectingUnits = new List<pgUnitController>();
	// UNDONE, 加一个Particle的生命控制组件
	GameObject m_DestinationEffect;
	#endregion
}
