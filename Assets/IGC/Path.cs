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
		end = segments.Length > 0 ? segments [segments.Length - 1] : root; 
		parent = segments.Length > 1
			? segments [segments.Length - 2]
			: root;
		int lastDot = end.LastIndexOf (".");
		extension = lastDot > 0 
			? end.Substring(lastDot, end.Length - lastDot)
			: "";
	}
}