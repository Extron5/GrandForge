using UnityEngine;
using System.Collections;

public class WoodType : MonoBehaviour {

    public string woodType = "Oak";
    


	// Use this for initialization
	void Start () {
	
	}

    public void setMat()
    {
        GetComponent<MeshRenderer>().material = Resources.Load("Wood/" + woodType) as Material;
    }
	
	// Update is called once per frame
	void Update () {

    }
}
