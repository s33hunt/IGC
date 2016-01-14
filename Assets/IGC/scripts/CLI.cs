using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CLI : MonoBehaviour {

	[HideInInspector]
	public OperatingSystem os;
	public string promptText = "[>] ";
	
	
	string
		commandLineText = "",
		CLISession;
	int _historyPointer = 0;
		

	List<string> history = new List<string>();
	
	
	int historyPointer
	{
		get { return _historyPointer; }
		set { _historyPointer = Mathf.Clamp(value, 0, history.Count - 1); }
	}








	public void Init()
	{
		history.Add("");
		
		
		

		
	}
}
