using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour {

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
        if (c.gameObject.transform.tag == "Metal")
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
            switch(c.gameObject.transform.tag)
            {
                case "Hammer":
                    power = 1.5f;
                    break;
                case "HammerSmall":
                    power = 0.01f;
                    break;
            }
            if (c.relativeVelocity.magnitude > power)
            {
                
                if (c.collider.gameObject.transform.parent != null)
                {
                    
                    SwordController sc = c.collider.gameObject.transform.parent.GetComponent<SwordController>();
                    if (sc != null)
                    {

                        if (xVel > zVel && xVel > yVel)
                        {
                            float sign = Mathf.Sign(c.collider.gameObject.transform.parent.transform.parent.transform.localRotation.y) * Mathf.Sign(c.collider.gameObject.transform.parent.transform.parent.transform.localRotation.x);
                            gap = (sc.pointGap[(int)c.collider.gameObject.GetComponent<Point>().number.x] + 0.5f) / 20;
                            //RaycastHit hit;
                            sc.movePoint((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, 0, -weight/2));
                            sc.Stretch((int)c.collider.gameObject.GetComponent<Point>().number.x, sc.moveAmount * (weight * 10));
                            /*
                            if (Physics.Raycast(transform.position, Vector3.down, out hit, gap + maxDist, mask.value))
                            {
                                if (hit.collider.tag == "Anvil")
                                {
                                    //Debug.Log("Hit Anvil");
                                    sc.movePoint((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, 0, -weight));
                                    //sc.Stretch((int)c.collider.gameObject.GetComponent<Point>().number.x, weight / 100);
                                }
                                else
                                {
                                    //Debug.Log("Hit No Anvil");
                                    sc.movePointDisplaceHeight((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, sign * -weight/100, 0));
                                }
                            }
                            else
                            {
                                //Debug.Log("No Hit");
                                sc.movePointDisplaceHeight((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, sign * -weight/100, 0));
                            }
                            
                            //Debug.Log(sign);
                            */

                        }
                        else if (zVel > yVel && zVel > xVel)
                        {
                            gap = (sc.pointGap[(int)c.collider.gameObject.GetComponent<Point>().number.x] + 2)/20;
                            RaycastHit hit;
                            sc.movePoint((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, 0, weight/2));
                            /*
                            if (Physics.Raycast(transform.position, Vector3.down, out hit,gap + maxDist, mask.value))
                            {
                                if(hit.collider.tag == "Anvil")
                                {
                                    Debug.Log("Hit Anvil");
                                    sc.movePoint((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, 0, weight));
                                }
                                else
                                {
                                    Debug.Log("Hit No Anvil");
                                    sc.movePointDisplace((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, 0, weight));
                                }
                            }else
                            {
                                Debug.Log("No Hit");
                                sc.movePointDisplace((int)c.collider.gameObject.GetComponent<Point>().number.x, (int)c.collider.gameObject.GetComponent<Point>().number.y, new Vector3(0, 0, weight));
                            }*/

                        }
                        else if (yVel > zVel && yVel > xVel)
                        {
                            Debug.Log("Stretch");
                            sc.Stretch(1, sc.moveAmount*(weight*10));
                        }
                        //sc.updateMesh();
                    }
                }
            }
        }
    }
}
