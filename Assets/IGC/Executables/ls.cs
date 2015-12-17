using UnityEngine;
using System.Collections;

public class ls : Executable
{
	public override void Action()
	{
		if(argv.Length == 2)
		{
			if (os.fileSystem.FileExists(argv[1]))
			{
				rd.standardOut = Utils.StringifyArray<File>(os.fileSystem.GetFile(argv[1]).children);
			}
			else
			{
				rd.standardOut = "file not found";
			}
		}
		else if(argv.Length == 1)
		{
			rd.standardOut = Utils.StringifyArray<File>(os.fileSystem.GetFile(os.env.cwdPath).children);
		}
	}
}
