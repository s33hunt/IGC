using UnityEngine;
using System.Collections;

public class Path
{
	public static string root = "/";

	public string[] segments;
	public string full, end, parent, extension;
	
	public Path(string path)
	{
		full = path;
		segments = Utils.SplitString (path, "/");
		end = segments [segments.Length - 1]; 
		parent = segments.Length > 1
			? segments [segments.Length - 2]
			: "/";
		int lastDot = end.LastIndexOf (".");
		extension = lastDot > 0 
			? end.Substring(lastDot, end.Length - lastDot)
				: root;
	}
}