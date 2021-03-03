using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertList;
    List<int> triangles;


    private void Awake()
    {
        hexMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = hexMesh ;

        hexMesh.name = "HexMash";
        vertList = new List<Vector3>();
        triangles = new List<int>();

    }


    public void Triangulate(GameObject[] hexs)
    {
        vertList.Clear();
        triangles.Clear();
        hexMesh.Clear();
        for (int i = 0; i < hexs.Length; i++)
        {
            Triangulate(hexs[i]);
        }
        hexMesh.vertices = vertList.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
    }
    public void Triangulate(GameObject hex)
    {
        Vector3 pos = hex.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangulate(pos, pos + CreateMap.centers[i], pos + CreateMap.centers[i+1]);
        }
    }

    //根据三角形的三个顶点 绘制三角形
    void AddTriangulate(Vector3 v1,Vector3 v2,Vector3 v3)
    {
        int index = vertList.Count;
        vertList.Add(v1);
        vertList.Add(v2);
        vertList.Add(v3);

        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
    }
}
