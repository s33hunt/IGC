using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//detect keys
//+ + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + 
//+ this should be refactored into a method that accepts an input code and acts on it
//+ multiple input options (keypard, virtualkeyboard, phone keyboard, etc?
//+ check for input type on start and add the appropriate interface
//+ + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + 
public class InputHandler : MonoBehaviour
{
	public static InputHandler instance;

	public delegate void OnInput(InputEvent e);
	public OnInput onInput;
	public enum Mode { Keyboard }
	public Mode inputMode;
	public float repeatHeldKeyTime = 0.7f, cursorBlinkSpeed = 0.5f;
	public int keyRepeatsPerSecond = 10;
	public InputEvent lastInputEvent, currentInputEvent;
	float
		lastKeyDownTime = 0,
		lastKeyRepeatTime = 0,
		keyRepeatTime;
	public List<KeyCode> keysDown = new List<KeyCode>();
	KeyCode lastKey = KeyCode.None;
	KeyCode _currentKey = KeyCode.None;
	KeyCode currentKeyDown
	{
		get { return _currentKey; }
		set
		{
			_currentKey = value;
			lastKeyDownTime = Time.time;
		}
	}
	public bool shiftKeyDown
	{
		get { return (keysDown.Contains(KeyCode.LeftShift) || keysDown.Contains(KeyCode.RightShift)); }
	}
	public bool controlKeyDown
	{
		get { return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)); }
	}
	public bool KeyDown(KeyCode kc)
	{
		return keysDown.Contains(kc);
	}

	public class InputEvent
	{
		public enum Type { modifierKey, actionKey, text, mouseButton, scrollWheel }
		public enum State { down, hold, up, scroll }
		public Type type;
		public State state;
		public int value;
		public KeyCode keyCode;

		public InputEvent(Type t, State s, object data = null)
		{
			this.type = t;
			this.state = s;

			if(type == Type.scrollWheel) {
				this.value = (int)data;
			} else if (type == Type.modifierKey || type == Type.actionKey || type == Type.text) {
				this.keyCode = (KeyCode)data;
			}
		}
	}

	void Awake()
	{
		instance = this;
		keyRepeatTime = 1f / keyRepeatsPerSecond;
	}

	void HandleInput(InputEvent e)
	{
		currentInputEvent = e;
		if (!keysDown.Contains(e.keyCode)) { keysDown.Add(e.keyCode); }
		lastKey = currentKeyDown;
		currentKeyDown = e.keyCode;
		if (onInput != null) { onInput(e); }
	}
	
	void Update()
	{
		if(currentInputEvent != null) { 
			lastInputEvent = currentInputEvent;
			currentInputEvent = null;
		}
		
		//scroll with wheel
		if (Input.GetAxis("Mouse ScrollWheel") > 0) { HandleInput(new InputEvent(InputEvent.Type.scrollWheel, InputEvent.State.scroll, 1)); }
		else if (Input.GetAxis("Mouse ScrollWheel") < 0) { HandleInput(new InputEvent(InputEvent.Type.scrollWheel, InputEvent.State.scroll, -1)); }
		
		if (Input.anyKey)
		{
			foreach (KeyCode kc in InputCharacters.modifierKeys) {
				if (Input.GetKeyDown(kc)) {
					HandleInput(new InputEvent(InputEvent.Type.modifierKey, InputEvent.State.down, kc));
				}
			}
			foreach (KeyCode kc in InputCharacters.actionKeys) {
				if (Input.GetKeyDown(kc)) {
					HandleInput(new InputEvent(InputEvent.Type.actionKey, InputEvent.State.down, kc));
				}
			}
			foreach (KeyCode kc in InputCharacters.typedChars.Keys){
				if (Input.GetKeyDown(kc)){
					HandleInput(new InputEvent(InputEvent.Type.text, InputEvent.State.down, kc));
				}
			}

			//key repetition
			if (currentKeyDown != KeyCode.None && Time.time - lastKeyDownTime > repeatHeldKeyTime) { TypeHeldKey(); }
		}

		//update keys held list
		List<KeyCode> keysToRemove = new List<KeyCode>();
		foreach (KeyCode kc in keysDown) { if (Input.GetKeyUp(kc)) { keysToRemove.Add(kc); } }
		foreach (KeyCode kc in keysToRemove) { keysDown.Remove(kc); }
		//reset key up
		if (Input.GetKeyUp(currentKeyDown)) { currentKeyDown = KeyCode.None; }
	}

	void TypeHeldKey()
	{
		if (Time.time - lastKeyRepeatTime > keyRepeatTime)
		{
			HandleInput(lastInputEvent);
			lastKeyRepeatTime = Time.time;
		}
	}
}
