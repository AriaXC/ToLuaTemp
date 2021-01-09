using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorText : BaseMeshEffect
{
    private Color _up=Color.blue;
    private Color _down=Color.green;

    private UIVertex vertex = new UIVertex();

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!this.IsActive())
            return;

        //Debug.LogError("总共的顶点数==" + vh.currentVertCount);

        int count = vh.currentVertCount;

        int curNum;
        for (int i = 0; i < count; i++)
        {
            curNum = i / 4;
            //字符一分为2
            if (i - curNum * 4 < 2)
            {
                vh.PopulateUIVertex(ref vertex, i);
                vertex.color = _up;
                vh.SetUIVertex(vertex, i);
            }
            else
            {
                vh.PopulateUIVertex(ref vertex, i);
                vertex.color = _down;
                vh.SetUIVertex(vertex, i);
            }

        }

        //List<UIVertex> list = new List<UIVertex>();
        //vh.GetUIVertexStream(list);
        ////Debug.LogError("总共的数==" + list.Count);
        //int count = list.Count;

        //int curNum;
        //UIVertex v = new UIVertex();
        //for (int i = 0; i < count; i++)
        //{
        //    curNum = i / 6;
        //    //字符一分为2
        //    //Debug.LogError(i - curNum * 6);
       
        //    if (i - curNum * 6 < 3)
        //    {
        //        v = list[i];
        //        v.color = _down;
        //        list[i] = v;
        //    }
        //    else
        //    {
        //        v = list[i];
        //        v.color = _up;
        //        list[i] = v;
        //    }

        //}
        //vh.AddUIVertexTriangleStream(list);
    }

}
