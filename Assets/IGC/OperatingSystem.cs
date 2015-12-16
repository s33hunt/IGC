using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//boot up
//operating system ties EVERYTHING together

//load files (they reach up to os)
//load users (from users file)
//load programs (from path: /bin & /usr/bin)

//on command execution
// if prog in programs, execute
// else if in cwd, load and execute

public class OperatingSystem : MonoBehaviour 
{
	public Dictionary<string, Executable> programs = new Dictionary<string, Executable>();

	Shell display;
	Transform userRegistry;
	FileSystem fileSystem;



	void Start()
	{
		display = transform.parent.Find ("display").GetComponent<Shell>();
		userRegistry = transform.Find ("user registry");
		fileSystem = transform.Find("file system").GetComponent<FileSystem>();

		display.os = this;

		fileSystem.Init();
		OnFileSystemReadyLoaded ();
	}

	void OnFileSystemReadyLoaded()
	{
        //...
        InitializeUserRegistry();
	}
	void InitializeUserRegistry()
	{
        //...
        LoadPrograms();
    }

	void LoadPrograms()
	{
		//grab exe's from bin only on system load
		Executable[] bin = fileSystem.transform.Find ("bin").GetComponentsInChildren<Executable> ();
		foreach (Executable p in bin) {programs.Add(p.name, p);}

		InitializeShell();
	}

	void InitializeShell()
	{
		display.Init();
	}

}
