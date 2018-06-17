using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {
    private MemoryPull m_Memory;

    private List<GameObject> m_listObj = new List<GameObject>();

    public PlayerAnimate m_Player;

    private void Awake()
    {
        m_Memory = MemoryPull.instance;
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimate>();
    }

    public void init(Vector3 Startposition , Vector3 EndPosition)
    {
        float Length = EndPosition.z - Startposition.z;
        Length = Length / 10;

        float[] position = { -1.5f, 0.0f, 1.5f };

        int count = 0;
        for (int i =0; i<10;i++)
        {
            if(Random.Range(0,1000) > 850 )
            {
                GameObject coin = m_Memory.getObj("USD");
                count++;
                coin.transform.position = new Vector3(position[Random.Range(0, 3)], Random.Range(0.4f, 0.7f), Startposition.z +(Length * i));
                coin.GetComponent<Coin>().init();

                if (count == 5)
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_Player.Test();
       // Debug.Log("빠짐");
    }



    public void Remove()
    {
        foreach (var v in m_listObj)
        {
            v.gameObject.SetActive(false);
        }
    }

}
