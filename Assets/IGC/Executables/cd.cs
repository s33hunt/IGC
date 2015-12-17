using UnityEngine;
using System.Collections;

public class cd : Executable
{
	public override void Action()
	{
		
		rd.standardOut = Utils.StringifyArray(argv);
	}
}
