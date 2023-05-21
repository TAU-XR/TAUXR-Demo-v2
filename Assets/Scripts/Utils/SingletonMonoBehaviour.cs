using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject globals = GameObject.Find("Globals");
				if (null == globals)
					globals = new GameObject("Globals");
				instance = globals.AddComponent<T>();
			}
			return instance;
		}
	}
	protected virtual void Awake()
	{
		if (null != instance)
		{
			Debug.LogWarning($"More than one instance of {typeof(T).Name} exist. Please make sure this doesn't happen - this is a singleton.");
			return;
		}
		instance = this as T;
	}
}
