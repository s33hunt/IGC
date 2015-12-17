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

	public void CreateFile(string targetDir, string filename, bool isDir = false)
	{
		File f = GetFile(targetDir);
		if(f == null) { return; }
		if (!f.isDirectory) { return; }
		if (f.containsFile(filename)) { return; }

		GameObject go = new GameObject(filename);
		go.transform.parent = f.transform;
		go.transform.position = Vector3.zero;
		File newfile = go.AddComponent<File>();
		newfile.isDirectory = isDir;
		newfile.fileSystem = this;
		newfile.os = os;
		newfile.Init();
		files.Add(newfile.path.full, newfile);
	}

	public void DeleteFile(Path target)
	{
		File f = GetFile(target);
		if (f == null) { return; }

		files.Remove(f.path.full);
		Destroy(f.gameObject);
	}


	public bool FileExists(Path p) { return files.ContainsKey(p.full); ; }
	public bool FileExists(string fullpath) {
		Path p = new Path(fullpath, os);
		if (!p.isValid) { return false; }
		return files.ContainsKey(p.full); ;
	}
	
	File _GetFile(string fullpath) { return files.ContainsKey(fullpath) ? files[fullpath] : null; }
	public File GetFile(Path p){return _GetFile(p.full);}
	public File GetFile(string fullpath) {
		Path p = new Path(fullpath, os);
		if (!p.isValid) { return null; }
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
