using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FloatEvent : UnityEvent<float> {}
public class IntEvent : UnityEvent<int> {}
public class BoolEvent : UnityEvent<bool> {}
public class StringEvent : UnityEvent<string> {}
public class Event : UnityEvent {}

public class EventAggregator : MonoBehaviour {
	
	public static IntEvent OnDamageEvent = new IntEvent();
	

	void Start () 
	{
		OnDamageEvent.AddListener(Damage);
		OnDamageEvent.Invoke(50);
	}
	

	void Damage(int damage)
	{
		Debug.Log("damage = " + damage);
	}
	
	
}
