using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ParseCourse : MonoBehaviour {
	// !!! REMEMBER THAT ALL RACECOURSE FILES ARE IN INCHES, AND MUST BE CONVERTED TO METERS
	List<Vector3> checkpointPos;

	// Add Checkpoints to map
	void Start () {
		string path = EditorUtility.OpenFilePanel("Choose a level", Directory.GetCurrentDirectory() + @"\Courses", "png");
		if (path == null)
			return;
		string[] coordinates = File.ReadAllLines (path);
		checkpointPos = new List<Vector3> (coordinates.Length);

		/* Read coordinates */

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
