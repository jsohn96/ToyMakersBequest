// Based on http://www.willrmiller.com/?p=87
using UnityEngine;
using System.Collections.Generic;

// Event System Usage:
/*

    // Create an event (note that it must inherit from GameEvent):
public class MyEvent : GameEvent {

	// Can add public/private properties, just like any other class

	public MyEvent () { // Can add params here
		// Any initialization you want
	}
}

// "Raise" an event -- notify any listeners that the event happened:
// ...
Events.G.Raise(new MyEvent());
// ...

// Listeners must listen to an event in order to know it happened.
// Best to put this in OnEnable:
void OnEnable () {
	// ...
	Events.G.AddListener<MyEvent>(MyEventHandler);
	// ...
}

// Make sure to stop listening in OnDisable or you'll leak!
void OnDisable () {
	// ...
	Events.G.RemoveListener<MyEvent>(MyEventHandler);
	// ...
}

// Some function that will get called when an event of type MyEvent gets "raised"
void MyEventHandler (MyEvent e) {
	// Do something with e
}

*/

public class GameEvent
{
}

public class Events {
	//_g = instanceInternal, G = instance on WillMiller
	static Events _g = null;
	public static Events G {
		get {
			if (_g == null) {
				_g = new Events();
			}
			return _g;
		}
	}

	public delegate void EventDelegate<T>(T e) where T : GameEvent;
	private delegate void EventDelegate(GameEvent e);

	readonly private Dictionary<System.Type, EventDelegate> _delegates = new Dictionary<System.Type, EventDelegate>();
	private Dictionary<System.Delegate, EventDelegate> _delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

	public void AddListener<T> (EventDelegate<T> del) where T : GameEvent {
		// Early-out if we've already registered this delegate
		if (_delegateLookup.ContainsKey(del))
			return;

		// Create a new non-generic delegate which calls our generic one.
		// This is the delegate we actually invoke.
		EventDelegate internalDelegate = e => del((T)e);
		_delegateLookup[del] = internalDelegate;

		EventDelegate tempDel;
		_delegates[typeof(T)] = _delegates.TryGetValue(typeof(T), out tempDel) ?
			tempDel += internalDelegate :
			internalDelegate;
	}

	public void RemoveListener<T> (EventDelegate<T> del) where T : GameEvent {
		EventDelegate internalDelegate;
		if (_delegateLookup.TryGetValue(del, out internalDelegate)) {
			EventDelegate tempDel;
			if (_delegates.TryGetValue(typeof(T), out tempDel)) {
				tempDel -= internalDelegate;
				if (tempDel == null) {
					_delegates.Remove(typeof(T));
				} else {
					_delegates[typeof(T)] = tempDel;
				}
			}
			_delegateLookup.Remove(del);
		}
	}

	public void Raise (GameEvent e) {
		//        #if UNITY_EDITOR
		//        Debug.Log(string.Format("RAISED EVENT: {0} at {1}s", e, Time.realtimeSinceStartup));
		//        #endif
		EventDelegate del;
		if (_delegates.TryGetValue(e.GetType(), out del)) {
			del.Invoke(e);
		}
	}
		
}