﻿using UnityEngine;
using System.Collections;

public class ls : Executable
{
	public override void Action()
	{
		rd.standardOut = file.path.full;
	}
}
