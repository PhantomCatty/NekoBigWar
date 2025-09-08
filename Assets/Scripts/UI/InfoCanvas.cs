using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// InfoCanvas负责显示信息,这些UI组件不会产生交互,他们是只读的
/// </summary>
public class InfoCanvas : MonoBehaviour
{
    public static InfoCanvas instance;

    public Dictionary<Transform, GameObject> FrontSights;//准星和对应的物体的tr
    public Dictionary<Transform, GameObject> HPBars;//血条和对应的物体的tr
    public List<GameObject> DamageNums;//伤害数字,使用dotween,一次性获取tr,不需要保留引用

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        FrontSights = new Dictionary<Transform, GameObject>();
        HPBars = new Dictionary<Transform, GameObject>();
        DamageNums = new List<GameObject>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        refreshBars();
        refreshFrontSight();
    }

    public void addFrontSight(GameObject obj)
    {
        if (FrontSights.ContainsKey(obj.transform)) removeFrontSight(obj);
        FrontSights.Add(obj.transform, ObjectPool.instance.getObject(EntityType.SELECTED_ICON));
        FrontSights[obj.transform].transform.position = obj.transform.position;
    }

    public void addHPBar(GameObject obj,TargetType type)
    {
        if (HPBars.ContainsKey(obj.transform)) removeHPBar(obj);
        if (type == TargetType.BASE)
        {
            //Debug.Log(obj);
            //Debug.Log(obj.tag);
            HPBars.Add(obj.transform, ObjectPool.instance.getObject(EntityType.HP_BAR_B));
            HPBars[obj.transform].transform.position = obj.transform.position + new Vector3(0, 2f, 0);
        }
        else
        {
            HPBars.Add(obj.transform, ObjectPool.instance.getObject(EntityType.HP_BAR));
            HPBars[obj.transform].transform.position = obj.transform.position + new Vector3(0, 0.6f, 0);
        }
    }

    //伤害数字可以有小范围的随机位置
    public void addDamageNum(GameObject obj,float damage, DamageType type)
    {
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float radius = Random.Range(0f, 0.4f);
        int isleft = Random.Range(0, 2);
        Vector3 vec = new Vector3(radius * Mathf.Cos(angle)+0.5f, 0f, 0f);//new Vector3(radius * Mathf.Cos(angle)+0.3f, radius * Mathf.Sin(angle), 0f);
        GameObject temp;
        switch(type){//generate damage number object
            case DamageType.physics:
                temp = ObjectPool.instance.getObject(EntityType.DAMAGE_NUM);
                break;
            case DamageType.magic:
                temp = ObjectPool.instance.getObject(EntityType.DAMAGE_NUM_MAGIC);
                break;
            case DamageType.truth:
                temp = ObjectPool.instance.getObject(EntityType.DAMAGE_NUM_TRUTH);
                break;
            case DamageType.health:
                temp = ObjectPool.instance.getObject(EntityType.HEAL_NUM);
                break;
            default:
                return;
        }
        temp.transform.position = obj.transform.position + vec;
        DamageNum dnum = temp.GetComponent<DamageNum>();
        dnum.startTween(isleft==0, damage);
    }

    private void refreshBars()
    {
        foreach (KeyValuePair<Transform, GameObject> tr in HPBars)
        {
            if(tr.Key.tag=="Ally_Base")
                tr.Value.transform.position = tr.Key.position + new Vector3(0, 2f, 0);
            else 
                tr.Value.transform.position = tr.Key.position + new Vector3(0, 0.6f, 0);
        }
    }

    private void refreshFrontSight()
    {
        foreach (KeyValuePair<Transform, GameObject> tr in FrontSights)
        {
            tr.Value.transform.position = tr.Key.position;
        }
    }

    public void clearFrontSight()
    {
        foreach(KeyValuePair<Transform,GameObject> tr in FrontSights)
        {
            ObjectPool.instance.pushObject(tr.Value,EntityType.SELECTED_ICON);
        }
        FrontSights.Clear();
    }

    public void removeHPBar(GameObject obj)
    {
        if (HPBars.ContainsKey(obj.transform))
        {
            ObjectPool.instance.pushObject(HPBars[obj.transform], EntityType.HP_BAR);
            HPBars.Remove(obj.transform);
        }
    }

    public void removeFrontSight(GameObject obj)
    {
        if (FrontSights.ContainsKey(obj.transform))
        {
            ObjectPool.instance.pushObject(FrontSights[obj.transform], EntityType.SELECTED_ICON);
            FrontSights.Remove(obj.transform);
        }
    }

    public void setHPBar(GameObject obj ,float proportion)
    {
        if(obj && HPBars.ContainsKey(obj.transform))
            HPBars[obj.transform].transform.GetChild(0).GetComponent<Image>().fillAmount = proportion;
    }
}