using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    private Vector3 m_LastPosition;
    private MemoryPull m_Memory;

    private GameObject[] m_ViewLoad = new GameObject[6];
    private GameObject[] m_HideLoad = new GameObject[5];
    private GameObject m_SpareLoad;

    private GameObject m_Wave;

    public GameObject m_Taget;

    // Use this for initialization
    void Start()
    {
        GameObject WaveObj = Resources.Load("Prefab/Wave") as GameObject;
        GameObject Wave = new GameObject("Wave");

        Wave.transform.parent = transform;

        m_Wave = GameObject.Instantiate<GameObject>(WaveObj);
        m_Wave.transform.parent = Wave.transform;

        m_Memory = MemoryPull.instance;

        m_LastPosition = Vector3.zero;

        int index = 0;

        foreach (var v in m_Memory.m_Listprefep)
        {
            if (index <9)
                m_Memory.initObject(string.Format("Load_{0}", index), 10);
            else if(index == 9) 
                m_Memory.initObject(string.Format("USD"), 30);
            else if (index == 10)
                m_Memory.initObject(string.Format("LoadCresh"), 30);
            else if (index == 11)
                m_Memory.initObject(string.Format("BlockCraesh"), 30);
            index++; 
        }


        m_ViewLoad[0]  = m_Memory.getObj(string.Format("Load_{0}", 0));

        m_LastPosition.z += m_ViewLoad[0].GetComponent<BoxCollider>().bounds.size.z;

        m_ViewLoad[0].transform.position = m_LastPosition;
        m_ViewLoad[0].GetComponent<Load>().init(m_ViewLoad[0].transform.position - m_ViewLoad[0].GetComponent<BoxCollider>().bounds.size
          , m_ViewLoad[0].transform.position);

        for (int i = 1; i < 6; i++)
        {
            m_ViewLoad[i] = m_Memory.getObj(string.Format("Load_{0}", Random.Range(1, 9)));

            m_LastPosition.z += m_ViewLoad[i].GetComponent<BoxCollider>().bounds.size.z;
            m_ViewLoad[i].transform.position = m_LastPosition;
            m_ViewLoad[i].GetComponent<Load>().init(m_ViewLoad[i].transform.position - m_ViewLoad[i].GetComponent<BoxCollider>().bounds.size
                , m_ViewLoad[i].transform.position);

        }
        m_SpareLoad = m_ViewLoad[4];
        m_Wave.transform.position = new Vector3(m_Wave.transform.position.x, m_Wave.transform.position.y,0);

        for (int i = 0; i < 5; i++)
        {
            m_HideLoad[i] = m_Memory.getObj(string.Format("Load_{0}", Random.Range(1, 9)));
            m_LastPosition.z += m_HideLoad[i].GetComponent<BoxCollider>().bounds.size.z;
            m_HideLoad[i].transform.position = m_LastPosition;
        }

        for (int i = 0; i < 5; i++)
        {
            m_HideLoad[i].SetActive(false);  
        }

    }

    // Update is called once per frame
    void Update()
    {
     
        if(m_ViewLoad[4].transform.position.z <= m_Taget.transform.position.z)
        {
            for(int i = 0; i < m_ViewLoad.Length; i++)
            {
                m_ViewLoad[i].SetActive(false);
                m_ViewLoad[i].GetComponent<Load>().Remove();
            }

            m_SpareLoad.SetActive(false);
            m_SpareLoad = m_ViewLoad[4];
            m_ViewLoad[0] = m_ViewLoad[5];
            m_ViewLoad[0].SetActive(true);
            m_SpareLoad.SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                m_ViewLoad[i + 1] = m_HideLoad[i];
                m_ViewLoad[i + 1].SetActive(true);

                m_ViewLoad[i+1].GetComponent<Load>().init(m_ViewLoad[i+1].transform.position - m_ViewLoad[i+1].GetComponent<BoxCollider>().bounds.size
              , m_ViewLoad[i+1].transform.position);
            }

            for (int i = 0; i < 5; i++)
            {
                m_HideLoad[i] = m_Memory.getObj(string.Format("Load_{0}", Random.Range(1, 9)));
                m_LastPosition.z += m_HideLoad[i].GetComponent<BoxCollider>().bounds.size.z;
                m_HideLoad[i].transform.position = m_LastPosition; 
            }

            for (int i = 0; i < 5; i++)
            {
                m_HideLoad[i].SetActive(false);
            }

            m_Wave.transform.position = new Vector3(m_Wave.transform.position.x, m_Wave.transform.position.y, m_ViewLoad[0].transform.position.z-100);

        }

    }

}
