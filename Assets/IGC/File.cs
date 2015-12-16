﻿using UnityEngine;
using System.Collections;

//needs perms

public class File : MonoBehaviour 
{
	public bool isDirectory = false;
	public Path path;
    public bool hidden { get { return path.end[0] == '.'; } }
	[HideInInspector] public FileSystem fileSystem;
	[HideInInspector] public Executable exe;

	void Awake()
	{
		path = new Path (GetPath());
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