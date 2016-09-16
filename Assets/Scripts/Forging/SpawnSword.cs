using UnityEngine;
using System.Collections;

public class SpawnSword : MonoBehaviour {

    public GameObject copperSword;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "CopperOre")
        {
            Instantiate(copperSword, c.transform.position,  c.transform.rotation);
            Destroy(c.gameObject);
        }
    }
}
