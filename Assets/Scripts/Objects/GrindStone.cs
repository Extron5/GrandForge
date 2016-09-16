using UnityEngine;
using System.Collections;

public class GrindStone : MonoBehaviour {

    public float spinSpeed = 1.0f;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.angularVelocity = new Vector3(0,0,spinSpeed);
	}

    void OnCollisionEnter(Collision c)
    {
        //Fix this
        if (c.gameObject.transform.tag == "Metal")
        {
            Debug.Log("Sharp");
            if (c.collider.gameObject.transform.parent != null)
            {
                
                SwordController sc = c.collider.gameObject.transform.parent.GetComponent<SwordController>();
                if (c.collider.gameObject.GetComponent<Point>() != null)
                {
                    
                    int xx = (int)c.collider.gameObject.GetComponent<Point>().number.x;
                    int zz = (int)c.collider.gameObject.GetComponent<Point>().number.y;
                    if (xx == sc.xSize)
                    {
                        sc.SharpenTip();
                    }
                    else
                    {
                        sc.SharpenPoint(xx, zz);
                    }
                }
            }
        }
    }
}
