using UnityEngine;
using System.Collections;

public class pwd : Executable
{
	public override void Action()
	{
		rd.standardOut = os.env.cwd;
	}
}
