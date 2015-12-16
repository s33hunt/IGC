using UnityEngine;
using System.Collections;

public class ls : Executable
{
	public override void Action()
	{
		print(file.path.full);
	}
}
