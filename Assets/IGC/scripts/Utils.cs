using UnityEngine;
using System.Collections;

public class Utils 
{

	public static string StringifyArray<T>(T[] array, string seporator = ", ", string start = "", string end = "")
	{
		if(array.Length == 0){return start+end;}
		string output = "";
		for (int i=0; i<array.Length; i++) {output += array[i].ToString()+seporator;}
		output = output.Remove(output.Length - seporator.Length, seporator.Length);
		return start + output + end;
	}

	public static string[] SplitString(string str, string sep)
	{
		return str.Split (new string[1]{sep}, System.StringSplitOptions.RemoveEmptyEntries);
	}

	public static bool StringIsQuoteWrapped(string str)
	{
		char[] cc = new char[] { '\"', '\'' };
		foreach(char c in cc) { //check for single AND double quotes
			bool a = str.IndexOf(c) >= 0;
			if (a) {
				var ex = SplitString(str, c.ToString());
				if (ex.Length > 1) {
					return false;
				}
				return true;
			}
		}
		return false;
	}
}