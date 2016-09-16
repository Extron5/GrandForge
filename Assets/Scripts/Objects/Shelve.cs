using UnityEngine;
using System.Collections;

public class Shelve : MonoBehaviour {

    public Vector3 targetPos;
    public GameObject Wall;
    private bool noKids = false;
    public Rigidbody rb;
	// Use this for initialization
	void Start () {
        //rb = transform.parent.GetComponent<Rigidbody>();
        targetPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
        if(transform.position == targetPos)
        {
            if (!noKids)
            {
                FreeKids();
            }
        }
        else
        {
            //rb.velocity = Vector3.Lerp(transform.position,targetPos,5);
        }
	}

    public void newWall()
    {

        GameObject wall = Instantiate(Wall, new Vector3(transform.parent.transform.position.x, transform.parent.transform.position.y, transform.parent.transform.position.z), transform.parent.transform.rotation) as GameObject;
        wall.GetComponentInChildren<Shelve>().targetPos = transform.position;
        Destroy(transform.parent.gameObject);
    }

    void FreeKids()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Debug.Log(i);
            transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            //transform.GetChild(i).gameObject.transform.parent = null;
            
            noKids = true;
        }
        transform.DetachChildren();
    }
}
