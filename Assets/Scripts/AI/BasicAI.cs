using UnityEngine;
using System.Collections;

public class BasicAI : MonoBehaviour {

    public NavMeshAgent nav;
    public Vector2 minMaxX;
    public Vector2 minMaxY;
    public Vector2 minMaxTime;
    public Vector3 goal;
    public float waitTime;
    private float timer = 0;
    public GameObject marketPoints;
    public GameObject[] shops;
    public float chanceToShop = 10;
    private bool stopped = false;
    public bool shopping = false;
    public Color color;

	// Use this for initialization
	void Start () {
        color = Random.ColorHSV();
        transform.FindChild("Body").GetComponent<MeshRenderer>().material.color = color;
        nav = GetComponent<NavMeshAgent>();
        marketPoints = GameObject.FindGameObjectWithTag("MarketPoints");
        shops = new GameObject[marketPoints.transform.childCount];
        for(int i = 0; i < shops.Length; i++)
        {
            shops[i] = marketPoints.transform.GetChild(i).gameObject;
        }
        setGoal();
        setTarget();
	}
	
	// Update is called once per frame
	void Update () {
        if (nav != null)
        {
            //Debug.DrawLine(transform.position, goal, Color.red);
            if (Vector3.Distance(transform.position, nav.destination) < 1 && !stopped)
            {
                setGoal();
                stopped = true;
            }
            if (waitTime >= 0 && stopped)
            {
                waitTime -= Time.deltaTime;
            }
            else if (waitTime <= 0 && stopped == true)
            {
                stopped = false;
                setTarget();
            }
        }
	}

    void setWaitTime()
    {
        waitTime = Random.Range(minMaxTime.x,minMaxTime.y);
    }

    void setGoal()
    {
        int rand = Random.Range(0, 100);
        if (rand <= chanceToShop)
        {
            if (!shopping)
            {
                setShopping();
            }
            else
            {
                setWander();
            }
        }
        else if(rand > chanceToShop)
        {
            if (shopping)
            {
                setShopping();
            }
            else
            {
                setWander();
            }
        }
    }

    void setShopping()
    {
        int num = Random.Range(0, shops.Length);
        goal = shops[num].transform.position;
        shopping = true;
        setWaitTime();
    }

    void setWander()
    {
        goal.x = Random.Range(minMaxX.x, minMaxX.y);
        goal.z = Random.Range(minMaxY.x, minMaxY.y);
        setWaitTime();
    }

    void setTarget()
    {
        nav.SetDestination(goal);
    }
}
