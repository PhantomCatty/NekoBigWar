
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//指挥官武器，用来管理指挥官的武器，很有可能，指挥官可以在战斗中切换武器
//
public class CommanderWeapon : MonoBehaviour
{
    public UnityEvent<float, float> AmmoEvent;
    //public int weaponNum;//武器列表的容量
    public List<GameObject> weaponPrefabList;//武器列表

    List<WeaponBasic> weaponBasicList;//获取武器的脚本
    List<GameObject> weaponList;
    int weaponIndex;//武器索引，表示当前武器在列表中的位置
    FireController currentFireC;
    public BuffCalculator calculator;

    // Start is called before the first frame update
    void Start()
    {
        calculator = GetComponentInParent<BuffCalculator>();
        weaponList = new List<GameObject>();
        weaponBasicList = new List<WeaponBasic>();
        weaponIndex = 0;
        for(int i = 0; i < 3; i++)
        {
            weaponList.Add(Instantiate(weaponPrefabList[i], transform));
            weaponList[i].GetComponent<FireController>().calculator = calculator;
            weaponBasicList.Add(weaponList[i].GetComponent<WeaponBasic>());
        }
        for(int i = 1; i < 3; i++)
        {
            weaponList[i].SetActive(false);
        }
        currentFireC = weaponList[0].GetComponent<FireController>();
    }

    // Update is called once per frame
    void Update()
    {
        AmmoEvent.Invoke(currentFireC.ammo, currentFireC.weaponBasic.magazing);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeWeapon(2);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentFireC.FireAuto();//现在它是fireAuto,但之后会使用Fire.而Fire会根据武器的type选择对应的函数
        }
    }

    private void changeWeapon(int index)
    {
        if (weaponIndex != index)
        {
            weaponList[weaponIndex].SetActive(false);
            weaponIndex = index;
            weaponList[index].SetActive(true);
            currentFireC = weaponList[index].GetComponent<FireController>();
        }
    }
}
