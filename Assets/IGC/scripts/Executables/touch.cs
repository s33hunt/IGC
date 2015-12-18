using UnityEngine;
using System.Collections;

public class touch : Executable
{
	public override void Action()
	{
		if (argv.Length == 2)
		{
			Path p = new Path(argv[1], os);
			os.fileSystem.CreateFile(p.parentFull, p.end);
		}
	}
}