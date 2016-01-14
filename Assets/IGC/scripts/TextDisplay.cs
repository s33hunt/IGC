using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
	public Color textColor = Color.blue;
	public bool cursorEnabled = true;
	public float cursorBlinkSpeed = 0.5f;
	public int
		height = 10,
		width = 10,
		maxTextLines = 1000;
	string _currentLine = "";
	int _scrollOffset = 0;
	float lastCursorBlink = 0;
	Transform cursor;
	Renderer cursorRenderer;
	TextMesh textDisplay;
	Vector2 _cursorPosition = Vector2.zero;
	List<string> displayLines = new List<string>();
	int displayLength {
		get { return displayLines.Count < height ? displayLines.Count : height; }
	}
	public int currentLineNumber{
		get { return scrollOffset + (int)cursorPosition.y; }
	}
	public string currentLine {
		get { return displayLines[currentLineNumber]; }
	}
	public Vector2 cursorPosition {
		get { return _cursorPosition; }
		set {

			_cursorPosition.y = Mathf.Clamp(value.y, 0, displayLines.Count - 1); //gotta do y first so currentLine can have the right number
			_cursorPosition.x = Mathf.Clamp(value.x, 0, currentLine.Length - 1);

			print(_cursorPosition+"    :    " + currentLine);

			UpdateCursorPos();
		}
	}
	int scrollOffset {
		get { return _scrollOffset; }
		set { _scrollOffset = Mathf.Clamp(value, 0, Mathf.Min(displayLines.Count - displayLength, maxTextLines - displayLength)); }
	}

	public void CursorLeft() { cursorPosition = new Vector2(cursorPosition.x - 1, cursorPosition.y); }
	public void CursorRight() { cursorPosition = new Vector2(cursorPosition.x + 1, cursorPosition.y); }
	public void CursorUp() { cursorPosition = new Vector2(cursorPosition.x, cursorPosition.y - 1); }
	public void CursorDown() { cursorPosition = new Vector2(cursorPosition.x, cursorPosition.y + 1); }
	void UpdateCursorPos()
	{
		cursor.localPosition = new Vector3(
			cursorPosition.x * cursor.localScale.x,
			cursorPosition.y * -cursor.localScale.y,
			0
		);
	}

	public void Init()
	{
		textDisplay = GetComponent<TextMesh>();

		textDisplay.characterSize = 0.05f;
		float
			cwidth = textDisplay.fontSize * .0025f,   //fs:w -> 400:1
			cheight = cwidth * 2;                   //w:h-> .5 : 1

		textDisplay.color = textColor;

		cursor = transform.Find("cursor");
		cursor.localScale = new Vector3(cwidth, cheight, 1);
		cursorRenderer = cursor.GetComponentInChildren<Renderer>();
		cursorRenderer.material.color = textColor;
		
		displayLines.Add("");
	}

	
	//test stuff
	public int lineNumber = 0, insertNum = 0;
	public string insertString = "XXXXX xxx XXX XX";
	string testString =
			"aa  aa a a aaaaaa a a aaa a a a\n" +
			"bbbb b b bbb bb bbb b b bb b bbb b\n" +
			"aa  aa a a aaaaaa a a aaa a a a\n" +
			"\n" +
			"cccc c c ccc cc c ccc  ccc ccc c\n" +
			"bbbb b b bbb bb bbb b b bb b bbb b\n" +
			"\n" +
			"cccc c c ccc cc c ccc  ccc ccc c\n" +
			"aa  aa a a aaaaaa a a aaa a a a\n" +
			"bbbb b b bbb bb bbb b b bb b bbb b\n" +
			"cccc c c ccc cc c ccc  ccc ccc c";
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow)) { CursorUp(); }
		if (Input.GetKeyDown(KeyCode.DownArrow)) { CursorDown(); }
		if (Input.GetKeyDown(KeyCode.LeftArrow)) { CursorLeft(); }
		if (Input.GetKeyDown(KeyCode.RightArrow)) { CursorRight(); }
		//if (Input.GetKeyDown(KeyCode.Space)){EditLine(lineNumber, insertString, insertNum);}
	}
	//end test stuff




	void Start() {
		Init();
		EditLine(0, testString, 0);
	}
	
	string[] FormatIntoLines(string unformatted)
	{
		List<string> lines = new List<string>();

		foreach (string l in unformatted.Split(new char[1] { '\n' })) {
			foreach (string formattedLine in FormatLine(l)) {
				lines.Add(formattedLine);
			}
		}
		return lines.ToArray();
	}

	string FormatText(string unformatted)
	{
		return string.Join("\n", FormatIntoLines(unformatted));
	}

	string[] FormatLine(string unformatted)
	{
		int wordPointer = 0;
		List<string>
			words = new List<string>(),
			lines = new List<string>();

		foreach (string s in unformatted.Split(new char[1] { ' ' })) { words.Add(s); }

		while (wordPointer < words.Count)
		{
			if (lines.Count >= 9999) { break; }//just in case of infinite loops

			lines.Add("");

			if (words[wordPointer].Length >= width)
			{
				lines[lines.Count - 1] += words[wordPointer++];
				continue;
			}
			while (lines[lines.Count - 1].Length + words[wordPointer].Length + 1 <= width)
			{
				lines[lines.Count - 1] += words[wordPointer] + " ";
				if (++wordPointer >= words.Count) { break; }
			}
		}

		return lines.ToArray();
	}


	public void EditLine(int lineNumber, string newText, int insertPosition)
	{
		string newline = displayLines[lineNumber].Insert(insertPosition, newText);
		print(newline);
        string[] targetLines = FormatIntoLines(newline);
		
		for(int i = targetLines.Length-1; i >= 0; i--)
		{
			//print(targetLines[i] + "\n" + i + "\n" + lineNumber);
			displayLines.Insert(lineNumber, targetLines[i]);
		}

		UpdateDisplay();
	}

	public void TypeChar(string c)
	{
		EditLine(currentLineNumber, c, (int)cursorPosition.x);
		CursorRight();
	}


	void UpdateDisplay()
	{
		textDisplay.text = string.Join("\n", displayLines.ToArray());
	}
}





















/*
void PrintChar(KeyCode kc)
{
	if (kc != KeyCode.None)
	{
		string c;
		if (InputHandler.instance.shiftKeyDown && InputCharacters.shiftedSpecialChars.ContainsKey(kc))
		{
			c = InputCharacters.shiftedSpecialChars[kc];
		}
		else {
			//check for alphabet char keycode names and shift held
			c = InputHandler.instance.shiftKeyDown //kc.ToString ().Length == 1 && 
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

			print(lines.Length + ":" + (int)cursorXY.y);

			//if at right end
			if (cursorOffset == 0) { lines[(int)cursorXY.y] += c; }
			//if in middle
			//else if (cursorPosition != 0) { rawEditText = commandLineText.Substring(0, cursorPosition) + c + commandLineText.Substring(cursorPosition, cursorOffset); }
			//if at left end
			//else { commandLineText = c + commandLineText; }
		}

	}
}
*/

/*
void Print(string s, bool includeCmdTxt = true)
{
	if (includeCmdTxt) { fullText += FormatDisplayString(promptText + commandLineText) + "\n"; }
	fullText += FormatDisplayString(s) + "\n";
	commandLineText = "";
	PrintBuffer();
}

void PrintBuffer()
{

	int startIndex = displayLength >= height && scrollOffset > 0
		? Mathf.Max(0, lineCount - displayLength - scrollOffset)
		: lineCount - displayLength;

	string output = "";
	if (CLIMode)
	{
		output = fullText + FormatDisplayString(promptText + commandLineText);
	}
	else if (editMode)
	{
		output = FormatDisplayString(string.Join("\n", lines));
	}

	lines = output.Split(new char[1] { '\n' });
	lineCount = lines.Length;

	print(fullText + "\n\n" + FormatDisplayString(string.Join("\n", lines)));

	IEnumerable<string> displayLines = lines.Skip(startIndex).Take(displayLength);

	output = string.Join("\n", displayLines.ToArray());

	textDisplay.text = output;

	UpdateCursorPos();
}*/
