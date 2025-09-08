
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//ָ�ӹ���������������ָ�ӹٵ����������п��ܣ�ָ�ӹٿ�����ս�����л�����
//
public class CommanderWeapon : MonoBehaviour
{
    public UnityEvent<float, float> AmmoEvent;
    //public int weaponNum;//�����б������
    public List<GameObject> weaponPrefabList;//�����б�

    List<WeaponBasic> weaponBasicList;//��ȡ�����Ľű�
    List<GameObject> weaponList;
    int weaponIndex;//������������ʾ��ǰ�������б��е�λ��
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
            currentFireC.FireAuto();//��������fireAuto,��֮���ʹ��Fire.��Fire�����������typeѡ���Ӧ�ĺ���
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
