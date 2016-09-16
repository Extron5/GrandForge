using UnityEngine;
using System.Collections;

public class Axe : MonoBehaviour {

    public Rigidbody rb;
    public float maxDist = 1;
    public float weight = 1;
    public float gap;
    public LayerMask mask;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Debug.DrawRay(transform.position, Vector3.down, Color.green, gap + maxDist);
	}

    void OnCollisionEnter(Collision c)
    {
        //Fix this
        //c.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        //Debug.Log(c.gameObject.transform.tag.ToString());
        if (c.gameObject.transform.tag == "Wood")
        {
            Vector3 vel = c.relativeVelocity.normalized;//(c.gameObject.transform.parent.gameObject.transform.parent.GetComponent<Rigidbody>().velocity - rb.velocity);//
            //Debug.Log(vel);
            Vector3 vel2 = vel;
            vel = vel.normalized;
            float yVel = Vector3.Dot(c.gameObject.transform.up, vel);
            float zVel = Vector3.Dot(c.gameObject.transform.forward, vel);
            float xVel = Vector3.Dot(c.gameObject.transform.right, vel);
            yVel = Mathf.Abs(yVel);
            zVel = Mathf.Abs(zVel);
            xVel = Mathf.Abs(xVel);
            float power = 1.5f;
            if (c.relativeVelocity.magnitude > power)
            {
                c.gameObject.GetComponent<BreakStump>().Chop();
            }
        }
    }
}
