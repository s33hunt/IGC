using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//needs to be able to take multiple 1 letter flags

[RequireComponent(typeof(File))]
public class Executable : MonoBehaviour
{
	public string[] acceptedFlags;
	[HideInInspector] public string[] argv;//this is just for typing convenience
	[HideInInspector] public ParsedCommandPhrase pc;
	[HideInInspector] public Dictionary<string, string> flags;
	[HideInInspector] public File file;
	protected OperatingSystem os { get { return file.fileSystem.os; } }
	protected ReturnData rd;

	public ReturnData Execute(ParsedCommandPhrase pc)
	{
		this.pc = pc;
		argv = pc.argv;
		ParseFlags();

		rd = new ReturnData();

		Action ();

		return rd;
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

	public class ReturnData
	{
		public bool persist = false;
		public string standardOut = "";
	}

	public virtual void Action()
	{
		rd.standardOut = "base prog: [argv " + argv.Length + "] [flags " + flags.Count + "]";
	}
}
