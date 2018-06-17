using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour {

    private readonly int m_StageIndex = 11;
    public GameObject m_obj;

    public List<GameObject> m_ListObj = new List<GameObject>();

    private void Start()
    {
        BeanManager bean = BeanManager.instance;
        UITable com = transform.GetComponent<UITable>();
        com.repositionNow = true;
        for (int i = 0; i < m_StageIndex; i++)
        {
            GameObject obj = NGUITools.AddChild(com.gameObject, m_obj);

            m_ListObj.Add(obj);
        
            if (PlayerPrefs.GetString(bean.StageKey()[i]) == "Open")
            {
                obj.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format("{0}", i + 1);
            }
            else
            {
                obj.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format("잠금");
            }

            com.Reposition();
        }

      //  PlayerPrefs.DeleteAll();
    }
    // Use this for initialization



}
