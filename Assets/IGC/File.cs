using UnityEngine;
using System.Collections;

//needs perms
//needs legal name-char check

public class File : MonoBehaviour 
{
	public bool isDirectory = false;
	public Path path;
    public bool hidden { get { return path.end[0] == '.'; } }
	[HideInInspector] public FileSystem fileSystem;
	[HideInInspector] public OperatingSystem os;
	[HideInInspector] public Executable exe;
	File[] _children;
	public int childCount {
		get {
			if (!isDirectory) { return 0; }
			return transform.childCount;
		}
	}
	public File[] children {
		get {
			if (!isDirectory){_children = new File[0];}

			if(_children == null || transform.childCount != _children.Length) {
				_children = new File[transform.childCount];
				int i = 0;
				foreach (Transform t in transform) {
					File f = t.GetComponent<File>();
					if (f != null) {
						_children[i++] = f;
					}
				}
			}
			return _children;
		}
	}

	public void Init()
	{
		path = new Path (GetPath(), os);
		exe = GetComponent<Executable>();
		if(exe != null) { exe.file = this; }
		
        /*print (
			path.full+"\n"+
			Utils.StringifyArray<string> (path.segments)+"\n"+
			path.end+"\n"+
			path.parent+"\n"+
			path.extension);*/
    }

	string GetPath()
	{
		if(name == "file system"){return "/";}
		
		string output = "/"+name;
		Transform up = transform.parent;

		if(up == null){return "broken";}
		int safety = 100;

		while(safety-- > 0){
			if(up.name == "file system"){break;}
			output = "/"+up.name+output;
			up = up.parent;
		}
		return output;
	}
}