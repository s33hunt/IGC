using UnityEngine;
using System.Collections;

//handle bad path

public class Path
{
	public static string root = "/";

	public string[] segments;
	public string full, end, parent, extension;
	
	public static bool IsPath(string path)
	{
		bool 
			multiSlashes = path.IndexOf("//") >= 0,
			containsQuotes = path.IndexOf('\"') >= 0,
			quoteWrapped = Utils.StringIsQuoteWrapped(path);

		if ((containsQuotes && !quoteWrapped) || multiSlashes)
		{
			return false;
		}

		return true;	
	}
	
	public Path(string path)
	{
		if (!IsPath(path)) { return; }
		
		full = path.Replace("\"", "");
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