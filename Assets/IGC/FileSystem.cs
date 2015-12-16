using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSystem : MonoBehaviour
{
    public Dictionary<string, File> files = new Dictionary<string, File>();
	

	public void Init ()
    {
	    //traverse all files from / down
		foreach(File f in transform.GetComponentsInChildren<File>())
		{
			files.Add(f.path.full, f);
			print("loading file: " + f.path.full);
		}
	}
}
