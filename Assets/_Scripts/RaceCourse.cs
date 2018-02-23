using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public delegate void TouchedTarget(CheckpointState targetckpt);

public class RaceCourse : MonoBehaviour {
	/* No Course */
	public Material noCourseSkybox;
	public GameObject noCourseLightsOff;
    public GameObject noCourseLightOn;
	public bool noCourse;

	/* RaceCourse */
	// !!! REMEMBER THAT ALL RACECOURSE FILES ARE IN INCHES, AND MUST BE CONVERTED TO METERS
	List<Vector3> checkpointPos;
	public List<CheckpointState> checkpoints;
	public CheckpointState checkpointPrefab;

    /* Race Game */
    public GameObject player;
    public PlayerScript playerScript;
	GameObject targetCheckpoint;
	GameObject nextCheckpoint;
	GameObject prevCheckpoint = null;
	public List<GameObject> arrows;
    private int arrowi;
    public Material checkpointGreyed;
	public Material checkpointNext;
	public Material checkpointPassed;
    public int checkpointNum = 0;
    public LineRenderer lineRenderer;


	// Add Checkpoints to map
	void Start () {
		string path = EditorUtility.OpenFilePanel("Choose a level", Directory.GetCurrentDirectory() + @"\Courses", "");
		Debug.Log (path);
		if (path.Length == 0) {	// Change skybox if no course is chosen
			NoCourseEffects();
			return;
		}
		string[] coordinates = File.ReadAllLines (path);
		if (coordinates.Length <= 1) {
			NoCourseEffects ();
			return;
		}
        // Initialize all objects (after checking if track is valid)
		checkpointPos = new List<Vector3> (coordinates.Length);
        arrowi = 0;

        /* Read coordinates */
        for (int i = 0; i < coordinates.Length; i++) {
			//Debug.Log (coordinates[i]);
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

        /* Connect positions with line */
        lineRenderer.positionCount = checkpointPos.Count;
        lineRenderer.SetPositions(checkpointPos.ToArray());

		/* Create checkpoints (player is placed at first coordinate) */
		checkpoints = new List<CheckpointState> ();
		for (int cpi = 1; cpi < checkpointPos.Count; cpi++) {
			var ckpt = CreateCheckpoint (checkpointPos[cpi]);
			checkpoints.Add (ckpt);
		}

        /* Place player at first pos */
        player.transform.position = checkpointPos[0];
        Debug.Log(player.transform.position);
        player.transform.LookAt(checkpoints[checkpointNum].transform); // Rotate towards first checkpoint
        
        /* Set first target checkpoint to noticable color */
		checkpoints [checkpointNum].SetMaterial (checkpointNext);
        checkpoints[checkpointNum].SetOnTriggerEnterCallback(touchedTarget);

        setArrowAtTargetCheckpoint();



        /********* START GAME **********/
        /* Don't start countdown until thumbs up */
	}

    void touchedTarget(CheckpointState targetckpt)
    {
        Debug.Log("TOUCHED SOMETHING????");
        /* Disable touched checkpoint using animation */   //checkpoints[checkpointNum].enabled = false;
        targetckpt.SetMaterial(checkpointPassed);
        targetckpt.StartShrinking();

        /* Set up next checkpoint */
        checkpointNum += 1;
        if (checkpointNum < checkpoints.Count) {
            checkpoints[checkpointNum].SetMaterial(checkpointNext);
            checkpoints[checkpointNum].SetOnTriggerEnterCallback(touchedTarget);
            setArrowAtTargetCheckpoint();
        }
        else { // Last Checkpoint
            //checkpointNum += 1;
            playerScript.EndGame();
        }

    }
    bool setArrowAtTargetCheckpoint()
    {
        if (checkpointNum < checkpoints.Count-1) {
            /* Put arrow inside checkpoint */
            arrows[arrowi].transform.position = checkpoints[checkpointNum].transform.position;
            arrows[arrowi].transform.LookAt(checkpoints[checkpointNum + 1].transform);
            arrowi = (arrowi + 1) % arrows.Count;
            return true;
        }
        else {
            return false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Create a grey checkpoint
	CheckpointState CreateCheckpoint (Vector3 ckptPos) {
		CheckpointState ckpt = Instantiate (checkpointPrefab, ckptPos, new Quaternion ());
		// make ckpts grey as default
		ckpt.SetMaterial(checkpointGreyed);
		return ckpt;
	}
	void NoCourseEffects () {
		RenderSettings.skybox = noCourseSkybox;
        noCourseLightsOff.SetActive(false);
        noCourseLightOn.SetActive(true);
		noCourse = true;
	}

	void DebugLogList (IEnumerable list) {
		foreach (var item in list) {
			Debug.Log (item);
		}
	}

    public Vector3 GetNextCheckpointPosition()
    {
        if (checkpointNum < checkpoints.Count) {
            return checkpoints[checkpointNum].transform.position;
        }
        else {
            return Vector3.negativeInfinity;
        }
    }
}
