using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tableItem : MonoBehaviour
{
    void OnPress(bool isDown)
    {
        if (isDown)
        {
            if (transform.GetChild(0).GetComponent<UILabel>().text != "잠금")
            {
                int index = System.Convert.ToInt32(transform.GetChild(0).GetComponent<UILabel>().text);
                PlayerPrefs.SetString("LastGame", "Stage1");
                PlayerPrefs.SetInt("LastGameindex", index-1);
                SceneManager.LoadScene("Loding", LoadSceneMode.Single);
               // SceneManager.LoadScene(PlayerPrefs.GetString("LastGame"));
            }
        }
    }
}
