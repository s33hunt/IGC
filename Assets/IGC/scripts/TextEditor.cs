using UnityEngine;
using System.Collections;

public class TextEditor : MonoBehaviour
{
	TextDisplay2 display;


	void Start()
	{
		display = GetComponent<TextDisplay2>();
		InputHandler.instance.onInput += ProcessInput;
	}

	void ProcessInput(InputHandler.InputEvent e)
	{
		if (e.type == InputHandler.InputEvent.Type.actionKey)
		{
			if (e.keyCode == KeyCode.UpArrow) { display.CursorUp(); print(TextDisplay2.instance.cursorOffsetL + ":" + TextDisplay2.instance.cursorOffsetR + "\n" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR] + ":" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR]); }
			if (e.keyCode == KeyCode.DownArrow) { display.CursorDown(); print(TextDisplay2.instance.cursorOffsetL + ":" + TextDisplay2.instance.cursorOffsetR + "\n" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR] + ":" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR]); }
			if (e.keyCode == KeyCode.LeftArrow) { display.CursorLeft(); print(TextDisplay2.instance.cursorOffsetL + ":" + TextDisplay2.instance.cursorOffsetR + "\n" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR] + ":" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR]); }
			if (e.keyCode == KeyCode.RightArrow) { display.CursorRight(); print(TextDisplay2.instance.cursorOffsetL + ":" + TextDisplay2.instance.cursorOffsetR + "\n" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR] + ":" + TextDisplay2.instance.rawText[TextDisplay2.instance.cursorOffsetR]); }
		}
		else if (e.type == InputHandler.InputEvent.Type.text)
		{
			display.InsertText(e.character);
		}
	}
}
