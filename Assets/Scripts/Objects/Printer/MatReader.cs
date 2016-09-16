using UnityEngine;
using System.Collections;

public class MatReader : MonoBehaviour {

    public GameObject MatBlock;
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
        if (c.tag == "Wood" || c.tag == "CopperOre")
        {
            GameObject Stump = Instantiate(MatBlock, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Stump.GetComponent<MeshRenderer>().material = c.GetComponent<MeshRenderer>().material;
            GetComponentInParent<Body>().BlockObject = Stump;
            GetComponentInParent<Body>().Block = true;
            Destroy(c.gameObject);
        }
    }
}
