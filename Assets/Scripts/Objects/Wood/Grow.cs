using UnityEngine;
using System.Collections;

public class Grow : MonoBehaviour {

    public Vector3 targetSize;
    public float growSize = 0.01f;
    public Rigidbody rb;
    public bool grav = true;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        rb.isKinematic = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(transform.localScale != targetSize)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSize);
        }
        else if(grav)
        {
            rb.isKinematic = false;
            Destroy(this);
        }
        
        
    }
}
