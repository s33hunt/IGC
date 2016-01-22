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
			if (e.keyCode == KeyCode.UpArrow) { display.CursorUp(); }
			if (e.keyCode == KeyCode.DownArrow) { display.CursorDown(); }
			if (e.keyCode == KeyCode.LeftArrow) { display.CursorLeft(); }
			if (e.keyCode == KeyCode.RightArrow) { display.CursorRight(); }
		}
		else if (e.type == InputHandler.InputEvent.Type.text)
		{
			display.InsertText(e.character);
		}
	}
}
