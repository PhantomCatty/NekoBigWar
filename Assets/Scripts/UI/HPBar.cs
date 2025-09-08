using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }
    void OnEnable()
    {
        transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
    }
}
