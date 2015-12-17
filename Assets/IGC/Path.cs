using UnityEngine;
using System.Collections;

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

		Debug.Log(IsPath(""));
		Debug.Log(IsPath("/bin"));
		Debug.Log(IsPath("/fart.exe"));
		Debug.Log(IsPath("\"hemper/asdf\"/skdfj"));
		Debug.Log(IsPath("\"hemper/asdf\""));
		Debug.Log(IsPath("\"hemper///asdf\""));
		Debug.Log("---------------------");

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