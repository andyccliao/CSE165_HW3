using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ParseCourse : MonoBehaviour {
	/* No Course */
	public Material noCourseSkybox;
	public Light noCourseLightOff;
	private bool noCourse;

	/* RaceCourse */
	// !!! REMEMBER THAT ALL RACECOURSE FILES ARE IN INCHES, AND MUST BE CONVERTED TO METERS
	List<Vector3> checkpointPos;
	List<GameObject> checkpoints;
	public GameObject checkpointPrefab;

	/* Race Game */
	GameObject targetCheckpoint;
	GameObject nextCheckpoint;
	GameObject arrow;

	// Add Checkpoints to map
	void Start () {
		string path = EditorUtility.OpenFilePanel("Choose a level", Directory.GetCurrentDirectory() + @"\Courses", "");
		Debug.Log (path);
		if (path.Length == 0) {	// Change skybox if no course is chosen
			NoCourseEffects();
			return;
		}
		string[] coordinates = File.ReadAllLines (path);
		checkpointPos = new List<Vector3> (coordinates.Length); 

		/* Read coordinates */
		for (int i = 0; i < coordinates.Length; i++) {
			Debug.Log (coordinates[i]);
			// For each checkpoint's coordinates
			string[] splitCoord = coordinates [i].Split(' ');
			float[] parsedCoord = new float[splitCoord.Length];

			for (int xyz = 0; xyz < splitCoord.Length; xyz++) {
				// parse x, y, z
				if (!float.TryParse(splitCoord[xyz], out parsedCoord[xyz])) {
					// if parse failed
					NoCourseEffects();
					return;
				}
			}
			// Add parsed coordinates to list
			checkpointPos.Add (new Vector3(parsedCoord[0], parsedCoord[1], parsedCoord[2]));
		}

		/* Convert from inches to meters */
		for (int pos = 0; pos < checkpointPos.Count; pos++) {
			checkpointPos[pos] *= 0.0254f;
		}

		/* Create checkpoints */
		checkpoints = new List<GameObject> ();
		for (int cpi = 0; cpi < checkpointPos.Count; cpi++) {
			Debug.Log ("WAT");
			var ckpt = Instantiate (checkpointPrefab, checkpointPos [cpi], new Quaternion ());
			checkpoints.Add (ckpt);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void NoCourseEffects () {
		RenderSettings.skybox = noCourseSkybox;
		noCourseLightOff.enabled = false;
		noCourse = true;
	}

	void DebugLogList (IEnumerable list) {
		foreach (var item in list) {
			Debug.Log (item);
		}
	}
}
