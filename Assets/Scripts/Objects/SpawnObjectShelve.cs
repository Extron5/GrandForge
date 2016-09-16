using UnityEngine;
using System.Collections;

public class SpawnObjectShelve : MonoBehaviour {

    public GameObject spawnPoint;
    public GameObject spawnObject;
    private GameObject currentObject;

	// Use this for initialization
	void Start () {
        SpawnCard();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnCard()
    {
        currentObject = Instantiate(spawnObject, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        if(currentObject.tag == "FloppyWood")
        {
            currentObject.GetComponent<WoodType>().setMat();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject == currentObject)
        {
            if (c.tag == spawnObject.tag)
            {
                SpawnCard();
            }
        }
    }
}
