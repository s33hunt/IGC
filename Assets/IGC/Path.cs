using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Path
{
	public static string root = "/";

	public string[] segments;
	public string full = "", end = "", parent = null, parentFull = "", extension = "", relative = "";
	public bool isValid = true, exists = false, isRelative = false;
	
	public static bool IsValid(string path)
	{
		bool 
			empty = string.IsNullOrEmpty(path),
			multiSlashes = path.IndexOf("//") >= 0,
			containsQuotes = path.IndexOf('\"') >= 0,
			quoteWrapped = Utils.StringIsQuoteWrapped(path);

		if ((containsQuotes && !quoteWrapped) || multiSlashes || empty) { return false; }

		return true;	
	}
	
	public Path(string path, OperatingSystem os)
	{
		//quick set for root
		if(path == "/" || path == "\"/\""){full = end = "/";return;}
		
		//valid path check
		isValid = IsValid(path);
		if (!isValid) { return; }
		
		//remove quotes
		full = path.Replace("\"", "");
		
		//remove ending slash
		if (full.LastIndexOf("/") == full.Length - 1) { full = full.Remove(full.Length - 1, 1); }
		
		//check for relative...
		if (full[0] != '/') {
			isRelative = true;
			relative = full;
			//check for ..
			if (relative.IndexOf("..") >= 0)
			{
				List<string> newSegs = new List<string>();//the new, constructed path from cwd + relative
				string[] 
					cwdSegs = Utils.SplitString(os.env.cwd, "/"),
					relSegs = Utils.SplitString(relative, "/");

				for (int i = 0; i < cwdSegs.Length; i++)
				{
					newSegs.Add(cwdSegs[i]);
				}

				for (int i = 0; i < relSegs.Length; i++)
				{
					full = Utils.StringifyArray<string>(newSegs.ToArray(), "/", "/");//update full string at top of cycle

					if (relSegs[i] == "..") {
						if(newSegs.Count > 0) {
							newSegs.RemoveAt(newSegs.Count - 1);
						} else {
							//...
						}
					} else {
						File f = os.fileSystem.GetFile(full);
						if(f != null) {
							if (f.containsFile(relSegs[i])) {
								newSegs.Add(relSegs[i]);
							} else {
								isValid = false;
							}
						}
						else
						{
							isValid = false;
						}	
					}
				}

				full = Utils.StringifyArray<string>(newSegs.ToArray(), "/", "/");//update full string at finish
			}
			else
			{
				full = os.env.cwd == root
					? root + relative //if you're in root dir
					: os.env.cwd + "/" + relative; //else
			}
			
		}
		
	
		
		segments = Utils.SplitString (full, "/");
		end = segments.Length > 0 ? segments [segments.Length - 1] : root;
		for (int i = 0; i < segments.Length - 1; i++) { parentFull += "/" + segments[i]; }
		if (string.IsNullOrEmpty(parentFull)) { parentFull = "/"; }
		parent = segments.Length > 1
			? segments [segments.Length - 2]
			: root;
		int lastDot = end.LastIndexOf (".");
		extension = lastDot > 0 
			? end.Substring(lastDot, end.Length - lastDot)
			: "";

		//check for valid again here...
		exists = os.fileSystem.files.ContainsKey(full);
	}

	public string ToString()
	{
		return "isPath: " + isValid +
			"\nisRelative: " + isRelative +
			(isRelative ? "\nrelative: " + relative : "") +
			"\nfull: " + full +
			"\nparent: " + parent +
			"\nparent full: " + parentFull +
			"\nend: " + end +
			"\nextension: " + extension;
	}
}