using UnityEngine;
using System.Collections;

public class cd : Executable
{
	public override void Action()
	{
		if(argv.Length == 2) {
			rd.standardOut = "" + Path.IsPath(argv[1]) + " " + argv[1];
		}
		else
		{
			rd.standardOut = "[malformed command]\nformat: cd <path>";
		}
	}
}
