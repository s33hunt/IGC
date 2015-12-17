using UnityEngine;
using System.Collections;

//handle bad path
//handle ending slashes

public class Path
{
	public static string root = "/";

	public string[] segments;
	public string full, end, parent, extension, relative = "";
	public bool isPath, isRelative = false;
	
	public static bool IsValid(string path)
	{
		bool 
			multiSlashes = path.IndexOf("//") >= 0,
			containsQuotes = path.IndexOf('\"') >= 0,
			quoteWrapped = Utils.StringIsQuoteWrapped(path);

		if ((containsQuotes && !quoteWrapped) || multiSlashes) { return false; }

		return true;	
	}
	
	public Path(string path, OperatingSystem os)
	{
		isPath = IsValid(path);
		if (!isPath) { return; }

		//remove quotes
		full = path.Replace("\"", "");
		//remove ending slash
		if (full.LastIndexOf("/") == full.Length - 1) { full = full.Remove(full.Length - 1, 1); }
		//check for relative...
		if (full.Length > 0)
		{
			if (full[0] != '/') {
				isRelative = true;
				relative = full;
				full = os.env.cwd == root 
					? root + relative 
					: os.env.cwd + "/" + relative; 
			}
		}
		//check for ..
		if(full.IndexOf("..") >= 0)
		{
			Debug.Log("got dem dots");
		}
	
		
		segments = Utils.SplitString (full, "/");
		end = segments.Length > 0 ? segments [segments.Length - 1] : root; 
		parent = segments.Length > 1
			? segments [segments.Length - 2]
			: root;
		int lastDot = end.LastIndexOf (".");
		extension = lastDot > 0 
			? end.Substring(lastDot, end.Length - lastDot)
			: "";
	}

	public string ToString()
	{
		return "isPath: " + isPath +
			"\nisRelative: " + isRelative +
			(isRelative ? "\nrelative: " + relative : "") +
			"\nfull: " + full +
			"\nparent: " + parent +
			"\nend: " + end +
			"\nextension: " + extension;
	}
}