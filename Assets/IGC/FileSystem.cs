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
		//this is a special init process, normal file creation is different
		foreach(File f in transform.GetComponentsInChildren<File>())
		{
			f.fileSystem = this;
			f.os = os;
			f.Init();
			files.Add(f.path.full, f);
		}
	}

	//public void CreateFil(string path)

	
	public bool FileExists(Path p) { return files.ContainsKey(p.full); ; }
	public bool FileExists(string fullpath) {
		Path p = new Path(fullpath, os);
		if (!p.isPath) { return false; }
		return files.ContainsKey(p.full); ;
	}
	
	File _GetFile(string fullpath) { return files.ContainsKey(fullpath) ? files[fullpath] : null; }
	public File GetFile(Path p){return _GetFile(p.full);}
	public File GetFile(string fullpath) {
		Path p = new Path(fullpath, os);
		if (!p.isPath) { return null; }
		return _GetFile(p.full);
	}

	public bool IsDir(Path p)
	{
		File f = GetFile(p);
		if (f != null) {
			return f.isDirectory;
		}
		return false;
	}
}
