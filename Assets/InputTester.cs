using UnityEngine;
using System.Collections;

public class InputTester : MonoBehaviour
{
	string fullOutput = "";


	void Start () { InputHandler.instance.onInput += ProcessInput; }
	
	void OnGUI()
	{
		GUILayout.BeginHorizontal();

		GUILayout.BeginVertical();
		GUILayout.Label(InputHandler.instance.currentInputEvent == null ? "..." : InputHandler.instance.currentInputEvent.type.ToString() + ":" + InputHandler.instance.currentInputEvent.state.ToString());
		GUILayout.Label("-------------------");
		GUILayout.Label(InputHandler.instance.lastInputEvent == null ? "..." : InputHandler.instance.lastInputEvent.type.ToString() + ":" + InputHandler.instance.lastInputEvent.state.ToString());
		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		GUILayout.Label(Utils.StringifyArray<KeyCode>(InputHandler.instance.keysDown.ToArray(), "\n"));
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();

		GUILayout.Label(fullOutput);
	}

	void ProcessInput(InputHandler.InputEvent e)
	{
		print(e);
		KeyCode kc = e.keyCode;

		if (kc != KeyCode.None)
		{
			fullOutput += kc.ToString();
		}
	}
}
