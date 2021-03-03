using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour
{

    public Mesh hexMesh;
    public List<Vector3> vertList;
    public List<int> triangles;

    private void Awake()
    {
        hexMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = hexMesh;

        hexMesh.name = "HexMash";
        vertList = new List<Vector3>();
        triangles = new List<int>();
     
    }

    public void Start()
    {
        vertList.Clear();
        triangles.Clear();
        hexMesh.Clear();

        Vector3 pos = this.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangulate(pos, pos + CreateMap.centers[i], pos + CreateMap.centers[i + 1]);
        }

        hexMesh.vertices = vertList.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();


        GetComponent<MeshCollider>().sharedMesh = hexMesh;
    }

    //根据三角形的三个顶点 绘制三角形
    void AddTriangulate(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int index = vertList.Count;
        vertList.Add(v1);
        vertList.Add(v2);
        vertList.Add(v3);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray way = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(way, out hit))
            {
                if (hit.collider.tag == "hex")
                {
                    Debug.LogError("射线检测到了");
                }
            }
        }
    }
}
