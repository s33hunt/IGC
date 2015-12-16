using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//needs to be able to take multiple 1 letter flags

public class Executable : MonoBehaviour
{
	public string[] acceptedFlags;
	[HideInInspector] string[] argv;//this is just for typing convenience
	[HideInInspector] public ParsedCommandPhrase pc;
	[HideInInspector] public Dictionary<string, string> flags;

	public void Execute(ParsedCommandPhrase pc)
	{
		this.pc = pc;
		argv = pc.argv;
		ParseFlags();

		Action ();
	}
	
	void ParseFlags()
	{
		flags = new Dictionary<string, string>();
		foreach(string flag in acceptedFlags){
			foreach(string arg in argv){
				if(arg == "-"+flag){
					string f = arg.Remove(0,1);
					//here is where do 1 letter flag grouping
					flags.Add(f, "");
					break;
				}
			}
		}
	}

	public virtual void Action()
	{
		print(new System.Diagnostics.StackTrace());
		print(argv);
        print(flags);
        print ("base prog: [argv " + argv.Length + "] [flags " + flags.Count + "]");
	}
}
