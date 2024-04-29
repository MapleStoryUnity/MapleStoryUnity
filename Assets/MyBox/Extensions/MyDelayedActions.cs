﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyBox
{
	public static class MyDelayedActions
	{
		/// <summary>
		/// Invoke Action on Delay
		/// </summary>
		public static Coroutine DelayedAction(float waitSeconds, Action action, bool unscaled = false)
		{
			return DelayedActionCoroutine(waitSeconds, action, unscaled).StartCoroutine();
		}
		
		
		/// <summary>
		/// Invoke Action next frame
		/// </summary>
		public static void DelayedAction(Action action)
		{
			Coroutine().StartCoroutine();

			IEnumerator Coroutine()
			{
				yield return null;
				action?.Invoke();
			}
		}
		
		/// <summary>
		/// Invoke Action on Delay
		/// </summary>
		public static Coroutine DelayedAction(this MonoBehaviour invoker, float waitSeconds, Action action, bool unscaled = false)
		{
			return invoker.StartCoroutine(DelayedActionCoroutine(waitSeconds, action, unscaled));
		}

		/// <summary>
		/// Invoke Action next frame
		/// </summary>
		public static Coroutine DelayedAction(this MonoBehaviour invoker, Action action)
		{
			return invoker.StartCoroutine(Coroutine());
			
			IEnumerator Coroutine()
			{
				yield return null;
				action?.Invoke();
			}
		}


		/// <summary>
		/// Set GO as selected next frame (EventSystem.current.SetSelectedGameObject)
		/// </summary>
		public static IEnumerator DelayedUiSelection(GameObject objectToSelect)
		{
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(objectToSelect);
		}

		/// <summary>
		/// Set GO as selected next frame (EventSystem.current.SetSelectedGameObject)
		/// </summary>
		public static Coroutine DelayedUiSelection(this MonoBehaviour invoker, GameObject objectToSelect)
		{
			return invoker.StartCoroutine(DelayedUiSelection(objectToSelect));
		}
		
		
		private static IEnumerator DelayedActionCoroutine(float waitSeconds, Action action, bool unscaled = false)
		{
			if (unscaled) yield return new WaitForSecondsRealtime(waitSeconds);
			else yield return new WaitForSeconds(waitSeconds);

			if (action != null) action.Invoke();
		}
	}
}