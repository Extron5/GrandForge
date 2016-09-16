using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SwordController : MonoBehaviour {
    public class Metalpoint
    {
        public bool sharpened;
        public Vector3 pos;
        public float height;
        public Vector3 originalPos;
        public Vector3 axis;
    }
    public bool pointSharpened = false;
    public int xSize, ySize, zSize;
	public int roundness;

	private Mesh mesh;
    public GameObject pointObject;
    public GameObject[] points;
	public Vector3[] vertices;
	private Vector3[] normals;
	private Color32[] cubeUV;
    public bool sharp = false;
    public float moveAmount = 0.5f;

    public Metalpoint[,] metalPoints;
    public float[] pointDist;
    public float[] pointGap;
    public float[] pointHeight;
    public bool[] pointHit;
    public Vector2 MinMaxDist;
    public bool meshEdited = false;
    public float maxGap = 3.0f;
    public float maxHeightGap = 3.0f;
    public float timer = 0;
    public Vector3 local;
    public float startDist = 0.2f;

	private void Awake () {
		Generate();
	}
    public void Update()
    {
        if(Input.GetButtonDown("W"))
        {
            SharpenTip();
        }if(Input.GetButtonDown("Q"))
        {
            //Sharpen();
        }
        if(meshEdited && timer >= 0.05f)
        {
            updateMesh();
            meshEdited = false;
            timer = 0;
        }else if(meshEdited)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        local = new Vector3(transform.parent.transform.localRotation.x, transform.parent.transform.localRotation.y, transform.parent.transform.localRotation.z);
    }

    public void SharpenTip()
    {
        pointSharpened = true;
        for (int i = 0; i < 4; i++)
        {
            metalPoints[xSize, i].sharpened = true;
        }
        updateMesh();
    }


    private void Generate () {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Cube";
        metalPoints = new Metalpoint[xSize + 1, zSize + 1];
        CreateVertices();
		CreateTriangles();
        pointDist = new float[xSize + 1];
        pointGap = new float[xSize + 1];
        pointHeight = new float[xSize + 1];
        pointHit = new bool[xSize + 1];
        for (int i = 0; i <= xSize; i++)
        {
            pointDist[i] = startDist;
        }
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                metalPoints[x, z].pos.y = 0.25f;
            }
        }
        for (int x = 0; x <= xSize; x++)
        {
            pointGap[x] = metalPoints[x, 2].pos.z - metalPoints[x, 1].pos.z;
            pointHeight[x] = 1.0f;
            pointHit[x] = false;
        }
        updateMesh();
	}

    public void Stretch(int x, float amount)
    {
        for (int i = x; i <= xSize; i++)
        {
            if (pointDist[i] <= MinMaxDist.y && pointDist[i] >= MinMaxDist.x)
            {
                pointDist[i] += amount;
            }
            else
            {
                pointDist[i] -= amount;
            }
        }
        updateMesh();
    }

    public void SharpenPoint(int x, int z)
    {
        if (metalPoints[x, z].sharpened == false)
        {
            movePoint(x, z, new Vector3(0, 0.25f, 0));
            metalPoints[x, z].sharpened = true;

            updatePoint();
        }
    }
    public void Sharpen()
    {
        for(int x = 0; x <= xSize; x++)
        {
            //points[x].transform.position = new Vector3(points[x].transform.position.x, 1, points[x].transform.position.z);
            movePoint(x, 0, new Vector3(0, 0.25f, 0));
        }
        for (int x = 0; x <= xSize; x++)
        {
            movePoint(x, 3, new Vector3(0, 0.25f, 0));
        }
        updatePoint();
    }

    public void SharpenPoint()
    {
        movePoint(xSize, 1, new Vector3(2, 0.5f, 0));
        metalPoints[xSize, 1].pos.z = metalPoints[xSize, 1].originalPos.z + 0.5f;
        movePoint(xSize - 1, 1, new Vector3(1, 0, 0));
        metalPoints[xSize - 1, 1].pos.z = metalPoints[xSize - 1, 1].originalPos.z + 0.5f;
        movePoint(xSize - 1, 2, new Vector3(1, 0, 0));
        metalPoints[xSize - 1, 2].pos.z = metalPoints[xSize - 1, 2].originalPos.z - 0.5f;
        movePoint(xSize, 2, new Vector3(2, 0.5f, 0));
        metalPoints[xSize, 2].pos.z = metalPoints[xSize, 2].originalPos.z - 0.5f;
        updatePoint();
    }

    public void movePoint(int x, int z, Vector3 v)
    {
        Vector3 temp = metalPoints[x, z].pos;
        Vector3 temp2 = Vector3.zero;
        Vector3 temp3 = Vector3.zero;


        if (metalPoints[x, z].pos.z > maxGap)
        {
            metalPoints[x, z].pos.z = maxGap;
            v.z = 0;
        }
        if (z <= zSize/2)
        {
            temp3 = metalPoints[x, z + 1].pos;
            //Right
            if (pointGap[x] <= 0)
            {
                metalPoints[x, z].pos.z = metalPoints[x, 2].pos.z - 1.0f;
                metalPoints[x, z + 1].pos.z = metalPoints[x, 2].pos.z;
            }

                metalPoints[x, z].pos += new Vector3(v.x, -v.y, v.z);
                metalPoints[x, z + 1].pos += new Vector3(v.x, 0, v.z);

        }else if (z >= zSize / 2)
        {
            temp2 = metalPoints[x, z - 1].pos;
            //Left
            if (pointGap[x] <= 0)
            {
                metalPoints[x, z].pos.z = metalPoints[x, 2].pos.z + 1.0f;
                metalPoints[x, z - 1].pos.z = metalPoints[x, 2].pos.z;
            }


                metalPoints[x, z].pos -= new Vector3(-v.x, v.y, v.z);
                metalPoints[x, z - 1].pos -= new Vector3(-v.x, 0, v.z);

        }
        pointGap[x] = metalPoints[x, 2].pos.z - metalPoints[x, 1].pos.z;
        meshEdited = true;
        //updateMesh();
        if(metalPoints[x,z].sharpened)
        {
            metalPoints[x, z].pos = temp;
            if (z >= zSize / 2)
            {
                metalPoints[x, z - 1].pos = temp2;
            }
            else
            {
                metalPoints[x, z + 1].pos = temp3;
            }
        }
    }

    public void movePointDisplace(int x, int z, Vector3 v)
    {
        if (metalPoints[x, z].pos.z > maxGap)
        {
            metalPoints[x, z].pos.z = maxGap;
            v.z = 0;
        }
        if (z <= zSize / 2)
        {
            for (int xx = x; xx <= xSize; xx++)
            {
                //Right
                if (pointHit[xx] == false)
                {
                    if (pointGap[x] <= 0)
                    {
                        metalPoints[xx, z].pos.z = metalPoints[xx, 2].pos.z - 1.0f;
                        metalPoints[xx, z + 1].pos.z = metalPoints[xx, 2].pos.z;
                    }
                    else if (z == 0)
                    {
                        metalPoints[xx, z].pos += new Vector3(v.x, -v.y, v.z);
                        metalPoints[xx, z + 1].pos += new Vector3(v.x, 0, v.z);
                        metalPoints[xx, z + 3].pos += new Vector3(-v.x, v.y, v.z);
                        metalPoints[xx, z + 2].pos += new Vector3(-v.x, 0, v.z);
                    }
                    pointHit[xx] = true;
                }
            }
        }
        else if (z >= zSize / 2)
        {
            for (int xx = x; xx <= xSize; xx++)
            {
                //Left
                if (pointHit[xx] == false)
                {
                    if (pointGap[xx] <= 0)
                    {
                        metalPoints[xx, z].pos.z = metalPoints[xx, 2].pos.z + 1.0f;
                        metalPoints[xx, z - 1].pos.z = metalPoints[xx, 2].pos.z;
                    }
                    else if (z == 3)
                    {
                        metalPoints[xx, z].pos -= new Vector3(-v.x, v.y, v.z);
                        metalPoints[xx, z - 1].pos -= new Vector3(-v.x, 0, v.z);
                        metalPoints[xx, z - 3].pos -= new Vector3(v.x, -v.y, v.z);
                        metalPoints[xx, z - 2].pos -= new Vector3(v.x, 0, v.z);
                    }
                    pointHit[xx] = true;
                }
            }
        }
        pointGap[x] = metalPoints[x, 2].pos.z - metalPoints[x, 1].pos.z;
        meshEdited = true;
        //updateMesh();
    }

    public void movePointDisplaceHeight(int x, int z, Vector3 v)
    {
        if (metalPoints[x, z].pos.y > maxHeightGap)
        {
            metalPoints[x, z].pos.y = maxHeightGap;
            Debug.Log("Nope");
            v.y = 0;
        }
        metalPoints[x, 0].pos += new Vector3(0, v.y,0);
        metalPoints[x, 1].pos += new Vector3(0, v.y, 0);
        metalPoints[x, 2].pos += new Vector3(0, v.y, 0);
        metalPoints[x, 3].pos += new Vector3(0, v.y, 0);
        
        //pointGap[x] = metalPoints[x, 2].pos.z - metalPoints[x, 1].pos.z;
        meshEdited = true;
        //updateMesh();
    }
    public void movePointDisplace1(int x, int z, Vector3 v)
    {
        if (metalPoints[x, z].pos.z > maxGap)
        {
            metalPoints[x, z].pos.z = maxGap;
            v.z = 0;
        }
        if (z <= zSize / 2)
        {
            for (int xx = x; xx <= xSize; xx++)
            {
                //Right
                if (pointGap[x] <= 0)
                {
                    metalPoints[xx, z].pos.z = metalPoints[xx, 2].pos.z - 1.0f;
                    metalPoints[xx, z + 1].pos.z = metalPoints[xx, 2].pos.z;
                }
                else if (z == 0)
                {
                    metalPoints[xx, z].pos += new Vector3(v.x, -v.y, v.z);
                    metalPoints[xx, z + 1].pos += new Vector3(v.x, 0, v.z);
                    metalPoints[xx, z + 3].pos += new Vector3(-v.x, v.y, v.z);
                    metalPoints[xx, z + 2].pos += new Vector3(-v.x, 0, v.z);
                }
            }
        }
        else if (z >= zSize / 2)
        {
            for (int xx = x; xx <= xSize; xx++)
            {
                //Left
                if (pointGap[xx] <= 0)
                {
                    metalPoints[xx, z].pos.z = metalPoints[xx, 2].pos.z + 1.0f;
                    metalPoints[xx, z - 1].pos.z = metalPoints[xx, 2].pos.z;
                }
                else if (z == 3)
                {
                    metalPoints[xx, z].pos -= new Vector3(-v.x, v.y, v.z);
                    metalPoints[xx, z - 1].pos -= new Vector3(-v.x, 0, v.z);
                    metalPoints[xx, z - 3].pos -= new Vector3(v.x, -v.y, v.z);
                    metalPoints[xx, z - 2].pos -= new Vector3(v.x, 0, v.z);
                }
            }
        }
        pointGap[x] = metalPoints[x, 2].pos.z - metalPoints[x, 1].pos.z;
        //updateMesh();
    }
    public void movePointDisplace2(int x, int z, Vector3 v)
    {
        if (metalPoints[x, z].pos.z > maxGap)
        {
            metalPoints[x, z].pos.z = maxGap;
            v.z = 0;
        }
        if (z <= zSize / 2)
        {
            //Right
            if (pointGap[x] <= 0)
            {
                metalPoints[x, z].pos.z = metalPoints[x, 2].pos.z - 1.0f;
                metalPoints[x, z + 1].pos.z = metalPoints[x, 2].pos.z;
            }
            else if (z == 0)
            {
                metalPoints[x, z].pos += new Vector3(v.x, -v.y, v.z);
                metalPoints[x, z + 1].pos += new Vector3(v.x, 0, v.z);
                metalPoints[x, z + 3].pos += new Vector3(-v.x, v.y, v.z);
                metalPoints[x, z +2].pos += new Vector3(-v.x, 0, v.z);
            }
        }
        else if (z >= zSize / 2)
        {
            //Left
            if (pointGap[x] <= 0)
            {
                metalPoints[x, z].pos.z = metalPoints[x, 2].pos.z + 1.0f;
                metalPoints[x, z - 1].pos.z = metalPoints[x, 2].pos.z;
            }
            else if (z == 3)
            {
                metalPoints[x, z].pos -= new Vector3(-v.x, v.y, v.z);
                metalPoints[x, z - 1].pos -= new Vector3(-v.x, 0, v.z);
                metalPoints[x, z-3].pos -= new Vector3(v.x, -v.y, v.z);
                metalPoints[x, z -2].pos -= new Vector3(v.x, 0, v.z);
            }
        }
        pointGap[x] = metalPoints[x, 2].pos.z - metalPoints[x, 1].pos.z;
        //updateMesh();
    }

    public void hit()
    {

    }

    public void updatePoint()
    {
        int j = 0;
        int extra = (zSize + 1) * (xSize + 1);

            for (int z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    //if (y >= 1)
                    //{
                        points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                        points[j+extra].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, -metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                        //points[j].gameObject.GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);
                        /*
                    }
                    else
                    {
                        //points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, -metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                        //points[j+extra].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, -metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                        //points[j].gameObject.GetComponent<BoxCollider>().size = new Vector3(2,2,2);
                    }
                    */
                    j++;
                }
        }
        for (int i = 0; i < points.Length; i++)
        {
            //normals[i] = (points[w].transform.localPosition).normalized;
            vertices[i * 2] = points[i].transform.localPosition;
            //normals[i+1] = (points[w].transform.localPosition).normalized;
            vertices[i * 2 + 1] = points[i].transform.localPosition;
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.colors32 = cubeUV;
    }
    public void updateMesh()
    {
        int j = 0;
        for (int y = 0; y <= ySize; y++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    // j = x + (z* (xSize+1)) + (y * (xSize+1)+(zSize+1))
                    if (x != 0 && y==0)
                    {
                        metalPoints[x, z].pos = new Vector3(pointDist[x] + metalPoints[x - 1, z].pos.x, metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                    }
                    if (y >= 1)
                    {
                        //points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                        if (!metalPoints[x, z].sharpened)
                        {
                            points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y , metalPoints[x, z].pos.z);
                        }
                        else //Sharpened
                        {
                            if (x == xSize && z != 1 && z != 2)//Tip
                            {
                                //float tempGap = metalPoints[xSize,0]
                                if (z <= zSize / 2)
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x + 1, metalPoints[x, z].pos.y - 0.25f, metalPoints[x, z].pos.z + (pointGap[x] + 2) / 2);
                                }
                                else
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x + 1, metalPoints[x, z].pos.y - 0.25f, metalPoints[x, z].pos.z - (pointGap[x] + 2) / 2);
                                }

                            }
                            else if (x == xSize)//Tip Middle
                            {
                                if (z <= zSize / 2)
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y, metalPoints[x, z].pos.z + (pointGap[x]) / 2);
                                }
                                else
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y, metalPoints[x, z].pos.z - (pointGap[x]) / 2);
                                }
                            }
                            else//Not Tip
                            {
                                points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                            }
                        }
                    }
                    else
                    {
                        if (!metalPoints[x, z].sharpened)
                        {
                            points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y - 0.5f, metalPoints[x, z].pos.z);
                        }
                        else //Sharpened
                        {
                            if (x == xSize && z != 1 && z!= 2)//Tip
                            {
                                if (z <= zSize / 2)
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x + 1, -metalPoints[x, z].pos.y + 0.25f, metalPoints[x, z].pos.z + (pointGap[x] + 2) / 2);
                                }
                                else
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x + 1, -metalPoints[x, z].pos.y + 0.25f, metalPoints[x, z].pos.z - (pointGap[x] + 2) / 2);
                                }
                            }
                            else if (x == xSize)//Tip Middle
                            {
                                if (z <= zSize / 2)
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y - 0.5f, metalPoints[x, z].pos.z + (pointGap[x]) / 2);
                                }
                                else
                                {
                                    points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, metalPoints[x, z].pos.y - 0.5f, metalPoints[x, z].pos.z - (pointGap[x]) / 2);
                                }
                            }
                            else//Not Tip
                            {
                                points[j].transform.localPosition = new Vector3(metalPoints[x, z].pos.x, -metalPoints[x, z].pos.y, metalPoints[x, z].pos.z);
                            }

                        }
                    }
                    
                    j++;
                }
            }
        }
        for(int x = 0; x <= xSize; x++)
        {
            pointHit[x] = false;
        }

        // update vertices array after moving points
        for (int i = 0; i < points.Length; i++)
        {
            //Maybe?
            vertices[i*2] = points[i].transform.localPosition;
            vertices[i*2 + 1] = points[i].transform.localPosition;
        }

        // update end positions (bottom and top)
        for (int z = 0; z <= zSize; z++)
        {
            for (int y = 0; y <= ySize; y++)
            {
                int pointIndex = z * (xSize + 1) + y * ((xSize + 1) * (zSize + 1));
                vertices[getBottomIndex(z, y)] = points[pointIndex].transform.localPosition;
                vertices[getTopIndex(z,y)] = points[pointIndex + xSize].transform.localPosition;
            }
        }

        // loop across all quads to calculate normals
        for (int z = 0; z < zSize; z++)
        {
            for (int y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    // get the indices of the four points of this quad
                    int indexBelow = getIndex((x == 0 ? x : x - 1), y, z, true);
                    int indexAbove = getIndex((x < xSize ? x + 1 : x), y, z, true);
                    int indexLeft = getIndex(x, y, z, true);
                    int indexRight = getIndex(x, y, z + 1, false);

                    // calculate the normals
                    Vector3 acrossSword = vertices[indexRight] - vertices[indexLeft];
                    Vector3 upSword = vertices[indexAbove] - vertices[indexBelow];
                    normals[indexLeft] = Vector3.Cross(acrossSword, upSword);

                    // flip the normal on the reverse side
                    if (y == 0)
                        normals[indexLeft] *= -1;

                    normals[indexRight] = normals[indexLeft];

                    if (y == 0)
                    {
                        if (z == 0)
                        {
                            int iBelow = getIndex((x == 0 ? x : x - 1), 0, z, false);
                            int iAbove = getIndex((x < xSize ? x + 1 : x), 0, z, false);
                            Vector3 acrossEdge = Vector3.up;
                            Vector3 upEdge = vertices[iAbove] - vertices[iBelow];
                            int i1 = getIndex(x, 0, z, false);
                            int i2 = getIndex(x, 1, z, false);
                            normals[i1] = normals[i2] = Vector3.Cross(acrossEdge, upEdge);
                        }
                        if (z == zSize - 1)
                        {
                            int iBelow = getIndex((x == 0 ? x : x - 1), 0, zSize ,true);
                            int iAbove = getIndex((x < xSize ? x + 1 : x), 0, zSize, true);
                            Vector3 acrossEdge = Vector3.up;
                            Vector3 upEdge = vertices[iAbove] - vertices[iBelow];
                            int i1 = getIndex(x, 0, zSize, true);
                            int i2 = getIndex(x, 1, zSize, true);
                            normals[i1] = normals[i2] = -Vector3.Cross(acrossEdge, upEdge);
                        }
                    }
                }
            }
        }

        for (int z = 0; z <= zSize; z++)
        {
                int iBelow = getBottomIndex((z == 0 ? z : z - 1), 0);
                int iAbove = getBottomIndex((z < zSize ? z + 1 : z), 0);
                Vector3 acrossEdge = Vector3.up;
                Vector3 upEdge = vertices[iAbove] - vertices[iBelow];
                int i1 = getBottomIndex(z, 0);
                int i2 = getBottomIndex(z, 1);
                normals[i1] = normals[i2] = -Vector3.Cross(acrossEdge, upEdge);

                iBelow = getTopIndex((z == 0 ? z : z - 1), 0);
                iAbove = getTopIndex((z < zSize ? z + 1 : z), 0);
                acrossEdge = Vector3.up;
                upEdge = vertices[iAbove] - vertices[iBelow];
                i1 = getTopIndex(z, 0);
                i2 = getTopIndex(z, 1);
                normals[i1] = normals[i2] = Vector3.Cross(acrossEdge, upEdge);
        }
        /*
        // calculate normals
        for (int y = 0; y <= ySize; y++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    int xBelow = (x == 0) ? 0 : x - 1;
                    int xAbove = (x == xSize) ? xSize : x + 1;
                    int zLeft  = (z == 0) ? 0 : z - 1;
                    int zRight = (z == zSize) ? zSize : z + 1;

                    int index = x + (z * (xSize + 1)) + (y * (xSize + 1) * (zSize + 1));

                    int indexBelow = xBelow + (z * (xSize + 1)) + (y * (xSize + 1) * (zSize + 1));
                    int indexAbove = xAbove + (z * (xSize + 1)) + (y * (xSize + 1) * (zSize + 1));

                    int indexLeft  = x + (zLeft  * (xSize + 1)) + (y * (xSize + 1) * (zSize + 1));
                    int indexRight = x + (zRight * (xSize + 1)) + (y * (xSize + 1) * (zSize + 1));

                    Vector3 localZ = points[indexRight].transform.localPosition - points[indexLeft].transform.localPosition;
                    Vector3 localX = points[indexAbove].transform.localPosition - points[indexBelow].transform.localPosition;

                    normals[index] = Vector3.Cross(localX, localZ).normalized;
                    // reverse the top faces so that the normals point upwards
                    if (y != 0)
                        normals[index] *= -1;
                }
            }
        }*/

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.colors32 = cubeUV;
    }

    int getIndex(int x, int y, int z, bool isRight)
    {
        return (x + z * (xSize + 1) + y *(xSize+1)*(zSize+1)) *2 + (isRight? 1:0);
    }

    int getBottomIndex(int z, int y)
    {
        int baseIndex = (xSize+1)*(ySize+1)*(zSize+1)*2; // skip over main faces

        return baseIndex + z + y * (zSize + 1);
    }

    int getTopIndex(int z, int y)
    {
        int baseIndex = (xSize + 1) * (ySize + 1) * (zSize + 1) * 2; // skip over main faces
        baseIndex += (ySize + 1) * (zSize + 1); // skip over bottom indexes

        return baseIndex + z + y * (zSize + 1);
    }

    private void CreateVertices () {
		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize -3) * 4;
		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
        points = new GameObject[cornerVertices + edgeVertices + faceVertices];
        vertices = new Vector3[(cornerVertices + edgeVertices + faceVertices)*2 + ((ySize + 1) * (zSize + 1)) *2];
        Debug.Log(vertices.Length);
		normals = new Vector3[vertices.Length];
		cubeUV = new Color32[vertices.Length];


        for (int y = ySize; y <= ySize; y++)
        {
            for (int z = 0; z <= zSize; z++)   
            {
                for (int x = 0; x <= xSize; x++)
                {
                    metalPoints[x, z] = new Metalpoint();
                    metalPoints[x, z].sharpened = false;
                    metalPoints[x, z].originalPos = new Vector3(x,y,z);
                    metalPoints[x, z].pos = new Vector3(x, y, z);
                    metalPoints[x, z].axis = new Vector3(0, 0, z);

                    SetVertex(x, y, z);
                }
            }
        }

        
        int i = 0;
        for (int y = 0; y <= ySize; y++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    points[i] = Instantiate(pointObject, transform.position + vertices[i], transform.rotation) as GameObject;
                    points[i].GetComponent<Point>().number = new Vector2(x, z);
                    points[i].transform.tag = "Metal";
                    points[i].transform.gameObject.layer = 10;
                    points[i].transform.parent = gameObject.transform;
                    if (i <= xSize || (i > ((xSize + 1) * (zSize)) - 1 && i < ((xSize + 1) * (zSize)) + ((xSize + 1) * 2)) || (i < (xSize + 1) * (zSize + 1) * (ySize + 1) && i >= (xSize + 1) * (zSize + 1) * (ySize + 1) - xSize - 1))
                    {
                        points[i].GetComponent<Point>().edge = true;
                        points[i].GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        Destroy(points[i].GetComponent<BoxCollider>());
                    }
                    i++;
                }
            }
        }
        points[(xSize+1)*2-1].GetComponent<Point>().edge = true;
        points[(xSize + 1) * 3 - 1].GetComponent<Point>().edge = true;
        points[(xSize + 1) * 6 - 1].GetComponent<Point>().edge = true;
        points[(xSize + 1) * 7 - 1].GetComponent<Point>().edge = true;

        mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.colors32 = cubeUV;

    }
    

    private void SetVertex (int x, int y, int z) {

        int vIndexL = getIndex(x, y, z, false);
        int vIndexR = getIndex(x, y, z, true);

        vertices[vIndexL] = metalPoints[x,z].pos;
        vertices[vIndexR] = metalPoints[x,z].pos;

        cubeUV[vIndexL] = new Color32((byte)x, (byte)y, (byte)z, 0);
        cubeUV[vIndexR] = new Color32((byte)x, (byte)y, (byte)z, 0);




        /*Vector3 inner = vertices[i] = new Vector3(x, y, z);
        if (x < roundness)
        {
            inner.x = roundness;
        }
        else if (x > xSize - roundness)
        {
            inner.x = xSize - roundness;
        }
        if (y < roundness)
        {
            inner.y = roundness;
        }
        else if (y > ySize - roundness)
        {
            inner.y = ySize - roundness;
        }
        if (z < roundness)
        {
            inner.z = roundness;
        }
        else if (z > zSize - roundness)
        {
            inner.z = zSize - roundness;
        }
        normals[i] = -(vertices[i] - inner).normalized;
		vertices[i] = inner + normals[i] ;
		cubeUV[i] = new Color32((byte)x, (byte)y, (byte)z, 0);*/
    }

    private void CreateTriangles () {
        Vector2[] uv = new Vector2[vertices.Length];
        for (int z = 0; z <= zSize; z++)
        {
            for (int y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    uv[getIndex(x, y, z, false)] = new Vector2((float)x / xSize, (float)z / zSize);
                    uv[getIndex(x, y, z, true)] = new Vector2((float)x / xSize, (float)y / ySize);
                }
            }
        }
        mesh.uv = uv;


        int[] triangles = new int[(((zSize+2) * (ySize + 1) * xSize) + zSize * 2) * 3 * 2];
        int index = 0;

        // loop across all quads
        for (int z = 0; z < zSize; z++)
        {
            for (int y = 0; y <= ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    // get the indices of the four points of this quad
                    int index00 = getIndex(x, y, z, true);
                    int index10 = getIndex(x + 1, y, z, true);
                    int index01 = getIndex(x, y, z + 1, false);
                    int index11 = getIndex(x + 1, y, z + 1, false);

                    // set up the two triangles for the quad
                    if (y != 0)
                    {
                        triangles[index] = index00; index++;
                        triangles[index] = index01; index++;
                        triangles[index] = index10; index++;

                        triangles[index] = index11; index++;
                        triangles[index] = index10; index++;
                        triangles[index] = index01; index++;
                    }
                    else
                    {
                        triangles[index] = index00; index++;
                        triangles[index] = index10; index++;
                        triangles[index] = index01; index++;

                        triangles[index] = index11; index++;
                        triangles[index] = index01; index++;
                        triangles[index] = index10; index++;
                    }

                    // stitch the top and bottom together
                    if (y == 0)
                    {
                        if (z == 0)
                        {
                            int i00 = getIndex(x, 0, z, false);
                            int i10 = getIndex(x + 1, 0, z, false);
                            int i01 = getIndex(x, 1, z, false);
                            int i11 = getIndex(x + 1, 1, z, false);

                            triangles[index] = i00; index++;
                            triangles[index] = i01; index++;
                            triangles[index] = i10; index++;

                            triangles[index] = i11; index++;
                            triangles[index] = i10; index++;
                            triangles[index] = i01; index++;
                        }
                        if (z == zSize-1)
                        {
                            int i00 = getIndex(x, 0, zSize, true);
                            int i10 = getIndex(x + 1, 0, zSize, true);
                            int i01 = getIndex(x, 1, zSize, true);
                            int i11 = getIndex(x + 1, 1, zSize, true);

                            triangles[index] = i00; index++;
                            triangles[index] = i10; index++;
                            triangles[index] = i01; index++;

                            triangles[index] = i11; index++;
                            triangles[index] = i01; index++;
                            triangles[index] = i10; index++;
                        }
                    }
                }
            }
        }

        for (int z = 0; z < zSize; z++)
        {
            // quad on bottom    
            int i00 = getBottomIndex(z, 0);
            int i10 = getBottomIndex(z, 1);
            int i01 = getBottomIndex(z + 1, 0);
            int i11 = getBottomIndex(z + 1, 1);

            triangles[index] = i00; index++;
            triangles[index] = i01; index++;
            triangles[index] = i10; index++;

            triangles[index] = i11; index++;
            triangles[index] = i10; index++;
            triangles[index] = i01; index++;

            // quad on top      
            i00 = getTopIndex(z, 0);
            i10 = getTopIndex(z, 1);
            i01 = getTopIndex(z + 1, 0);
            i11 = getTopIndex(z + 1, 1);

            triangles[index] = i00; index++;
            triangles[index] = i10; index++;
            triangles[index] = i01; index++;

            triangles[index] = i11; index++;
            triangles[index] = i01; index++;
            triangles[index] = i10; index++;


        }

        mesh.SetTriangles(triangles, 0);

        /*

        int[] trianglesBlade2 = new int[xSize * zSize * 6 + 12 + (24 *(zSize-2))];
        int[] trianglesBlade3 = new int[xSize * zSize * 6 * 2];
        int tu = 0;
        int ti = 0;
       

        for (int vi = xSize +1, y = 1; y < zSize-1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                if (x == xSize - 1)
                {
                    trianglesBlade2[tu] = vi;
                    trianglesBlade2[tu + 3] = trianglesBlade2[tu + 2] = vi + xSize + 1;
                    trianglesBlade2[tu + 4] = trianglesBlade2[tu + 1] = vi + 1;
                    trianglesBlade2[tu + 5] = vi + xSize + 2;
                    tu += 6;
                }
                else {
                    trianglesBlade3[ti] = vi;
                    trianglesBlade3[ti + 3] = trianglesBlade3[ti + 2] = vi + xSize + 1;
                    trianglesBlade3[ti + 4] = trianglesBlade3[ti + 1] = vi + 1;
                    trianglesBlade3[ti + 5] = vi + xSize + 2;
                }
            }
        }
        for (int vi = ((zSize + 1) * xSize) + zSize + 1 + xSize+1, y = 1; y < zSize-1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                if (x == xSize-1)
                {
                    trianglesBlade2[tu] = vi;
                    trianglesBlade2[tu + 3] = trianglesBlade2[tu + 2] = vi + 1;
                    trianglesBlade2[tu + 4] = trianglesBlade2[tu + 1] = vi + xSize + 1;
                    trianglesBlade2[tu + 5] = vi + xSize + 2;
                    tu += 6;
                }
                else {
                    trianglesBlade3[ti] = vi;
                    trianglesBlade3[ti + 3] = trianglesBlade3[ti + 2] = vi + 1;
                    trianglesBlade3[ti + 4] = trianglesBlade3[ti + 1] = vi + xSize + 1;
                    trianglesBlade3[ti + 5] = vi + xSize + 2;
                }
            }
        }
        ti += 6;

        ti = 0;
        
        for (int vi = ((xSize + 1) * (zSize - 1)), y = 0; y < 1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, tu += 6, vi++)
            {
                trianglesBlade2[tu] = vi;
                trianglesBlade2[tu + 3] = trianglesBlade2[tu + 2] = vi + xSize + 1;
                trianglesBlade2[tu + 4] = trianglesBlade2[tu + 1] = vi + 1;
                trianglesBlade2[tu + 5] = vi + xSize + 2;
            }
        }

        for (int vi = ((xSize + 1) * (zSize )) * (ySize + 1), y = 0; y < 1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, tu += 6, vi++)
            {
                trianglesBlade2[tu] = vi;
                trianglesBlade2[tu + 3] = trianglesBlade2[tu + 2] = vi + 1;
                trianglesBlade2[tu + 4] = trianglesBlade2[tu + 1] = vi + xSize + 1;
                trianglesBlade2[tu + 5] = vi + xSize + 2;
            }
        }

        for (int vi = (xSize + 1) * (zSize), y = 0; y < 1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, tu += 6, vi++)
            {
                trianglesBlade2[tu] = vi;
                trianglesBlade2[tu + 3] = trianglesBlade2[tu + 2] = vi + ((zSize + 1) * xSize) + zSize + 1;
                trianglesBlade2[tu + 4] = trianglesBlade2[tu + 1] = vi + 1;
                trianglesBlade2[tu + 5] = vi + ((zSize + 1) * xSize) + zSize + 1 + 1;
            }
        }
        for (int vi = (xSize) * (zSize) + zSize - 1, y = 0; y < zSize-2; y++, vi++)
        {
            for (int z = 2; z < zSize; z++, tu += 6, vi++)
            {
                trianglesBlade2[tu] = vi;
                trianglesBlade2[tu + 1] = (vi) * 2 + 1;
                trianglesBlade2[tu + 2] = (vi) * 2 + 2 + xSize;
                trianglesBlade2[tu + 3] = vi-xSize-1;
                trianglesBlade2[tu + 4] = (vi) * 2 + 1;
                trianglesBlade2[tu + 5] = vi;
            }
        }
        trianglesBlade2[tu] = (xSize + 1) * (zSize);
        trianglesBlade2[tu + 1] = (xSize + 1) * (zSize) + ((zSize + 1) * xSize) + zSize + 1;
        trianglesBlade2[tu + 2] = (xSize + 1) * (zSize) - (xSize + 1);
        trianglesBlade2[tu + 3] = (xSize + 1) * (zSize) + ((zSize + 1) * xSize) + zSize - xSize;
        trianglesBlade2[tu + 4] = (xSize + 1) * (zSize) - (xSize + 1);
        trianglesBlade2[tu + 5] = (xSize + 1) * (zSize) + ((zSize + 1) * xSize) + zSize + 1;
        tu += 6;
        trianglesBlade2[tu] = (xSize) * (zSize+1) + zSize;
        trianglesBlade2[tu + 1] = ((xSize) * (zSize + 1) + zSize) * 2 + 1 - xSize-1;
        trianglesBlade2[tu + 2] = ((xSize) * (zSize + 1) + zSize) *2 + 1;
        trianglesBlade2[tu + 3] = (xSize) * (zSize + 1) + zSize;
        trianglesBlade2[tu + 4] = (xSize) * (zSize + 1) + zSize - xSize-1;
        trianglesBlade2[tu + 5] = ((xSize) * (zSize + 1) + zSize) * 2 + 1 - xSize - 1;
        ti += 6;

        int[] trianglesBlade1 = new int[xSize * zSize * ySize * 6 +12];
        
        ti = 0;
        
        for (int vi = 0, y = 0; y < 1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                trianglesBlade1[ti] = vi;
                trianglesBlade1[ti + 3] = trianglesBlade1[ti + 2] = vi + xSize + 1;
                trianglesBlade1[ti + 4] = trianglesBlade1[ti + 1] = vi + 1;
                trianglesBlade1[ti + 5] = vi + xSize + 2;
            }
        }

        for (int vi = 0, y = 0; y < 1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                trianglesBlade1[ti] = vi;
                trianglesBlade1[ti + 3] = trianglesBlade1[ti + 2] = vi + 1 ;
                trianglesBlade1[ti + 4] = trianglesBlade1[ti + 1] = vi + ((zSize + 1) * xSize) + zSize + 1;
                trianglesBlade1[ti + 5] = vi + ((zSize + 1) * xSize) + zSize + 1 + 1;
            }
        }

        for (int vi = ((zSize + 1) * xSize) + zSize +1, y = 0; y < 1; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                trianglesBlade1[ti] = vi;
                trianglesBlade1[ti + 3] = trianglesBlade1[ti + 2] = vi + 1;
                trianglesBlade1[ti + 4] = trianglesBlade1[ti + 1] = vi + xSize + 1;
                trianglesBlade1[ti + 5] = vi + xSize + 2;
            }
        }
        trianglesBlade1[ti] = 0;
        trianglesBlade1[ti + 1] = xSize + 1;
        trianglesBlade1[ti + 2] = ((zSize + 1) * xSize) + zSize + 1;
        trianglesBlade1[ti + 3] = xSize + 1 + ((zSize + 1) * xSize) + zSize + 1;
        trianglesBlade1[ti + 4] = ((zSize + 1) * xSize) + zSize + 1;
        trianglesBlade1[ti + 5] = xSize + 1;
        ti += 6;
        trianglesBlade1[ti] = xSize;
        trianglesBlade1[ti + 1] = ((zSize + 1) * xSize) + zSize + 1 + xSize;
        trianglesBlade1[ti + 2] = xSize * 2 + 1;
        trianglesBlade1[ti + 3] = xSize * 2 + 1 + ((zSize + 1) * xSize) + zSize + 1;
        trianglesBlade1[ti + 4] = xSize * 2 + 1;
        trianglesBlade1[ti + 5] = ((zSize + 1) * xSize) + zSize + 1 + xSize;
        ti += 6;
        
        mesh.subMeshCount = 3;
        mesh.SetTriangles(trianglesBlade1, 0);
        mesh.SetTriangles(trianglesBlade2, 1);
        mesh.SetTriangles(trianglesBlade3, 2);
        */

        hit();
    }

	private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * ySize;
		for (int x = 0; x < xSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

		int vMin = ring * (ySize + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
			}
			t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}

	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}

	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {
		triangles[i] = v00;
		triangles[i + 1] = triangles[i + 4] = v01;
		triangles[i + 2] = triangles[i + 3] = v10;
		triangles[i + 5] = v11;
		return i + 6;
	}

	private void CreateColliders () {
		AddBoxCollider(xSize, ySize - roundness * 2, zSize - roundness * 2);
		AddBoxCollider(xSize - roundness * 2, ySize, zSize - roundness * 2);
		AddBoxCollider(xSize - roundness * 2, ySize - roundness * 2, zSize);

		Vector3 min = Vector3.one * roundness;
		Vector3 half = new Vector3(xSize, ySize, zSize) * 0.5f; 
		Vector3 max = new Vector3(xSize, ySize, zSize) - min;

		AddCapsuleCollider(0, half.x, min.y, min.z);
		AddCapsuleCollider(0, half.x, min.y, max.z);
		AddCapsuleCollider(0, half.x, max.y, min.z);
		AddCapsuleCollider(0, half.x, max.y, max.z);
		
		AddCapsuleCollider(1, min.x, half.y, min.z);
		AddCapsuleCollider(1, min.x, half.y, max.z);
		AddCapsuleCollider(1, max.x, half.y, min.z);
		AddCapsuleCollider(1, max.x, half.y, max.z);
		
		AddCapsuleCollider(2, min.x, min.y, half.z);
		AddCapsuleCollider(2, min.x, max.y, half.z);
		AddCapsuleCollider(2, max.x, min.y, half.z);
		AddCapsuleCollider(2, max.x, max.y, half.z);
	}

	private void AddBoxCollider (float x, float y, float z) {
		BoxCollider c = gameObject.AddComponent<BoxCollider>();
		c.size = new Vector3(x, y, z);
	}

	private void AddCapsuleCollider (int direction, float x, float y, float z) {
		CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();
		c.center = new Vector3(x, y, z);
		c.direction = direction;
		c.radius = roundness;
		c.height = c.center[direction] * 2f;
	}

//	private void OnDrawGizmos () {
//		if (vertices == null) {
//			return;
//		}
//		for (int i = 0; i < vertices.Length; i++) {
//			Gizmos.color = Color.black;
//			Gizmos.DrawSphere(vertices[i], 0.1f);
//			Gizmos.color = Color.yellow;
//			Gizmos.DrawRay(vertices[i], normals[i]);
//		}
//	}
}