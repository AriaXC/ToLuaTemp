using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{
    public int wight = 10;
    public int height = 10;
    public GameObject child;

    GameObject[] hexs;
    //外半径
    public const float outerRadius = 10f;
    //内半径
    public const float innerRadius = outerRadius * 0.866025404f;
    public static Vector3[] centers = {
             new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius),
    };

    //public Canvas gridCanvas;
    public HexMesh gridMesh;
    private void Awake()
    {
        hexs = new GameObject[wight*height];
        int index = 0;
        for (int i = 0; i < wight; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                AddMap(i, j, index);
                index++;
            }
        }

        //gridCanvas = GetComponentInChildren<Canvas>();
        gridMesh = GetComponentInChildren<HexMesh>();
    }
    private void Start()
    {
        //对网格进行拆分
       gridMesh.Triangulate(hexs);
    }
    private void AddMap(int i,int j,int index)
    {
        Vector3 pos;
        pos.x = (i+ j*0.5f - j/2) * innerRadius*2;
        pos.y = 0;
        pos.z = j * outerRadius*1.5f;

        hexs[index] = Instantiate(child, this.transform);
        hexs[index].transform.localPosition = pos;
        hexs[index].transform.Find("Text").gameObject.GetComponent<TextMesh>().text = index.ToString();
    }
}


