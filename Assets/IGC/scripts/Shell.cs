using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour 
{
	[HideInInspector] public OperatingSystem os;
	public Color textColor = Color.blue;
	public enum TextMode {CLI, TextEdit}
	public TextMode textMode = TextMode.CLI;
	public string promptText = "[>] ";
	public float repeatHeldKeyTime = 0.7f, cursorBlinkSpeed = 0.5f;
	public int 
		height = 10,
		width = 10,
		keyRepeatsPerSecond = 10,
		maxTextLines = 100;

	string[] lines;
	bool cursorEnabled = true;
	Renderer cursorRenderer;
	float
		lastKeyDownTime = 0,
		lastKeyRepeatTime = 0,
		keyRepeatTime,
		lastCursorBlink = 0;
	string
		fullText = "",
		commandLineText = "",
		CLISession;
	int
		lineCount = 0,
		_historyPointer = 0,
		_scrollOffset = 0,
		_cursorOffset = 0,
		_cursorOffsetVertical = 0,
		_cursorPosition = 0;
	Transform cursor;
	TextMesh textDisplay;
	List<KeyCode> keysDown = new List<KeyCode> ();
	KeyCode _lastKey;
	List<string> history = new List<string> ();
	KeyCode lastKeyDown {
		get { return _lastKey; }
		set {
			_lastKey = value;
			lastKeyDownTime = Time.time;
		}
	}
	bool editMode {
		get { return textMode == TextMode.TextEdit; }
		set { if (value) EnterTextEditMode(); } }
	bool CLIMode {
		get { return textMode == TextMode.CLI; }
		set { if (value) EnterCLIMode(); } }
	bool shiftKeyDown {
		get { return (keysDown.Contains(KeyCode.LeftShift) || keysDown.Contains(KeyCode.RightShift)); }
	}
	bool controlKeyDown {
		get { return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)); }
	}
	int displayLength {
		get { return lineCount < height ? lineCount : height; }
	}
	int cursorOffset {
		get {return _cursorOffset;}
		set {

			if (textMode == TextMode.TextEdit) {
				_cursorOffset = Mathf.Clamp(value, 0, width);
			} else if (textMode == TextMode.CLI) {
				_cursorOffset = Mathf.Clamp(value, 0, commandLineText.Length);
			}
		}
	}
	int cursorOffsetVertical
	{
		get { return _cursorOffsetVertical; }
		set { _cursorOffsetVertical = Mathf.Clamp(value, 0, lineCount); }
	}
	int cursorPosition {
		get {return commandLineText.Length - cursorOffset;}
		set {_cursorPosition = Mathf.Clamp(value, 0, commandLineText.Length);}
	}
	int scrollOffset {
		get { return _scrollOffset;}
		set { _scrollOffset = Mathf.Clamp (value, 0, Mathf.Min(lineCount - displayLength, maxTextLines - displayLength)); }
	}
	int historyPointer {
		get { return _historyPointer; }
		set { _historyPointer = Mathf.Clamp(value, 0, history.Count - 1);}
	}
	

	public void Init()
	{
		history.Add ("");
		textDisplay = GetComponent<TextMesh> ();
		keyRepeatTime = 1f / keyRepeatsPerSecond;
		textDisplay.text = promptText;
		textDisplay.characterSize = 0.05f;
		float
			cwidth = textDisplay.fontSize * .0025f,   //fs:w -> 400:1
			cheight = cwidth * 2;                   //w:h-> .5 : 1
		cursor = transform.Find("cursor");
		cursor.localScale = new Vector3(cwidth, cheight, 1);
		cursorRenderer = cursor.GetComponentInChildren<Renderer>();
		textDisplay.color = textColor;
		cursorRenderer.material.color = textColor;

		UpdateCursorPos();
	}

	void Update ()
	{
		//scroll with wheel
		if (Input.GetAxis ("Mouse ScrollWheel") > 0){scrollOffset++;ProcessInput(KeyCode.None);}
		else if(Input.GetAxis ("Mouse ScrollWheel") < 0){scrollOffset--;ProcessInput(KeyCode.None);}
		//temp testing mode switcher
		if (Input.GetKeyDown(KeyCode.Escape)) { if (CLIMode) { editMode = true; } else { CLIMode = true; } }

		//detect keys
		//+ + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + 
		//+ this should be refactored into a method that accepts an input code and acts on it
		//+ multiple input options (keypard, virtualkeyboard, phone keyboard, etc?
		//+ check for input type on start and add the appropriate interface
		//+ + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + 
		if (Input.anyKey){
			foreach (KeyCode kc in InputCharacters.modifierKeys) {
				if(Input.GetKeyDown(kc)){
					if(!keysDown.Contains(kc)){keysDown.Add(kc);}
					lastKeyDown = kc;
				}
			}

			foreach (KeyCode kc in InputCharacters.actionKeys) {
				if(Input.GetKeyDown(kc)){
					if(!keysDown.Contains(kc)){keysDown.Add(kc);}
					ProcessInput (kc);
					lastKeyDown = kc;
				}
			}

			foreach (KeyCode kc in InputCharacters.typedChars.Keys) {
				if(Input.GetKeyDown(kc)){
					if(!keysDown.Contains(kc)){keysDown.Add(kc);}
					ProcessInput (kc);
					lastKeyDown = kc;
				}
			}
			
			//key repetition
			if (lastKeyDown != KeyCode.None && Time.time - lastKeyDownTime > repeatHeldKeyTime) {RepeatHeldKey();}

			if(CLIMode)
			{
				scrollOffset = 0;//jump to typing of scrolled
				ShowCursor();
			}
		}

		//update keys held list
		List<KeyCode> keysToRemove = new List<KeyCode>();
		foreach (KeyCode kc in keysDown) {if (Input.GetKeyUp(kc)) {keysToRemove.Add(kc);}}
		foreach (KeyCode kc in keysToRemove) {keysDown.Remove(kc);}
		//reset key up
		if (Input.GetKeyUp (lastKeyDown)) {lastKeyDown = KeyCode.None;}

		if(scrollOffset > 0 && CLIMode) {
			HideCursor();
		} else {
			if(Time.time > lastCursorBlink + cursorBlinkSpeed) {
				lastCursorBlink = Time.time;
				if (cursorEnabled) { HideCursor(); } else { ShowCursor(); }
			}
		}

		//print(shiftKeyDown);
		//print(Utils.StringifyArray<KeyCode>(keysDown.ToArray()));
	}

	public void EnterTextEditMode()
	{
		textMode = TextMode.TextEdit;
		CLISession = fullText;
		fullText = "";
		PrintBuffer();
	}
	public void EnterCLIMode()
	{
		textMode = TextMode.CLI;
		fullText = CLISession;
		CLISession = "";
		PrintBuffer();
	}

	Vector2 cursorXY
	{
		get {
			int y = cursorOffsetVertical - scrollOffset;
			return new Vector2(cursorOffset, y == 0 ? 1 : 0);
		}
	}

	void ShowCursor(){ if (!cursorEnabled) cursorRenderer.enabled = cursorEnabled = true; }
	void HideCursor(){ if (cursorEnabled) cursorRenderer.enabled = cursorEnabled = false; }
	void UpdateCursorPos()
	{
		if (editMode) {
			cursor.localPosition = new Vector3(
				(commandLineText.Length - cursorOffset + promptText.Length) * cursor.localScale.x,
				cursor.localScale.y * -Mathf.Clamp((displayLength - (cursorOffsetVertical - scrollOffset)), 0, displayLength-1),
				0
			);
			print(cursorXY);
		} else if (CLIMode) {
			cursor.localPosition = new Vector3(
				(commandLineText.Length - cursorOffset + promptText.Length) * cursor.localScale.x,
				cursor.localScale.y * (displayLength == 0 ? 0 : 1 - displayLength),
				0
			);
		}
		
	}

	void RepeatHeldKey()
	{
		if (Time.time - lastKeyRepeatTime > keyRepeatTime) {
			ProcessInput (lastKeyDown);
			lastKeyRepeatTime = Time.time;
		}
	}

	void ProcessInput(KeyCode kc)
	{
		if (kc != KeyCode.None) {
			if (InputCharacters.actionKeys.Contains (kc)) {
				KeyAction (kc);
			} else if(!InputCharacters.modifierKeys.Contains(kc)){
				if (kc == KeyCode.Return) {
					ReturnKey ();
				} else if (kc == KeyCode.Space && commandLineText [commandLineText.Length - 1] == ' ') {
				} else {
					PrintChar (kc);
				}
			}
		}

		//if(historyPointer == history.Count-1){history [historyPointer] = commandLineText;}

		PrintBuffer ();
	}

	void PrintChar(KeyCode kc)
	{
		if(kc != KeyCode.None){
			string c;
			if (shiftKeyDown && InputCharacters.shiftedSpecialChars.ContainsKey(kc)) {
				c = InputCharacters.shiftedSpecialChars[kc];
			} else {
				//check for alphabet char keycode names and shift held
				c = shiftKeyDown //kc.ToString ().Length == 1 && 
					? InputCharacters.typedChars[kc].ToUpper()
					: InputCharacters.typedChars[kc];
			}

			if (CLIMode)
			{
				//if at right end
				if (cursorPosition == commandLineText.Length) { commandLineText += c; }
				//if in middle
				else if (cursorPosition != 0) { commandLineText = commandLineText.Substring(0, cursorPosition) + c + commandLineText.Substring(cursorPosition, cursorOffset); }
				//if at left end
				else { commandLineText = c + commandLineText; }
			}
			else if (editMode)
			{
				fullText += c;
			}
			
		}
	}

	void KeyAction(KeyCode kc)
	{
		//print ("action key: " + kc);
		if(kc == KeyCode.LeftArrow){LeftArrow();}
		if(kc == KeyCode.RightArrow){RightArrow();}
		if(kc == KeyCode.Backspace){BackSpace();}
		if(kc == KeyCode.Delete){Delete();}
		if(kc == KeyCode.UpArrow){UpArrow();}
		if(kc == KeyCode.DownArrow){DownArrow();}
	}

	string FormatDisplayString(string unformatted)
	{
		//int i=0, 
		int wordPointer = 0;
		List<string> 
			words = new List<string> (),
			lines = new List<string> ();
		
		foreach(string s in unformatted.Split(new char[1]{' '}/*, System.StringSplitOptions.RemoveEmptyEntries*/)){words.Add(s);}

		while(wordPointer < words.Count){
			if(lines.Count >= 999){break;}//just in case if infinite loops

			lines.Add ("");

			if(words[wordPointer].Length >= width){
				lines[lines.Count - 1] += words[wordPointer++];
				continue;
			}
			while(lines[lines.Count-1].Length + words[wordPointer].Length + 1 <= width){
				lines[lines.Count-1] += words[wordPointer]+" ";
				if(++wordPointer >= words.Count){break;}
			}
		}

		return string.Join ("\n", lines.ToArray ());
	}

	void Print(string s, bool includeCmdTxt = true)
	{
		if (includeCmdTxt) { fullText += FormatDisplayString(promptText + commandLineText) + "\n"; }
		fullText += FormatDisplayString (s)+"\n";
		commandLineText = "";
		PrintBuffer ();
	}
	
	void PrintBuffer()
	{
		string output = fullText + (CLIMode ? FormatDisplayString(promptText + commandLineText) : "");
		lines = output.Split(new char[1]{'\n'});
		lineCount = lines.Length;
		
		int startIndex = displayLength >= height && scrollOffset > 0
			? Mathf.Max(0, lineCount - displayLength - scrollOffset)
			: lineCount - displayLength;
		
		IEnumerable<string> displayLines = lines.Skip(startIndex).Take(displayLength);
		
		output = string.Join ("\n", displayLines.ToArray ());
		
		textDisplay.text = output;

		UpdateCursorPos();
	}
	
	#region KeyActions
	void LeftArrow()
	{
		cursorOffset++;
		UpdateCursorPos();
	}
	void RightArrow()
	{
		cursorOffset --;
		UpdateCursorPos();
	}
	void UpArrow()
	{
		if (editMode) {
			cursorOffsetVertical++;
			if(cursorOffsetVertical - scrollOffset > displayLength) {
				scrollOffset++;
			}
		} else if (CLIMode) {
			if (controlKeyDown) {
				scrollOffset++;
			} else {
				historyPointer--;
				commandLineText = history[historyPointer];
				cursorOffset = 0;
			}
		}
		UpdateCursorPos();
	}
	void DownArrow()
	{
		if(editMode) {
			cursorOffsetVertical--;
			if (cursorOffsetVertical - scrollOffset < 1) {
				scrollOffset--;
			}
		} else if (CLIMode) {
			if (controlKeyDown) {
				scrollOffset--;
			} else {
				historyPointer++;
				commandLineText = history[historyPointer];
				cursorOffset = 0;
			}
		}
		UpdateCursorPos();
	}
	void ReturnKey()
	{
        if (!string.IsNullOrEmpty (commandLineText)) {
			history[history.Count-1] = commandLineText;
			history.Add("");
		}
        
        fullText += FormatDisplayString (CLIMode ? promptText + commandLineText : "")+"\n";

		if (CLIMode) {
			ParsedCommandPhrase pc = new ParsedCommandPhrase(commandLineText);
			Executable.ReturnData rd = new Executable.ReturnData();
			bool isCommand = false;
			if (os != null) {
				if (!pc.error) {
					isCommand = os.programs.ContainsKey(pc.argv[0]);
					if (isCommand) { rd = os.programs[pc.argv[0]].Execute(pc); }
				}
			}

			commandLineText = "";
			historyPointer = history.Count - 1;

			if (pc.error) { Print("input error: " + pc.errorMessage); }
			else if (!isCommand && !string.IsNullOrEmpty(pc.argv[0])) { Print("command not found: " + pc.argv[0]); }
			if (!string.IsNullOrEmpty(rd.standardOut)) { Print(rd.standardOut, false); }
		}
		
		cursorOffset = 0;
	}
	void Delete()
	{
		if (cursorPosition < commandLineText.Length) {
			commandLineText = commandLineText.Remove(cursorPosition, 1);
			cursorOffset--;
		}
	}
	void BackSpace()
	{
		//if at right end
		if (cursorPosition == commandLineText.Length && commandLineText.Length > 0) {
			commandLineText = commandLineText.Substring(0, commandLineText.Length-1);
			
		//if in middle
		} else if (cursorPosition > 0) {
			commandLineText = commandLineText.Remove(cursorPosition-1, 1);
		}
	}
	#endregion
}