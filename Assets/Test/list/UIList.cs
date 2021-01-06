using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
public class UIList : MonoBehaviour
{
    [FormerlySerializedAs("ChildItem"), SerializeField]
    protected GameObject ChildItem;

    //滑动方向
    [FormerlySerializedAs("isHorizontal"), SerializeField]
    protected bool isHorizontal=false;
    [FormerlySerializedAs("isV"), SerializeField]
    protected bool isVertical = false;
   

    //视窗大小
    [FormerlySerializedAs("viewSize"), SerializeField]
    protected Vector2 viewSize=new Vector2(500,500);
    //间距
    [FormerlySerializedAs("gap"), SerializeField]
    protected Vector2 gap=new Vector2(0,0);

    [FormerlySerializedAs("Count"), SerializeField]
    protected Vector2 Count = new Vector2(12,12);



    private ScrollRect _scroll;

    private GameObject _viewPort;
    private GameObject _content;

    private int _maxCount;
    private int[] data;

    private RectTransform _reContent;
    private RectTransform _reChild;

    private int _startIndex;
    private int _endIndex;
    private int _nowIndex;

    private Vector2 _startContentPos;

    private List<GameObject> _showList;
    private List<GameObject> _delList;

    private Dictionary<GameObject, int> indexDic = new Dictionary<GameObject, int>();
    private void Awake()
    {
        _scroll = gameObject.AddComponent<ScrollRect>();

        _viewPort = new GameObject("ViewPort");
        _viewPort.transform.SetParent(this.transform);
        _viewPort.transform.localPosition = Vector3.zero;
        RectTransform _reViewPort = _viewPort.AddComponent<RectTransform>();
        _reViewPort.pivot = Vector2.up;
        _viewPort.AddComponent<Mask>().showMaskGraphic = false;
        _viewPort.AddComponent<Image>();
        _scroll.viewport = _reViewPort;

        _content = new GameObject("Content");
        _reContent = _content.AddComponent<RectTransform>();
        _content.transform.SetParent(_viewPort.transform);
        if (isVertical)
        {
            _reContent.anchoredPosition = new Vector2(-viewSize.x / 2, viewSize.y / 2);
        }
        _reContent.pivot = Vector2.up;
        _scroll.content = _reContent;
        //记录最开始的值 
        _startContentPos = _reContent.anchoredPosition;


        //只有一种方向
        _reViewPort.sizeDelta = viewSize;
        _scroll.horizontal = isHorizontal;
        _scroll.vertical = !isHorizontal;

        //赋值
        _reChild = ChildItem.GetComponent<RectTransform>();
        _showList = new List<GameObject>();
        _delList = new List<GameObject>();
    }

    private void Start()
    {
        data = new int[100];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i;
        }
        SetContent(data);
        InitView(data);
        _nowIndex = 0;
        _scroll.onValueChanged.AddListener(OnValueChange);
    }

    //初始化
    public void InitView(int[] data)
    {
        //最多几行  加2
        if (isVertical)
        {
            _maxCount = Convert.ToInt32(Math.Ceiling(viewSize.y / (_reChild.sizeDelta.y + gap.y) + 2) * Count.x);
            if (_maxCount - 2 * Count.x < data.Length && _maxCount > data.Length)
            {
                _maxCount = data.Length;
            }
            if (_maxCount - 2 * Count.x > data.Length)
            {
                _maxCount = data.Length;
            }
        }

        for (int i = 0; i < _maxCount; i++)
        {
            _showList.Add(GetItem(i));
        }
    }

    public void OnValueChange(Vector2 ve)
    {
        if (isHorizontal)
        {
    
        }
        else if (isVertical)
        {
            _startIndex = GetStartIndex();
            if (_nowIndex == _startIndex)
            {
                return;
            }
            _nowIndex = _startIndex;
            if (_startIndex != -1)
            {
                UpdateItem();
            }
        }
     }
    public void UpdateItem()
    {
        _endIndex = _startIndex + _maxCount;
        for (int i = _showList.Count-1; i >=0; i--)
        {
            int index = indexDic[_showList[i]];
            //if (index < _startIndex || index >= _endIndex)
            //{
                _delList.Add(_showList[i]);
                _showList.RemoveAt(i);
            //}
        }
        //Debug.LogError("LLL==="+_delList.Count);
        for (int i = _startIndex; i < _endIndex; i++)
        {
            if (i > data.Length)
                continue;

            _showList.Add(GetItem(i));
        }
    }
    //获取第一个
    public int GetStartIndex()
    {
        if (isHorizontal)
        {

        }
        else if (isVertical)
        {
            return Convert.ToInt32(Mathf.Floor((_reContent.anchoredPosition.y - _startContentPos.y) / (_reChild.sizeDelta.y + gap.y)) * Count.x);
        }
        return -1;
    }


    public void SetItemData(GameObject item,int index)
    {
        indexDic[item] = index;
        //Debug.LogError(item.name + "   " + index);
        item.transform.Find("Text").gameObject.GetComponent<Text>().text = item.name;
    }
    public void SetContent(int[] data)
    {
        if (isHorizontal)
        {
           
        }
        else if(isVertical) {
            float y = data.Length / Count.x;
            if (data.Length % Count.x != 0)
            {
                y = y + 1;
            }
            _reContent.sizeDelta =
                new Vector2(viewSize.x, _reChild.sizeDelta.y * y+ (y- 1) * gap.y);
        }
    }

    public void SetPosition(GameObject item)
    {
        int index = indexDic[item];
        RectTransform childRect = item.GetComponent<RectTransform>();
        if (isHorizontal)
        {

        }
        else if (isVertical)
        {
            float x = index % Count.x; //行
            float y = (float)Math.Floor(index / Count.x); //列
            //Debug.Log(index + "  " + x + "  " + y);
            childRect.anchoredPosition = new Vector2(x * childRect.sizeDelta.x + (gap.x * x),
                -((y * childRect.sizeDelta.y) + (gap.y * (y - 1))));

        }
    }


    private GameObject poolRoot;
    private ArrayList _poolList = new ArrayList();
    //对象池
    public GameObject GetItem(int index)
    {

        CheckPoolRoot();
        GameObject obj;
        if (_delList.Count>0)
        {
            obj =_delList[0];
            _delList.Remove(obj);
        }
        else if (_poolList.Count > 0)
        {
            obj = _poolList[0] as GameObject;
            _poolList.RemoveAt(0);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(ChildItem);
        }
        obj.transform.parent = _content.transform;
        obj.name = index.ToString();
        SetItemData(obj, index);
        SetPosition(obj);
        return obj;
    }
    public void OnRecovery(GameObject obj)
    {
        obj.SetActive(false);
        CheckPoolRoot();
        obj.transform.parent = poolRoot.transform;
        _poolList.Add(obj);
    }
    public void CheckPoolRoot()
    {
        if (poolRoot == null)
        {
            poolRoot = new GameObject("[PoolRoot]");
            poolRoot.SetActive(false);
        }
    }
}
