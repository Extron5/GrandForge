using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour
{

    public int finger = 0;
    public HingeJoint joint1;
    public HingeJoint joint2;
    public HingeJoint joint3;
    bool pressed = false;

    // Use this for initialization
    void Start()
    {
        joint1 = transform.GetChild(0).GetComponent<HingeJoint>();
        joint2 = transform.GetChild(1).GetComponent<HingeJoint>();
        joint3 = transform.GetChild(2).GetComponent<HingeJoint>();
    }

    // Update is called once per frame
    void Update()
    {

        if (finger == 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                pressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                pressed = false;
            }
        }
        if (finger == 1)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                pressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                pressed = false;
            }
        }
        if (finger == 2)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                pressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                pressed = false;
            }
        }
        if (finger == 3)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                pressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                pressed = false;
            }
        }
        if (finger == 4)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                pressed = false;
            }
        }

        if (pressed && finger != 4)
        {
            JointSpring spring;
            spring = joint1.spring;
            spring.targetPosition = 85;
            joint1.spring = spring;
            spring = joint2.spring;
            spring.targetPosition = 85;
            joint2.spring = spring;
            spring = joint3.spring;
            spring.targetPosition = 85;
            joint3.spring = spring;
        }
        else if (pressed && finger == 4)
        {
            JointSpring spring;
            spring = joint1.spring;
            spring.targetPosition = 30;
            joint1.spring = spring;
            spring = joint2.spring;
            spring.targetPosition = 15;
            joint2.spring = spring;
            spring = joint3.spring;
            spring.targetPosition = 70;
            joint3.spring = spring;
        }
        else
        {
            JointSpring spring;
            spring = joint1.spring;
            spring.targetPosition = 0;
            joint1.spring = spring;
            spring = joint2.spring;
            spring.targetPosition = 0;
            joint2.spring = spring;
            spring = joint3.spring;
            spring.targetPosition = 0;
            joint3.spring = spring;
        }


    }
}
