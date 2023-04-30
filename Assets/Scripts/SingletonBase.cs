using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
	public static T instance;

	protected void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning($"There is already an instance of {typeof(T).Name}.");
			Destroy(this);
			return;
		}

		instance = (T)this;
	}
}