using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��¼�����Ļ�����Ϣ���������ͣ�������������
//���Ե����ӵ��ű�����˵���������ʹ�õ��������ӵ�
//��firecontroller����

public enum WeaponType
{
    Melee,
    SemiAuto,
    Burst,//���������ÿ�����Ӣ����
    Auto,
    Charge
}

public class WeaponBasic:MonoBehaviour
{
    protected WeaponData weaponData;

    public WeaponType weaponType;
    public float bulletSpeed;
    public float damage;
    public float mDamage;
    public int pierceNum;
    public float fireInterval;//���ι���֮��ļ����firecontroller����һ����ʱ������ʱ�����޲ο����ֵ
    public EntityType bulletType;
    public int magazing;
    public float reloadTime;
    public TargetType targetType;
    public float armorPierce;//����
    public float mArmorPierce;//���ӿ���

    private void Start()
    {
        if (weaponData)
        {
            weaponType = weaponData.weaponType;
            bulletSpeed = weaponData.bulletSpeed;
            damage = weaponData.damage[0];
            mDamage = weaponData.mDamage[0];
            pierceNum = weaponData.pierceNum;
            fireInterval = weaponData.fireInterval[0];
            bulletType = weaponData.bulletType;
            magazing = weaponData.magazing;
            reloadTime = weaponData.reloadTime[0];
            targetType = weaponData.targetType;
            armorPierce = weaponData.armorPierce[0];
            mArmorPierce = weaponData.mArmorPierce[0];
        }
    }

    public void setWeapon(WeaponBasic temp)
    {
        weaponType = temp.weaponType;
        bulletSpeed = temp.bulletSpeed;
        damage = temp.damage;
        pierceNum = temp.pierceNum;
        fireInterval = temp.fireInterval;
        bulletType = temp.bulletType;
        magazing = temp.magazing;
        reloadTime = temp.reloadTime;
        targetType = temp.targetType;
        armorPierce = temp.armorPierce;
    }

    public virtual void setDamage(){}
    public virtual void setRange(){ }
    public virtual void setFireInterval(){}
    public virtual void setArmorPierce(){}
    public virtual void setMoveSpeed(){}
    public virtual void setReloadTime(){}
    public virtual void setHP(){}
}
