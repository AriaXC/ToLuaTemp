using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class other
{
    public other()
    {
        Debug.Log("我是基类的构造函数");
    }

}
public class OOT : other
{
    public OOT()
    {
        Debug.Log("我是子类的构造函数");
    }
}
