using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 ????????????????????????????????????????????????????
 ??????????????????????��????weaponbasic???????????????????????????????
 */

public class BulletBasic : MonoBehaviour
{
    public GameObject source;
    public float bulletSpeed;
    public int pierceNum;
    public float damage;
    public float mDamage;
    public float armorPierce;
    public float mArmorPierce;
    public EntityType entityType;
    //public BulletSource bulletSource;
    public TargetType targetType;
    public GameObject target;

    public int currentPierce;

    private float timer;
    private float colliTimer;
    //ContactFilter2D cFilter;
    //Collider2D[] result;

    //prevent duplicated damage calculation
    private int currentFrame = -1;
    private List<Collider2D> hitBuffer = new List<Collider2D>();


    //FireController????
    //?��???,???????????????????,??????????????????,???,?????????????????????
    public void setBulletInfo(WeaponBasic weaponBasic)
    {
        bulletSpeed = weaponBasic.bulletSpeed;
        pierceNum = weaponBasic.pierceNum;
        damage = weaponBasic.damage;
        mDamage = weaponBasic.mDamage;
        targetType = weaponBasic.targetType;
        currentPierce = pierceNum;
        armorPierce = weaponBasic.armorPierce;
        mArmorPierce = weaponBasic.mArmorPierce;
    }

    public void setBulletTarget(GameObject obj)
    {
        target = obj;
    }

    public void setBulletSource(GameObject obj)
    {
        source = obj;
    }

    private void OnEnable()
    {
        timer = 0f;
        colliTimer = 0.1f;
        currentPierce = pierceNum;
    }

    private void Start()
    {
        //cFilter = new ContactFilter2D();
        //cFilter.layerMask = LayerMask.GetMask("Enemy");
        //result = new Collider2D[99];
    }

    private void Update()
    {
        if (colliTimer < 0.1f) colliTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 4f)
        {
            ObjectPool.instance.pushObject(this.gameObject, entityType);
        }
        transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
    }

    //I have always been wrong for these years.If I need realize that obj damaged when contact the bullet
    //I should let the bullet script call functions that tackle the health of obj in those objs
    //???????
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果已经达到上限，直接 return
        if (currentPierce <= 0) return;

        int frame = Time.frameCount;
        if (currentFrame != frame)
        {
            // 新的一帧，清空缓存
            currentFrame = frame;
            hitBuffer.Clear();
        }

        // 这一帧重复碰撞同一目标不计数
        if (hitBuffer.Contains(other)) return;
        hitBuffer.Add(other);


        if (CommonTools.compareTarget(targetType, TargetType.TARGET))//���Ŀ��������"Ŀ��"����ֻ��Ŀ������˺�
        {
            if (other.gameObject != target) return;
        }
        if (other.tag == "Enemy" && CommonTools.compareTarget(targetType, TargetType.ENEMY))//���Ŀ��������"����"����ֻ�Ե�������˺�
        {
            other.GetComponentInChildren<EnemyController>().hurt(damage, mDamage, armorPierce, mArmorPierce);
            currentPierce--;
            if (currentPierce <= 0) ObjectPool.instance.pushObject(gameObject, entityType);
        }
        if (CommonTools.compareTarget(targetType, TargetType.ALLY))//���Ŀ��������"�Ѿ�"����ֻ���Ѿ�����˺�
        {
            if (other.tag == "Player")
            {
                other.GetComponentInChildren<NekoMove>().hurt(damage, mDamage, armorPierce, mArmorPierce);
                currentPierce--;
                if (currentPierce <= 0) ObjectPool.instance.pushObject(gameObject, entityType);
            }
            else if (other.tag == "Agent")
            {
                other.GetComponentInChildren<AgentController>().hurt(damage, mDamage, armorPierce, mArmorPierce);
                currentPierce--;
                if (currentPierce <= 0) ObjectPool.instance.pushObject(gameObject, entityType);
            }
        }
        if ((other.tag == "Ally_Fortification"|| other.tag == "Ally_Base") && CommonTools.compareTarget(targetType, TargetType.ALLY_FORTIFICATION))//���Ŀ��������"�Ѿ�����"����ֻ���Ѿ���������˺�
        {
            other.GetComponentInChildren<BaseController>().hurt(damage, mDamage, armorPierce, mArmorPierce);
            currentPierce--;
            if (currentPierce <= 0) ObjectPool.instance.pushObject(gameObject, entityType);
        }
    }
}

//public enum BulletSource
//{
//    ENEMY = 0,
//    ALLY = 1,
//    PLAYER = 2,
//    FORTIFICATION = 3
//}