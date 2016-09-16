using UnityEngine;
using System.Collections;

public class HandleSnap : MonoBehaviour
{
    public Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        rb = transform.parent.transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 8 )
        {
            if (c.gameObject.transform.FindChild("HandleSnap").childCount == 0)
            {

                transform.parent.transform.parent.transform.parent = c.gameObject.transform.FindChild("HandleSnap");
                transform.parent.transform.parent.transform.localPosition = Vector3.zero;
                transform.parent.transform.parent.transform.rotation = c.gameObject.transform.FindChild("HandleSnap").rotation;
                Destroy(transform.parent.GetComponentInParent<FixedJoint>());
                Destroy(rb);
                
                //rb.isKinematic = true;
                transform.gameObject.layer = 10;
                Destroy(transform.parent.GetComponentInParent<VRTK.VRTK_InteractableObject>());
                transform.parent.transform.parent.transform.parent = c.gameObject.transform.FindChild("HandleSnap");
                transform.parent.transform.parent.transform.localPosition = Vector3.zero;
                transform.parent.transform.parent.transform.rotation = c.gameObject.transform.FindChild("HandleSnap").rotation;
            }
        }
    }
}
