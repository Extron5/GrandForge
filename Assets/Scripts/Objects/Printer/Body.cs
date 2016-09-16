using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {

    public bool Block = false;
    public GameObject BlockObject;
    public bool Part = false;
    public GameObject PartObject;
    public GameObject Arm;
    private Quaternion ArmStart;
    public GameObject Pusher;

	// Use this for initialization
	void Start () {
        ArmStart = new Quaternion(0, 0, 0, 1);
    }
	
    void Push()
    {
        Pusher.GetComponent<Pusher>().GoalPos = Pusher.GetComponent<Pusher>().EndPos;
    }

	// Update is called once per frame
	void Update () {
	    if(Block && Part)
        {
           
            BlockObject.transform.localScale = Vector3.MoveTowards(BlockObject.transform.localScale, new Vector3(BlockObject.transform.localScale.x,0, BlockObject.transform.localScale.z),0.0005f);
            Arm.transform.GetChild(0).transform.RotateAround(Arm.transform.position, new Vector3(0, 1, 0), 3);
            //Arm.transform.localRotation = Quaternion.RotateTowards(BlockObject.transform.localRotation, new Quaternion(90, BlockObject.transform.localRotation.y +1, 0,1) , 0.0005f);
            if (BlockObject.transform.localScale.y <= 0)
            {
                Destroy(BlockObject);
                for (int i = 0; i < PartObject.transform.childCount; i++)
                {
                    PartObject.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
                    
                }
                PartObject.GetComponent<Grow>().grav = true;
                Block = false;
                Part = false;
                Push();
            }
        }
        else
        {
            if(Arm.transform.GetChild(0).transform.localRotation.y >= 360)
            {
                Arm.transform.GetChild(0).transform.localRotation = new Quaternion(Arm.transform.GetChild(0).transform.localRotation.x, Arm.transform.GetChild(0).transform.localRotation.y - 360, Arm.transform.GetChild(0).transform.localRotation.z, Arm.transform.GetChild(0).transform.localRotation.w);
            }
            if (Mathf.Abs(Arm.transform.GetChild(0).transform.localRotation.y)*Mathf.Rad2Deg <= Mathf.Abs(ArmStart.y) * Mathf.Rad2Deg + 1 && Mathf.Abs(Arm.transform.GetChild(0).transform.localRotation.y) * Mathf.Rad2Deg >= Mathf.Abs(ArmStart.y) * Mathf.Rad2Deg - 1 &&
                Mathf.Abs(Arm.transform.GetChild(0).transform.localRotation.x) * Mathf.Rad2Deg <= Mathf.Abs(ArmStart.x) * Mathf.Rad2Deg + 1 && Mathf.Abs(Arm.transform.GetChild(0).transform.localRotation.x) * Mathf.Rad2Deg >= Mathf.Abs(ArmStart.x) * Mathf.Rad2Deg - 1 &&
                Mathf.Abs(Arm.transform.GetChild(0).transform.localRotation.z) * Mathf.Rad2Deg <= Mathf.Abs(ArmStart.z) * Mathf.Rad2Deg + 1 && Mathf.Abs(Arm.transform.GetChild(0).transform.localRotation.z) * Mathf.Rad2Deg >= Mathf.Abs(ArmStart.z) * Mathf.Rad2Deg - 1)
            {
            }
            else
            {
                Arm.transform.GetChild(0).transform.RotateAround(Arm.transform.position, new Vector3(0, 1, 0), 1);
            }
        }
	}
}
