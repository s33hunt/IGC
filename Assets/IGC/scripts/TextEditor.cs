using UnityEngine;
using System.Collections;

public class TextEditor : MonoBehaviour
{
	TextDisplay display;


	void Start()
	{
		display = GetComponent<TextDisplay>();
		InputHandler.instance.onInput += ProcessInput;
	}

	void ProcessInput(InputHandler.InputEvent e)
	{
		if (e.type == InputHandler.InputEvent.Type.text)
		{
			display.TypeChar(e.character);
		}
	}
}
