using UnityEngine;
using System.Collections;

public class rm : Executable
{
	public override void Action()
	{
		if (argv.Length == 2)
		{
			Path p = new Path(argv[1], os);
			os.fileSystem.DeleteFile(p);
		}
	}
}