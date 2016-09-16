using UnityEngine;
using System.Collections;

public class Pusher : MonoBehaviour {

    public Vector3 StartPos;
    public Vector3 EndPos;
    public Vector3 GoalPos;

	// Use this for initialization
	void Start () {
        StartPos = transform.position;
        GoalPos = StartPos;
        EndPos = transform.position+ new Vector3(0,0,0.25f);
    }
	
	// Update is called once per frame
	void Update () {
	    if(transform.position != GoalPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, GoalPos, 0.001f);
        }
        else
        {
            if(GoalPos == EndPos)
            {
                GoalPos = StartPos;
            }
        }
	}
}
