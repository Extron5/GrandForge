using UnityEngine;
using System.Collections;

public class BreakStump : MonoBehaviour {

    public GameObject Planks;
    public Vector3 targetSize;
    public Vector3 startSize;

    // Use this for initialization
    void Start () {
        
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        startSize = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, 0.01f);
	}

    public void Chop()
    {
        GameObject planks = Instantiate(Planks, transform.position,transform.rotation) as GameObject;
        for(int i = 0; i < planks.transform.childCount; i++)
        {
            planks.transform.GetChild(i).GetComponent<WoodType>().woodType = gameObject.GetComponent<WoodType>().woodType;
            planks.transform.GetChild(i).GetComponent<WoodType>().setMat();
        }
        planks.transform.DetachChildren();
        Destroy(gameObject);
    }
}
