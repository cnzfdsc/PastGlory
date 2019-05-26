using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对输入的封装. 单例
/// 这个脚本挂在这个Prefab上: System/Input/InputManager
/// 鼠标点击和触摸, 封装为InputTouch
/// 按键和虚拟按钮, 封装为InputButton
/// </summary>
public class skInput : MonoBehaviour
{
	#region Const变量
	/// <summary>
	/// 按下鼠标按键和松开鼠标按键之间, 最长间隔多久会被认为是一次单击
	/// </summary>
	public float CLICK_THRESHOLD = 0.1f;

	/// <summary>
	/// 按下鼠标按键最短多久会被认为是一次按住
	/// </summary>
	public float HOLD_THRESHOLD = 0.2f;

	/// 两次松开鼠标按键之间, 最长间隔多久会被认为是一次双击
	/// </summary>
	public float MAX_DOUBLE_CLICK_TIME = 0.2f;
	#endregion

	#region Private变量
	private static skInput m_Instance = null;

	private const int MAX_TOUCH = 10;
	InputTouch[] m_InputTouchs;

	// 定义一个InputButton类, InputButton有一个ButtonIndex, 对应的keyCode 或者 对应的虚拟按键
	// ButtonIndex用来定义button的功能, 或者说名字, 比如Fire, LaunchMissile, Attack, Patrol. 是一个枚举
	// UpdateButton在skInput的Update中做, 在UpdateTouch之后, 因为虚拟按键的判断基于触摸和点击事件
	// InputButton对应KeyCode时, 按下判断直接在UpdateButton中用Input.GetKeyCode来做
	// InputButton对应虚拟按键时, 在虚拟按键的类中包含一个ButtonIndex, 在编辑器中定义虚拟按键映射到的InputButton. 按下判断在UpdateButton时判断Touch点击到的虚拟按键
	#endregion

	#region  Public方法
	public static skInput Instance
	{
		get
		{
			return m_Instance;
		}
	}

	public InputTouch[] GetTouches()
	{
		return m_InputTouchs;
	}

#if UNITY_EDITOR || UNITY_STANDALONE
	public bool GetLeftButtonDown(out InputTouch touch)
	{
		if (m_InputTouchs[0].IsValid() && m_InputTouchs[0].Phase == skTouchPhase.Moved)
		{
			touch = m_InputTouchs[0];
			return true;
		}
		else
		{
			touch = null;
			return false;
		}
	}

	public bool GetLeftButtonHold(out InputTouch touch)
	{
		if (m_InputTouchs[0].IsValid() && m_InputTouchs[0].Phase == skTouchPhase.Moved)
		{
			touch = m_InputTouchs[0];
			return true;
		}
		else
		{
			touch = null;
			return false;
		}
	}

	public bool GetLeftButtonUp(out InputTouch touch)
	{
		if (m_InputTouchs[0].IsValid() && m_InputTouchs[0].Phase == skTouchPhase.Ended)
		{
			touch = m_InputTouchs[0];
			return true;
		}
		else
		{
			touch = null;
			return false;
		}
	}

	public bool GetLeftClick(out InputTouch touch)
	{
		if (m_InputTouchs[0].IsValid() && m_InputTouchs[0].Click)
		{
			touch = m_InputTouchs[0];
			return true;
		}
		else
		{
			touch = null;
			return false;
		}
	}

	public bool GetRightClick(out InputTouch touch)
	{
		if (m_InputTouchs[1].IsValid() && m_InputTouchs[1].Click)
		{
			touch = m_InputTouchs[1];
			return true;
		}
		else
		{
			touch = null;
			return false;
		}
	}

	public bool GetLeftDoubleClick(out InputTouch touch)
	{
		if (m_InputTouchs[0].IsValid() && m_InputTouchs[0].TapCount == 2)
		{
			touch = m_InputTouchs[0];
			return true;
		}
		else
		{
			touch = null;
			return false;
		}
	}

	public Vector3 GetMousePosition()
	{
		return Input.mousePosition;
	}
#endif

	#endregion

	#region  Unity消息
	protected void Awake()
	{
		if (m_Instance != null)
			skDebug.Assert(false, "重复创建skInput", gameObject);

		m_Instance = this;
		DontDestroyOnLoad(this);

		m_InputTouchs = new InputTouch[MAX_TOUCH];
		for (int i = 0; i < m_InputTouchs.Length; i++)
			m_InputTouchs[i] = new InputTouch();
	}
	protected void Start()
	{

	}

	protected void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		// 把鼠标点击映射为Touch
		// 注意, 在PC平台和编辑器下, 把左键映射为第一个Touch, 即m_InputTouch[0]; 右键映射为第二个Touch, 即m_InputTouch[1]
		// 因为要判断点击和双击, 不能清空上一帧的Touch信息
		if (Input.GetMouseButton(0)) // 鼠标左键
		{
			InputTouch touch0 = m_InputTouchs[0];
			bool justActivated = touch0.!IsValid();
			touch0.FingerID = 0;
			touch0.Click = false;
			// Windows上不触发skTouchPhase.Canceled状态. 鼠标按键松开就是skTouchPhase.Ended
			// Windows上不触发skTouchPhase.stationary状态. 不记得有什么操作需要鼠标在同一个地方按下不动的.
			touch0.Phase = skTouchPhase.Began;
			touch0.ScreenPoint = Input.mousePosition;
			touch0.TapCount = 1;
			touch0._SetBeginTime(Time.realtimeSinceStartup);

			if (justActivated)
			{
				touch0._SetBeginTime(Time.realtimeSinceStartup);
				touch0.Phase = skTouchPhase.Began;
			}
			else
			{
				if (Time.realtimeSinceStartup - touch0._GetBeginTime() > HOLD_THRESHOLD)
					touch0.Phase = skTouchPhase.Moved;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			InputTouch touch0 = m_InputTouchs[0];
			touch0.FingerID = 0;
			touch0.Click = Time.realtimeSinceStartup - touch0._GetBeginTime() < CLICK_THRESHOLD;
			touch0.Phase = skTouchPhase.Ended;
			touch0.ScreenPoint = Input.mousePosition;
			touch0.TapCount = Time.realtimeSinceStartup - touch0._GetEndTime() < MAX_DOUBLE_CLICK_TIME ? 2 : 1;
			touch0._SetEndTime(Time.realtimeSinceStartup);
			if (Time.realtimeSinceStartup - touch0._GetBeginTime() < CLICK_THRESHOLD)
				touch0.Phase = skTouchPhase.Click;
		}
		else
		{
			m_InputTouchs[0].Reset();
		}

		if (Input.GetMouseButton(1)) // 鼠标右键
		{
			InputTouch touch1 = m_InputTouchs[1];
			bool justActivated = touch1.!IsValid();
			touch1.FingerID = 1;
			touch1.Click = false;
			touch1.Phase = skTouchPhase.Began;
			touch1.ScreenPoint = Input.mousePosition;
			touch1.TapCount = 1;

			if (justActivated)
			{
				touch1._SetBeginTime(Time.realtimeSinceStartup);
				touch1.Phase = skTouchPhase.Began;
			}
			else
			{
				if (Time.realtimeSinceStartup - touch1._GetBeginTime() > HOLD_THRESHOLD)
					touch1.Phase = skTouchPhase.Moved;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			InputTouch touch1 = m_InputTouchs[1];
			touch1.FingerID = 0;
			touch1.Click = Time.realtimeSinceStartup - touch1._GetBeginTime() < CLICK_THRESHOLD;
			touch1.ScreenPoint = Input.mousePosition;
			touch1.TapCount = Time.realtimeSinceStartup - touch1._GetEndTime() < MAX_DOUBLE_CLICK_TIME ? 2 : 1;
			touch1._SetEndTime(Time.realtimeSinceStartup);
			if (Time.realtimeSinceStartup - touch1._GetBeginTime() < CLICK_THRESHOLD)
				touch1.Phase = skTouchPhase.Click;
		}
		else
		{
			m_InputTouchs[1].Reset();
		}
#else
	// 更新InputTouch
	for (int i = 0; i < m_InputTouchs.Length; i++)
		m_InputTouchs[i].Reset();

	// for (int i = 0; i < Input.touchCount; i++)
	//	m_InputTouchs[i] = Input.GetTouch(i);
#endif
	}
	#endregion
}


/// <summary>
/// 对应每一次触摸/点击
/// TODO, 要不要改成struct增加缓存命中
/// </summary>
public class InputTouch
{
	/// <summary>
	/// 表示当前Touch没有被激活
	/// </summary>
	public static int INVALID_TOUCH = -1;

	/// <summary>
	/// FingerID不光表示触摸的ID, 还用来表示当前InputTouch的数据是否可用.
	/// 如果FingerID小于0, 则这个InputTouch没有使用. 即: FingerID >= 0 相当于Enabled, FingerID < 0 相当于Disabled
	/// </summary>
	public int FingerID;
	public Vector2 ScreenPoint;
	public skTouchPhase Phase;
	/// <summary>
	/// 这一帧中是否完成了一次点击(左键右键触摸均可)
	/// </summary>
	public bool Click;
	/// <summary>
	/// 点击次数, 双击这个值就为2
	/// UNDONE, 使用当前机制去判断触摸屏双击不现实
	/// </summary>
	public int TapCount;
	/// <summary>
	/// 上一次鼠标按下的时间. 用于判断单击
	/// </summary>
	private float BeginTime;
	/// <summary>
	/// 上一次鼠标抬起的时间, 用于判断双击
	/// </summary>
	private float EndTime;

	public InputTouch()
	{
		FingerID = INVALID_TOUCH;
		ScreenPoint = Vector2.zero;
		Phase = skTouchPhase.Ended;
		Click = false;
		TapCount = 0;
		BeginTime = -1;
		EndTime = -1;
	}

	public bool IsValid()
	{
		return FingerID != INVALID_TOUCH;
	}

	public void Reset()
	{
		FingerID = -1;
		// 不能清理BeginTime和endTime, 用于判断点击和双击. 其实只要不清理EndTime就行. 对称一下, 都别清了
		// 其他属性也没必要清理, 因为每次都会重设. 只有BeginTime和EndTime会依赖上一次点击的数据
	}

	/// <summary>
	/// 内部方法, 用于计算单击. 不应该在skInput外调用
	/// </summary>
	/// <param name="time"></param>
	public void _SetBeginTime(float time)
	{
		BeginTime = time;
	}
	/// <summary>
	/// 内部方法, 用于计算单击. 不应该在skInput外调用
	/// </summary>
	/// <param name="time"></param>
	public float _GetBeginTime()
	{
		return BeginTime;
	}
	/// <summary>
	/// 内部方法, 用于计算双击. 不应该在skInput外调用
	/// </summary>
	/// <param name="time"></param>
	public void _SetEndTime(float time)
	{
		EndTime = time;
	}
	/// <summary>
	/// 内部方法, 用于计算双击. 不应该在skInput外调用
	/// </summary>
	/// <param name="time"></param>
	public float _GetEndTime()
	{
		return EndTime;
	}
}


public enum skTouchPhase
{
	Began,
	Click,
	Moved,
	Stationary,
	Ended,
	Canceled,
}