using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPull : MonoBehaviour
{
    public List<GameObject> m_Listprefep = new List<GameObject>();
    private Dictionary<string, List<GameObject>> m_map = new Dictionary<string, List<GameObject>>();
    private static MemoryPull s_Instance = null;

    private void Awake()
    {
        Object[] m_Obj = Resources.LoadAll("Prefab/infiniteLoad");

        foreach(var v in m_Obj)
        {
            m_Listprefep.Add(v as GameObject);
        }
    }

    public GameObject FindObj(string ObjName)
    {
        foreach (var v in m_Listprefep)
        {
            if (v.name == ObjName)
                return v;
        }

        Debug.LogError(string.Format("해당 이름이 없습니다."));

        return null;
    }

    public void initObject(string OBjName, int Res)
    {
        GameObject prefab = FindObj(OBjName);

        if (m_map.ContainsKey(OBjName) == false)
        {
            List<GameObject> list = new List<GameObject>();

            m_map.Add(OBjName, list);
        }

        List<GameObject> insertList = m_map[OBjName];

        GameObject parent = new GameObject(OBjName);
        parent.transform.parent = transform;


        for (int i = 0; i < Res; i++)
        {
            GameObject obj = GameObject.Instantiate<GameObject>(prefab);
            obj.SetActive(false);
            obj.transform.parent = parent.transform;
            insertList.Add(obj);
        }
    }

    public GameObject getObj(string OBjName)
    {
        List<GameObject> SechList = m_map[OBjName];

        foreach (var v in SechList)
        {
            if (v.activeSelf == false)
            {
                v.SetActive(true);
                return v;

            }
        }

        initObject(OBjName, SechList.Count);

        return getObj(OBjName);
    }

    public static MemoryPull instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new GameObject("Manager").AddComponent<MemoryPull>();
                //오브젝트가 생성이 안되있을경우 생성. 
            }
            return s_Instance;
        }
    }


    void OnApplicationQuit()
    {
        s_Instance = null;
        //게임종료시 삭제. 
    }
}
