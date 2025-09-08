using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerPause : MonoBehaviour
{
    public static UIControllerPause instance;
    public GameObject ButtonResume;
    public GameObject ButtonExit;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }


}
