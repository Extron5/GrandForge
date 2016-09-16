using UnityEngine;
using System.Collections;

public class ArrowParent : MonoBehaviour {
    //private bool stuck = false;

    void Start()
    {

    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag != "AI")
        {
            Destroy(c.gameObject.GetComponent<NavMeshAgent>());
            //stuck = true;
            //transform.SetParent(c.transform);
            //GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
