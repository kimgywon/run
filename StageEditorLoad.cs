using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class StageEditorLoad : MonoBehaviour
{

    private stageMemoryPull m_Memory;

    [Range(0, 11)]
    public int m_StageNumber;

    [Range(1, 8)]
    public int m_LoadType;

    private string path;

    private List<int> m_ListMap = new List<int>();
    private List<GameObject> m_ListVeiwObj = new List<GameObject>();
    private Vector3 m_LastPosition;
    private BeanManager m_bean;
    private int m_LastIndex;
    private int m_CameraIndex;

    private GameObject m_Camera;

    private bool m_IsMapLoad;

    private bool m_IsCameraMoving;

    private void Awake()
    {
        m_IsMapLoad = false;
        m_IsCameraMoving = false;
        m_bean = BeanManager.instance;
        m_bean.reset();
        m_LastPosition = Vector3.zero;
        path = System.IO.Directory.GetCurrentDirectory() + @"\Assets\Map\";
        m_StageNumber = 0;
        m_LoadType = 0;
        m_LastIndex = 0;
        m_CameraIndex = 0;
        m_Memory = stageMemoryPull.instance;

        m_Camera = GameObject.FindGameObjectWithTag("camera");

        int index = 0;

        foreach (var v in m_Memory.m_Listprefep)
        {
            m_Memory.initObject(string.Format("Load_{0}", index), 20);
            index++;
        }
    }


    public void MapLoad()
    {
        m_IsMapLoad = true;
        //맵 읽기
        GameObjReset();
        //int c = m_ListVeiwObj.Count;

        string source;

        string MapPath = path + m_bean.getStageName(m_StageNumber);
    REDER:
        if (File.Exists(MapPath))
        {
            StreamReader sr = new StreamReader(MapPath);
            source = sr.ReadLine();
            string[] values;
            while (source != null)
            {
                values = source.Split(',');

                if (values.Length == 0)
                {
                    sr.Close();
                    break;
                }

                foreach (var v in values)
                {
                    if (v != "")
                        m_ListMap.Add(System.Convert.ToInt32(v));
                }

                source = sr.ReadLine();
            }

            MapDraw();
        }
        else
        {

            StreamWriter sw = new StreamWriter(MapPath);
            //   StreamWriter sw2 = new StreamWriter(path + BeanManager.instance.getHurdleName(PlayerPrefs.GetInt("LastGameindex")));
            sw.Close();
            //   sw2.Close();
            goto REDER;
        }
    }

    public void MapSave()
    {
        StreamWriter sw = new StreamWriter(path + m_bean.getStageName(m_StageNumber));

        foreach (var v in m_ListMap)
        {
            if(v != 9)
            sw.Write(string.Format("{0},", v));
            else
                sw.Write(string.Format("{0},", 12));
        }
        sw.Close();

        Debug.Log("파일 저장완료");
    }

    private void MapDraw()
    {
        int index = 0;
        foreach (var v in m_ListMap)
        {
            m_ListVeiwObj.Add(m_Memory.getObj(string.Format("Load_{0}", v)));
            m_LastPosition.z += m_ListVeiwObj[index].GetComponent<BoxCollider>().bounds.size.z;
            m_ListVeiwObj[index].transform.position = m_LastPosition;
            index++;
        }

        m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, 0);

        m_LastIndex = m_ListMap.Count;
        Debug.Log("파일을 불러오기 완료");
    }

    private void GameObjReset()
    {
        m_ListMap.Clear();

        foreach (var v in m_ListVeiwObj)
        {
            v.SetActive(false);
        }

        m_ListVeiwObj.Clear();
    }


    public void BlockAdd()
    {
        if (!m_IsMapLoad)
        {
            Debug.Log("파일을 불러와 주세요");
            return;
        }

        m_ListMap.Add(m_LoadType);

        GameObject block = m_Memory.getObj(string.Format("Load_{0}", m_LoadType));

        float size = block.GetComponent<BoxCollider>().bounds.size.z;
        m_LastPosition.z += size;
        block.transform.position = m_LastPosition;

        m_ListVeiwObj.Add(block);
        m_LastIndex++;
        m_CameraIndex++;

        m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, m_LastPosition.z - (size / 2));
    }

    public void BlockSub()
    {
        if (!m_IsMapLoad)
        {
            Debug.Log("파일을 불러와 주세요");
            return;
        }

        if (m_LastIndex == 0)
        {
            Debug.Log("삭제할 블록이 없습니다.");
            return;
        }

        m_LastIndex--;
        m_CameraIndex--;

        m_ListMap.RemoveAt(m_LastIndex);

        float size = m_ListVeiwObj[m_LastIndex].GetComponent<BoxCollider>().bounds.size.z;
        m_LastPosition.z -= size;
        m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, m_LastPosition.z - (size / 2));

        m_ListVeiwObj[m_LastIndex].SetActive(false);
        m_ListVeiwObj.RemoveAt(m_LastIndex);
    }

    public void cameraRight()
    {
        if (m_IsCameraMoving)
            return;

        m_CameraIndex++;
        if (m_CameraIndex >= m_LastIndex)
        {
            m_CameraIndex--;
            return;
        }

        float size = m_ListVeiwObj[m_CameraIndex].GetComponent<BoxCollider>().bounds.size.z;
        m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z + size);

    }

    public void cameraLeft()
    {
        if (m_IsCameraMoving)
            return;

        m_CameraIndex--;
        if (m_CameraIndex < 0)
        {
            m_CameraIndex++;
            return;
        }

        float size = m_ListVeiwObj[m_CameraIndex].GetComponent<BoxCollider>().bounds.size.z;
        m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z - size);

    }


    IEnumerator CameraEnd()
    {
        if (m_IsCameraMoving == false)
        {
            m_IsCameraMoving = true;
            m_CameraIndex = m_LastIndex ;
            while (m_Camera.transform.position.z < m_LastPosition.z)
            {

                m_Camera.transform.Translate(Vector3.forward * 5f * Time.deltaTime,Space.World);

                yield return null;
            }

            m_IsCameraMoving = false;
        }

    }

    IEnumerator CameraStart()
    {
        if (m_IsCameraMoving == false)
        {
            m_IsCameraMoving = true;
            m_CameraIndex = 0;

            while (m_Camera.transform.position.z>0)
            {

                m_Camera.transform.Translate(-Vector3.forward * 5f * Time.deltaTime, Space.World);

                yield return null;
            }



            m_IsCameraMoving = false;
        }

    }
}
