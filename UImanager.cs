using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UImanager : MonoBehaviour
{
    private int m_PlayerLife;
    private int m_CoinCount;
    private PlayerAnimate m_PalyerAnimate;

    public GameObject[] m_ObjPlayerLife = new GameObject[3];

    public GameObject m_Player;
    public UILabel m_CoinLabel;
    public GameObject m_GameOverLabel;

	void Start ()
    {
        m_PlayerLife = 3;
        m_CoinCount = 0;
        m_PalyerAnimate = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimate>();
    }

	void Update ()
    {
		
	}

    public void SubLife()
    {
        m_PlayerLife--;
        if(m_PlayerLife>=0)
            m_ObjPlayerLife[m_PlayerLife].GetComponent<TweenAlpha>().enabled = true;

        if (m_PlayerLife == 0)
            m_PalyerAnimate.GameEnd();
    }

    public void AddCoin()
    {
        m_CoinCount++;
        m_CoinLabel.text =string.Format("{0}", m_CoinCount);
    }

    public void GameOver()
    {
        m_GameOverLabel.SetActive(true);
        Invoke("GameEndScene", 5.0f);
    }

    public void GameEndScene()
    {
        PlayerPrefs.SetInt("Coin", m_CoinCount);
        if(m_PlayerLife == 0)
            PlayerPrefs.SetInt("GameStaete", 0);
        else
            PlayerPrefs.SetInt("GameStaete", 1);

        SceneManager.LoadScene("GameEnd", LoadSceneMode.Single);
        

    }
}
