using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour
{
    public static LoseController Instance;

    float timer = 5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.panik)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Classes();
            }
        }
    }

    public void Panic()
    {
        GlobalVars.panik = true;
        //Debug.Log("have paniked");
        SceneManager.LoadScene(4);
    }

    public void Classes()
    {
        SceneManager.LoadScene(3);
    }

    public void Party()
    {
        GameObject.Find("Main Camera").GetComponent<ThemesChangeTrigger>().Off();
        SceneManager.LoadScene(1);
    }
}
