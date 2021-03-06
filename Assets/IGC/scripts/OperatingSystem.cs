﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//boot up
//operating system ties EVERYTHING together

//load files (they reach up to os)
//load users (from users file)
//load programs (from path: /bin & /usr/bin)
//init shell

//on command execution
// if prog in programs, execute
// else if in cwd, load and execute

public class OperatingSystem : MonoBehaviour 
{
	public Dictionary<string, Executable> programs = new Dictionary<string, Executable>();

	[HideInInspector] Shell shell;
	[HideInInspector] Transform userRegistry;
	[HideInInspector] public FileSystem fileSystem;
	public EnvironmentVariables env;

	public class EnvironmentVariables
	{
		public File cwdir;
		public Path _cwdPath;
		public Path cwdPath
		{
			get { return _cwdPath; }
			set
			{
				if (value.isValid)
				{
				 	if (os.fileSystem.IsDir(value))
					{
						_cwdPath = value;
						cwdir = os.fileSystem.GetFile(value);
					}
				}
			}
		}
		public string cwd { get { return cwdPath.full; } }
		OperatingSystem os;
		public EnvironmentVariables(OperatingSystem os) {
			this.os = os;
			cwdPath = new Path("/", os);
		}
	}


	void Start()
	{
		shell = transform.parent.Find ("display").GetComponent<Shell>();
		userRegistry = transform.Find ("user registry");
		fileSystem = transform.Find("file system").GetComponent<FileSystem>();
		shell.os = fileSystem.os = this;
		
		BootUp ();
	}

	void BootUp()
	{
		//filsystem _______________________________________________________

		fileSystem.Init();

		//setup env _______________________________________________________

		env = new EnvironmentVariables(this);

		//user registry ___________________________________________________

		//...

		//programs ________________________________________________________

		Executable[] bin = fileSystem.transform.Find ("bin").GetComponentsInChildren<Executable> ();//only grab exe's from bin on system load
		foreach (Executable p in bin) {programs.Add(p.name, p);}

		//shell ___________________________________________________________

		shell.Init();
	}

}
