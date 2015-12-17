using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSystem : MonoBehaviour
{
    public Dictionary<string, File> files = new Dictionary<string, File>();
	[HideInInspector] public OperatingSystem os;

	public void Init ()
    {
	    //traverse all files from / down
		foreach(File f in transform.GetComponentsInChildren<File>())
		{
			f.fileSystem = this;
			files.Add(f.path.full, f);
			//print("loading file: " + f.path.full);
		}
	}

	public File GetFile(Path p){return _GetFile(p.full);}
	public File GetFile(string fullpath) { return _GetFile(fullpath); }
	File _GetFile(string fullpath)
	{
		return files.ContainsKey(fullpath) ? files[fullpath] : null;
	}
}
