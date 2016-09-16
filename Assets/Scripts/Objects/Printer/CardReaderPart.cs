using UnityEngine;
using System.Collections;

public class CardReaderPart : MonoBehaviour {

    
    public GameObject spawnPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SpawnBlock()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "FloppyPart" && GetComponentInParent<Body>().Block)
        {
            GameObject Part = Instantiate(c.GetComponent<FloppyPart>().part, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            for (int i = 0; i < Part.transform.childCount; i++)
            {
                Part.transform.GetChild(i).GetComponent<Collider>().isTrigger = true;
                Part.transform.GetChild(i).GetComponent<MeshRenderer > ().material = GetComponentInParent<Body>().BlockObject.GetComponent<MeshRenderer>().material;
            }
            
            //Part.GetComponentInChildren<MeshCollider>().isTrigger = true;
            Part.transform.localScale = Part.GetComponent<Grow>().targetSize;
            Part.GetComponent<Grow>().grav = false;
            GetComponentInParent<Body>().PartObject = Part;
            GetComponentInParent<Body>().Part = true;
            //Part.GetComponent<Rigidbody>().isKinematic = true;
            //Stump.GetComponent<MeshRenderer>().material = c.GetComponent<MeshRenderer>().material;
            Destroy(c.gameObject);
        }
    }
}
