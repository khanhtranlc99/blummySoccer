using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
	private static Dictionary<Type, Stack<global::EventHandler.InvokableActionBase>> m_GenericPool = new Dictionary<Type, Stack<global::EventHandler.InvokableActionBase>>();
	private static Dictionary<object, Dictionary<EventID, List<global::EventHandler.InvokableActionBase>>> s_EventTable = new Dictionary<object, Dictionary<EventID, List<global::EventHandler.InvokableActionBase>>>();
	private static Dictionary<EventID, List<global::EventHandler.InvokableActionBase>> s_GlobalEventTable = new Dictionary<EventID, List<global::EventHandler.InvokableActionBase>>();
	internal abstract class InvokableActionBase
	{
	}
	internal class InvokableAction : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action m_Action;

		public void Initialize(Action action)
		{
			this.m_Action = action;
		}

		public void Invoke()
		{
			this.m_Action();
		}

		public bool IsAction(Action action)
		{
			return this.m_Action == action;
		}
	}
	internal class InvokableAction<T1> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<T1> m_Action;

		public void Initialize(Action<T1> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1)
		{
			this.m_Action(arg1);
		}

		public bool IsAction(Action<T1> action)
		{
			return this.m_Action == action;
		}
	}

	internal class InvokableAction<T1, T2> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<T1, T2> m_Action;

		public void Initialize(Action<T1, T2> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1, T2 arg2)
		{
			this.m_Action(arg1, arg2);
		}

		public bool IsAction(Action<T1, T2> action)
		{
			return this.m_Action == action;
		}
	}

	internal class InvokableAction<T1, T2, T3> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<T1, T2, T3> m_Action;

		public void Initialize(Action<T1, T2, T3> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3)
		{
			this.m_Action(arg1, arg2, arg3);
		}

		public bool IsAction(Action<T1, T2, T3> action)
		{
			return this.m_Action == action;
		}
	}

	internal class InvokableAction<T1, T2, T3, T4> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<T1, T2, T3, T4> m_Action;

		public void Initialize(Action<T1, T2, T3, T4> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			this.m_Action(arg1, arg2, arg3, arg4);
		}

		public bool IsAction(Action<T1, T2, T3, T4> action)
		{
			return this.m_Action == action;
		}
	}
	public delegate void Action<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	internal class InvokableAction<T1, T2, T3, T4, T5> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event global::EventHandler.Action<T1, T2, T3, T4, T5> m_Action;

		public void Initialize(global::EventHandler.Action<T1, T2, T3, T4, T5> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			this.m_Action(arg1, arg2, arg3, arg4, arg5);
		}

		public bool IsAction(global::EventHandler.Action<T1, T2, T3, T4, T5> action)
		{
			return this.m_Action == action;
		}
	}

	internal class InvokableAction<T1, T2, T3, T4, T5, T6> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<T1, T2, T3, T4, T5, T6> m_Action;

		public void Initialize(Action<T1, T2, T3, T4, T5, T6> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			this.m_Action(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		public bool IsAction(Action<T1, T2, T3, T4, T5, T6> action)
		{
			return this.m_Action == action;
		}
	}

	internal class InvokableAction<T1, T2, T3, T4, T5, T6, T7> : global::EventHandler.InvokableActionBase
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private event Action<T1, T2, T3, T4, T5, T6, T7> m_Action;

		public void Initialize(Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			this.m_Action = action;
		}

		public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			this.m_Action(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		public bool IsAction(Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			return this.m_Action == action;
		}
	}
	#region REGISTER
	public static void RegisterEvent(EventID eventName, Action Action)
	{
		global::EventHandler.InvokableAction invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction>();
		invokable.Initialize(Action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}
	public static void RegisterEvent(object obj, EventID eventName, Action action)
	{
		global::EventHandler.InvokableAction invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(obj, eventName, invokable);
	}
	private static void RegisterEvent(EventID eventName, global::EventHandler.InvokableActionBase invokableAction)
	{
		List<global::EventHandler.InvokableActionBase> list;
		if (global::EventHandler.s_GlobalEventTable.TryGetValue(eventName, out list))
		{
			list.Add(invokableAction);
		}
		else
		{
			list = new List<global::EventHandler.InvokableActionBase>();
			list.Add(invokableAction);
			global::EventHandler.s_GlobalEventTable.Add(eventName, list);
		}
	}
	public static void RegisterEvent<T1>(EventID eventName, Action<T1> action)
	{
		global::EventHandler.InvokableAction<T1> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}

	public static void RegisterEvent<T1>(object obj, EventID eventName, Action<T1> action)
	{
		global::EventHandler.InvokableAction<T1> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(obj, eventName, invokable);
	}

	public static void RegisterEvent<T1, T2>(EventID eventName, Action<T1, T2> action)
	{
		global::EventHandler.InvokableAction<T1, T2> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}

	public static void RegisterEvent<T1, T2>(object obj, EventID eventName, Action<T1, T2> action)
	{
		global::EventHandler.InvokableAction<T1, T2> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(obj, eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3>(EventID eventName, Action<T1, T2, T3> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3>(object obj, EventID eventName, Action<T1, T2, T3> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(obj, eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3, T4>(EventID eventName, Action<T1, T2, T3, T4> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3, T4> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3, T4>(object obj, EventID eventName, Action<T1, T2, T3, T4> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3, T4> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(obj, eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3, T4, T5>(object obj, EventID eventName, global::EventHandler.Action<T1, T2, T3, T4, T5> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3, T4, T5> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(obj, eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3, T4, T5>(EventID eventName, global::EventHandler.Action<T1, T2, T3, T4, T5> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3, T4, T5> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3, T4, T5, T6>(EventID eventName, Action<T1, T2, T3, T4, T5, T6> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}

	public static void RegisterEvent<T1, T2, T3, T4, T5, T6, T7>(EventID eventName, Action<T1, T2, T3, T4, T5, T6, T7> action)
	{
		global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6, T7> invokable = global::EventHandler.GetInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6, T7>>();
		invokable.Initialize(action);
		global::EventHandler.RegisterEvent(eventName, invokable);
	}
	private static void RegisterEvent(object obj, EventID eventName, global::EventHandler.InvokableActionBase invokableAction)
	{
		Dictionary<EventID, List<global::EventHandler.InvokableActionBase>> dictionary;
		if (!global::EventHandler.s_EventTable.TryGetValue(obj, out dictionary))
		{
			dictionary = new Dictionary<EventID, List<global::EventHandler.InvokableActionBase>>();
			global::EventHandler.s_EventTable.Add(obj, dictionary);
		}
		List<global::EventHandler.InvokableActionBase> list;
		if (dictionary.TryGetValue(eventName, out list))
		{
			list.Add(invokableAction);
		}
		else
		{
			list = new List<global::EventHandler.InvokableActionBase>();
			list.Add(invokableAction);
			dictionary.Add(eventName, list);
		}
	}
	#endregion

	#region EXECUTE
	public static void ExecuteEvent(EventID eventName)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction).Invoke();
			}
		}
	}
	public static void ExecuteEvent(object obj, EventID eventName)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction).Invoke();
			}
		}
	}
	public static void ExecuteEvent<T1>(EventID eventName, T1 arg1)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1>).Invoke(arg1);
			}
		}
	}

	public static void ExecuteEvent<T1>(object obj, EventID eventName, T1 arg1)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1>).Invoke(arg1);
			}
		}
	}

	public static void ExecuteEvent<T1, T2>(EventID eventName, T1 arg1, T2 arg2)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2>).Invoke(arg1, arg2);
			}
		}
	}

	public static void ExecuteEvent<T1, T2>(object obj, EventID eventName, T1 arg1, T2 arg2)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2>).Invoke(arg1, arg2);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3>(EventID eventName, T1 arg1, T2 arg2, T3 arg3)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3>).Invoke(arg1, arg2, arg3);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3>(object obj, EventID eventName, T1 arg1, T2 arg2, T3 arg3)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3>).Invoke(arg1, arg2, arg3);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3, T4>(EventID eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4>).Invoke(arg1, arg2, arg3, arg4);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3, T4>(object obj, EventID eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4>).Invoke(arg1, arg2, arg3, arg4);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3, T4, T5>(EventID eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>).Invoke(arg1, arg2, arg3, arg4, arg5);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3, T4, T5>(object obj, EventID eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>).Invoke(arg1, arg2, arg3, arg4, arg5);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3, T4, T5, T6>(object obj, EventID eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
			}
		}
	}

	public static void ExecuteEvent<T1, T2, T3, T4, T5, T6, T7>(EventID eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = actionList.Count - 1; i >= 0; i--)
			{
				(actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6, T7>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
		}
	}
	#endregion

	#region  UNREGISTER
	public static void UnregisterEvent(EventID eventName, Action action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction invokableAction = actionList[i] as global::EventHandler.InvokableAction;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	public static void UnregisterEvent(object obj, EventID eventName, Action action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction invokableAction = actionList[i] as global::EventHandler.InvokableAction;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}
	public static void UnregisterEvent<T1>(EventID eventName, Action<T1> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1>(object obj, EventID eventName, Action<T1> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2>(EventID eventName, Action<T1, T2> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2>(object obj, EventID eventName, Action<T1, T2> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3>(EventID eventName, Action<T1, T2, T3> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3>(object obj, EventID eventName, Action<T1, T2, T3> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3, T4>(EventID eventName, Action<T1, T2, T3, T4> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3, T4> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3, T4>(object obj, EventID eventName, Action<T1, T2, T3, T4> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3, T4> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3, T4, T5>(EventID eventName, global::EventHandler.Action<T1, T2, T3, T4, T5> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3, T4, T5> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3, T4, T5>(object obj, EventID eventName, global::EventHandler.Action<T1, T2, T3, T4, T5> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3, T4, T5> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3, T4, T5, T6>(object obj, EventID eventName, Action<T1, T2, T3, T4, T5, T6> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(obj, eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(obj, eventName, actionList);
		}
	}

	public static void UnregisterEvent<T1, T2, T3, T4, T5, T6, T7>(EventID eventName, Action<T1, T2, T3, T4, T5, T6, T7> action)
	{
		List<global::EventHandler.InvokableActionBase> actionList = global::EventHandler.GetActionList(eventName);
		if (actionList != null)
		{
			for (int i = 0; i < actionList.Count; i++)
			{
				global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6, T7> invokableAction = actionList[i] as global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6, T7>;
				if (invokableAction.IsAction(action))
				{
					global::EventHandler.ReturnInvokable<global::EventHandler.InvokableAction<T1, T2, T3, T4, T5, T6, T7>>(invokableAction);
					actionList.RemoveAt(i);
					break;
				}
			}
			global::EventHandler.CheckForEventRemoval(eventName, actionList);
		}
	}

	#endregion
	private static T GetInvokable<T>() where T : global::EventHandler.InvokableActionBase
	{
		Stack<global::EventHandler.InvokableActionBase> stack;
		if (global::EventHandler.m_GenericPool.TryGetValue(typeof(T), out stack) && stack.Count > 0)
		{
			return stack.Pop() as T;
		}
		return Activator.CreateInstance<T>();
	}
	private static void ReturnInvokable<T>(T invokableAction) where T : global::EventHandler.InvokableActionBase
	{
		Stack<global::EventHandler.InvokableActionBase> stack;
		if (!global::EventHandler.m_GenericPool.TryGetValue(typeof(T), out stack))
		{
			stack = new Stack<global::EventHandler.InvokableActionBase>();
			global::EventHandler.m_GenericPool.Add(typeof(T), stack);
		}
		stack.Push(invokableAction);
	}
	private static List<global::EventHandler.InvokableActionBase> GetActionList(EventID eventName)
	{
		List<global::EventHandler.InvokableActionBase> result;
		if (global::EventHandler.s_GlobalEventTable.TryGetValue(eventName, out result))
		{
			return result;
		}
		return null;
	}
	private static List<global::EventHandler.InvokableActionBase> GetActionList(object obj, EventID eventName)
	{
		Dictionary<EventID, List<global::EventHandler.InvokableActionBase>> dictionary;
		List<global::EventHandler.InvokableActionBase> result;
		if (global::EventHandler.s_EventTable.TryGetValue(obj, out dictionary) && dictionary.TryGetValue(eventName, out result))
		{
			return result;
		}
		return null;
	}
	private static void CheckForEventRemoval(EventID eventName, List<global::EventHandler.InvokableActionBase> actionList)
	{
		if (actionList.Count == 0)
		{
			global::EventHandler.s_GlobalEventTable.Remove(eventName);
		}
	}
	private static void CheckForEventRemoval(object obj, EventID eventName, List<global::EventHandler.InvokableActionBase> actionList)
	{
		Dictionary<EventID, List<global::EventHandler.InvokableActionBase>> dictionary;
		if (actionList.Count == 0 && global::EventHandler.s_EventTable.TryGetValue(obj, out dictionary))
		{
			dictionary.Remove(eventName);
			if (dictionary.Count == 0)
			{
				global::EventHandler.s_EventTable.Remove(obj);
			}
		}
	}
}
