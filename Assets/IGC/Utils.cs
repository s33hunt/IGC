using UnityEngine;
using System.Collections;

public class Utils 
{

	public static string StringifyArray<T>(T[] array, string seporator = ", ", string start = "", string end = "")
	{
		if(array.Length == 0){return "";}
		string output = start;
		for (int i=0; i<array.Length; i++) {output += array[i].ToString()+seporator;}
		return output.Remove(
					Mathf.Max (0, output.Length - seporator.Length),
					Mathf.Min (output.Length, seporator.Length)
				) + end;
	}

	public static string[] SplitString(string str, string sep)
	{
		return str.Split (new string[1]{sep}, System.StringSplitOptions.RemoveEmptyEntries);
	}
}