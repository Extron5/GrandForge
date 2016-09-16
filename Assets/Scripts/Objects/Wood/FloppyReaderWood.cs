using UnityEngine;
using System.Collections;

public class FloppyReaderWood : MonoBehaviour {

    public GameObject spawnPoint;
    public GameObject WoodStump;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "FloppyWood")
        {
            GameObject Stump = Instantiate(WoodStump, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Stump.GetComponent<WoodType>().woodType = c.gameObject.GetComponent<WoodType>().woodType;
            Stump.GetComponent<WoodType>().setMat();
            Destroy(c.gameObject);
        }
    }
}
