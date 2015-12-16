using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParsedCommandPhrase
{
	public string rawText;
	public string[] argv;
	public bool error {get{return !string.IsNullOrEmpty(errorMessage);}}
	public string errorMessage = "";
	
	
	public ParsedCommandPhrase(string raw)//feed in actual text
	{
		rawText = raw; 
		
		List<string> args = new List<string>(){""};
		bool gettingPhrase = false;
		string phraseWrapper = "";
		
		//build argv
		for(int i=0; i<raw.Length; i++){
			string c = raw[i].ToString();
			
			//detect phrases
			if((c == "\"" || c == "'") && !gettingPhrase){
				phraseWrapper = c; 
				gettingPhrase = true;
				if(!string.IsNullOrEmpty(args[args.Count-1])){args.Add("");}
			}
			
			//add characters
			if(gettingPhrase){
				args[args.Count-1] += c;
				if(c == phraseWrapper && args[args.Count-1].Length > 1){
					args.Add("");
					gettingPhrase = false;
				}
			}else{
				if(c == " "){
					if(!string.IsNullOrEmpty(args[args.Count-1])){args.Add("");}
				}else{
					args[args.Count-1] += c;
				}
			}
		}
		//check for errors
		if(gettingPhrase){
			errorMessage = "incomplete quote phrase";
			return;//stop here if error
		}
		
		argv = args.ToArray ();
	}
}
