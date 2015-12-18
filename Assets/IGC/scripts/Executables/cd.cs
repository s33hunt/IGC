using UnityEngine;
using System.Collections;

public class cd : Executable
{
	public override void Action()
	{
		if(argv.Length == 2) {
			Path p = new Path(argv[1], os);
			if (p.exists)
			{
				if (os.fileSystem.IsDir(p))
				{
					os.env.cwdPath = p;
					return;
				}
			}

			rd.standardOut = "invalid path"; 
		} else {
			rd.standardOut = "[malformed command]\nformat: cd <path>";
		}
	}
}
