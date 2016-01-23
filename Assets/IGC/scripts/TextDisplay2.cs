﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TextDisplay2 : MonoBehaviour
{
	public int width = 30, height = 10;	
	TextMesh textDisplay;
	public int currentLineNumber = 0;
	string[] flowedLines;
	public string 
		rawText, 
		flowedText;
	int 
		_cursorOffsetL = 0, 
		_cursorOffsetR = 0,
		_cursorOffsetTop = 0, 
		_cursorOffsetBottom = 0;
	public int lineCount { get { return flowedLines.Length; } }
	public int cursorOffsetL {
		get { return _cursorOffsetL; }
		set {
			_cursorOffsetL = Mathf.Clamp(value, 0, rawText.Length);
			_cursorOffsetR = rawText.Length - _cursorOffsetL;
	}}
	public int cursorOffsetR {
		get { return _cursorOffsetR; }
		set {
			_cursorOffsetR = Mathf.Clamp(value, 0, rawText.Length);
			_cursorOffsetL = rawText.Length - _cursorOffsetR;
	}}
	//IEnumerable<string> displayLines = lines.Skip(startIndex).Take(displayLength);


	void Update()
	{
		textDisplay.text = flowedText;
	}

	public void Init()
	{
		cursorOffsetL = 0;
		textDisplay = GetComponent<TextMesh>();
		flowedText = FlowText();
	}
	
	public void CursorLeft() { cursorOffsetL++; }
	public void CursorRight() { cursorOffsetL--; }
	public void CursorUp()
	{
		//get text above cursor
		string 
			aboveLine = "",
			currentLine = "",
			aboveText = rawText.Substring(0, cursorOffsetR);
		//break into lines
		string[] linesAbove = SplitLines(FlowText(aboveText));

		currentLineNumber = linesAbove.Length - 1;

		if(linesAbove.Length > 1) {
			aboveLine = linesAbove[linesAbove.Length - 2];
			currentLine = linesAbove[linesAbove.Length - 1];
		} else { return; }

		cursorOffsetL += aboveLine.Length;

		print(TextDisplay2.instance.cursorOffsetL + ":" + TextDisplay2.instance.cursorOffsetR + "\n" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR] + ":" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR]);
	}
	public void CursorDown()
	{
		string below = rawText.Substring(cursorOffsetR, cursorOffsetL);
		//...
	}
	
	public void InsertText(string text)
	{
		rawText =
			rawText.Substring(0, cursorOffsetR) + 
			text +
			rawText.Substring(cursorOffsetR, cursorOffsetL);//(cursorOffsetL == 0 ? "" : ... )
		cursorOffsetL = cursorOffsetL;
		flowedText = FlowText();
	}

	public void RemoveText(int count)
	{
		rawText = rawText.Remove(cursorOffsetR, Mathf.Min(count, cursorOffsetL));
		cursorOffsetR = cursorOffsetR;
		flowedText = FlowText();
	}

	public string FlowText(string raw = null)
	{
		bool snippet = raw == null;
		raw = raw ?? rawText;
		string[] lines = SplitLines(raw);
		List<string> flowed = new List<string>();
		foreach (string l in lines) { flowed.Add(FlowLine(l)); }
		if (!snippet) {flowedLines = flowed.ToArray();}
		return string.Join("\n", flowed.ToArray());
	}

	string[] SplitLines (string raw){return raw.Split(new char[1] { '\n' });}

	string FlowLine(string raw)
	{
		int wordPointer = 0;
		List<string>
			words = new List<string>(),
			lines = new List<string>();

		foreach (string s in raw.Split(new char[1] { ' ' })) { words.Add(s); }

		while (wordPointer < words.Count) {
			if (lines.Count >= 9999) { break; }//just in case of infinite loops

			lines.Add("");

			if (words[wordPointer].Length >= width) {
				lines[lines.Count - 1] += words[wordPointer++];
				continue;
			}
			while (lines[lines.Count - 1].Length + words[wordPointer].Length + 1 <= width) {
				lines[lines.Count - 1] += words[wordPointer] + " ";
				if (++wordPointer >= words.Count) { break; }
			}
		}
		return string.Join("\n", lines.ToArray());
	}











	//<testing>
	public static TextDisplay2 instance;
	string testString =
		"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam lectus. Sed sit amet ipsum mauris. Maecenas congue ligula ac quam viverra nec consectetur ante hendrerit. Donec et mollis dolor. " +
		"\n" +
		"Praesent et diam eget libero egestas mattis sit amet vitae augue. Nam tincidunt congue enim, ut porta lorem lacinia consectetur. Donec ut libero sed arcu vehicula ultricies a non tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut gravida lorem. " +
		"\n" +
		"\n" +
		"Ut turpis felis, pulvinar a semper sed, adipiscing id dolor. Pellentesque auctor nisi id magna consequat sagittis. Curabitur dapibus enim sit amet elit pharetra tincidunt feugiat nisl imperdiet. " +
		"\n" +
		"\n" +
		"Ut convallis libero in urna ultrices accumsan. Donec sed odio eros. Donec viverra mi quis quam pulvinar at malesuada arcu rhoncus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In rutrum accumsan ultricies. Mauris vitae nisi at sem facilisis semper ac in est.";
	void Start()
	{
		instance = this;
		rawText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam lectus. Sed sit amet ipsum mauris. Maecenas congue ligula ac quam viverra nec consectetur ante hendrerit. Donec et mollis dolor. " +
		"\n" +
		"Praesent et diam eget libero egestas mattis sit amet vitae augue. Nam tincidunt congue enim, ut porta lorem lacinia consectetur. Donec ut libero sed arcu vehicula ultricies a non tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut gravida lorem.";
		Init();
	}
	//</testing>
}




//cursorOffsetL = 3;
//InsertText("X");
//cursorOffsetL = 3;
//RemoveText(1);

//cursorOffsetR = 2;
//InsertText("XxX");

//InsertText("X");
//cursorOffsetL = 0;
//RemoveText(1);

//CursorLeft(); CursorLeft(); CursorLeft(); CursorLeft(); CursorLeft();
//InsertText("W");