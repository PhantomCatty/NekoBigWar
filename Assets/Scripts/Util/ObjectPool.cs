using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EntityType
{
    //bullet:1-100
    BU_LONG             =1,
    BU_SHORT            =2,
    BU_SHORT_RED        =3,
    BU_LONG_RED         =4,
    BU_ARROW            =5,
    //enemy 100-200
    EN_PATROLLER             =101,
    EN_JACKHAMMER            =102,
    EN_HOUND                 =103,
    EN_TOWER                 =104,
    //UI 200-300
    DAMAGE_NUM          =201,
    SELECTED_ICON       =202,
    HP_BAR              =203,
    HP_BAR_B            =204,
    DAMAGE_NUM_MAGIC    =205,
    DAMAGE_NUM_TRUTH    =206,
    HEAL_NUM            =207,
}

public class ObjectPool : MonoBehaviour
{
    //����ص���
    public static ObjectPool instance;

    //objectPoolData - scriptable object\
    public ObjectPoolData objectPoolData;
    //Ԥ����
    public GameObject buLongPrefab;
    public GameObject enPatrollerPrefab;
    public GameObject enJackhammerPrefab;
    public GameObject enHoundPrefab;
    public GameObject enTowerPrefab;
    public GameObject buShortPrefab;
    public GameObject buShortRPrefab;
    public GameObject buLongRPrefab;
    public GameObject buArrowPrefab;
    public GameObject damageNumPrefab;
    public GameObject damageNumMagicPrefab;
    public GameObject damageNumTruthPrefab;
    public GameObject healNumPrefab;
    public GameObject selectedIconPrefab;
    public GameObject HPBarPrefab;
    public GameObject HPBarBPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //pool = new Dictionary<EntityType, GameObject>();
        pool = new List<KeyValuePair<EntityType, GameObject>>();
        poolDic = pool.ToLookup(r => r.Key);
        GameObject buLongPrefab = objectPoolData.buLongPrefab;
        GameObject enPatrollerPrefab = objectPoolData.enPatrollerPrefab;
        GameObject enJackhammerPrefab = objectPoolData.enJackhammerPrefab;
        GameObject enHoundPrefab = objectPoolData.enHoundPrefab;
        GameObject enTowerPrefab = objectPoolData.enTowerPrefab;
        GameObject buShortPrefab = objectPoolData.buShortPrefab;
        GameObject buShortRPrefab = objectPoolData.buShortRPrefab;
        GameObject buLongRPrefab = objectPoolData.buLongRPrefab;
        GameObject buArrowPrefab = objectPoolData.buArrowPrefab;
        GameObject damageNumPrefab = objectPoolData.damageNumPrefab;
        GameObject damageNumMagicPrefab = objectPoolData.damageNumMagicPrefab;
        GameObject damageNumTruthPrefab = objectPoolData.damageNumTruthPrefab;
        GameObject healNumPrefab = objectPoolData.healNumPrefab;
        GameObject selectedIconPrefab = objectPoolData.selectedIconPrefab;
        GameObject HPBarPrefab = objectPoolData.HPBarPrefab;
        GameObject HPBarBPrefab = objectPoolData.HPBarBPrefab;
        //buShortList = new Queue<GameObject>();
        //enTestList = new Queue<GameObject>();
        //buLongList = new Queue<GameObject>();
        //selectedIconList = new Queue<GameObject>();
        //HPBarList = new Queue<GameObject>();
        //DamageNumList = new Queue<GameObject>();
    }

    //һ���бȴ���һ���queue���õİ취,��˵����̬����objһ������queue��ʵ��,��ֻ��������,��������
    //private Queue<GameObject> buShortList;
    //private Queue<GameObject> enTestList;
    //private Queue<GameObject> buLongList;
    //private Queue<GameObject> selectedIconList;
    //private Queue<GameObject> HPBarList;
    //private Queue<GameObject> DamageNumList;
    //private Dictionary<EntityType, GameObject> pool;
    private List<KeyValuePair<EntityType, GameObject>> pool;
    ILookup<EntityType, KeyValuePair<EntityType, GameObject>> poolDic;

    public GameObject getObject(EntityType entityType)
    {
        // 检索pool中是否有EntityType匹配的对象
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].Key == entityType && pool[i].Value != null)
            {
                GameObject obj = pool[i].Value;
                pool.RemoveAt(i);
                obj.SetActive(true);
                return obj;
            }
        }
        // 没有可用对象则新建
        return selectObject(entityType);
    }

    private GameObject selectObject(EntityType entityType)
    {
        GameObject temp;
        switch (entityType)
        {
            case EntityType.BU_LONG:
                return Instantiate(buLongPrefab);
            case EntityType.EN_PATROLLER:
                return Instantiate(enPatrollerPrefab);
            case EntityType.EN_JACKHAMMER:
                return Instantiate(enJackhammerPrefab);
            case EntityType.EN_HOUND:
                return Instantiate(enHoundPrefab);
            case EntityType.EN_TOWER:
                return Instantiate(enTowerPrefab);
            case EntityType.BU_SHORT:
                return Instantiate(buShortPrefab);
            case EntityType.BU_SHORT_RED:
                return Instantiate(buShortRPrefab);
            case EntityType.BU_ARROW:
                return Instantiate(buArrowPrefab);
            case EntityType.BU_LONG_RED:
                return Instantiate(buLongRPrefab);
            case EntityType.SELECTED_ICON:
                temp = Instantiate(selectedIconPrefab);
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            case EntityType.HP_BAR:
                temp = Instantiate(HPBarPrefab);
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            case EntityType.HP_BAR_B:
                temp = Instantiate(HPBarBPrefab);
                //temp.transform.parent = InfoCanvas.instance.transform;
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            case EntityType.DAMAGE_NUM:
                temp = Instantiate(damageNumPrefab);
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            case EntityType.DAMAGE_NUM_MAGIC:
                temp = Instantiate(damageNumMagicPrefab);
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            case EntityType.DAMAGE_NUM_TRUTH:
                temp = Instantiate(damageNumTruthPrefab);
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            case EntityType.HEAL_NUM:
                //Debug.Log("cure");
                temp = Instantiate(healNumPrefab);
                temp.transform.SetParent(InfoCanvas.instance.transform, false);
                return temp;
            default:
                return null;
        }
    }

    //public GameObject getObject(EntityType entityType)
    //{
    //    switch (entityType)
    //    {
    //        case (EntityType.BU_LONG):
    //            {
    //                if (this.buLongList.Count == 0)
    //                {
    //                    return Instantiate(this.buLongPrefab);
    //                }
    //
    //                else
    //                {
    //                    GameObject temp = buLongList.Dequeue();
    //                    temp.SetActive(true);
    //                    return temp;
    //                }
    //            }
    //        case (EntityType.EN_PATROLLER):
    //            {
    //                if (this.enTestList.Count == 0)
    //                    return Instantiate(this.enPatrollerPrefab);
    //                else
    //                {
    //                    GameObject temp = enTestList.Dequeue();
    //                    temp.SetActive(true);
    //                    return temp;
    //                }
    //            }
    //        case (EntityType.BU_SHORT):
    //            {
    //                if (this.buShortList.Count == 0)
    //                    return Instantiate(this.buShortPrefab);
    //                else
    //                {
    //                    GameObject temp = buShortList.Dequeue();
    //                    temp.SetActive(true);
    //                    return temp;
    //                }
    //            }
    //        case (EntityType.SELECTED_ICON):
    //            {
    //                if (selectedIconList.Count == 0)
    //                {
    //                    GameObject temp = Instantiate(selectedIconPrefab);
    //                    temp.transform.parent = InfoCanvas.instance.transform;
    //                    return temp;
    //                }
    //                else
    //                {
    //                    GameObject temp = selectedIconList.Dequeue();
    //                    temp.SetActive(true);
    //                    return temp;
    //                }
    //            }
    //        case (EntityType.HP_BAR):
    //            {
    //                if (HPBarList.Count == 0)
    //                {
    //                    GameObject temp = Instantiate(HPBarPrefab);
    //                    temp.transform.parent = InfoCanvas.instance.transform;
    //                    return temp;
    //                }
    //                else
    //                {
    //                    GameObject temp = HPBarList.Dequeue();
    //                    temp.SetActive(true);
    //                    return temp;
    //                }
    //            }
    //        case (EntityType.DAMAGE_NUM):
    //            {
    //                if (DamageNumList.Count == 0)
    //                {
    //                    GameObject temp = Instantiate(damageNumPrefab);
    //                    temp.transform.parent = InfoCanvas.instance.transform;
    //                    return temp;
    //                }
    //                else
    //                {
    //                    GameObject temp = DamageNumList.Dequeue();
    //                    temp.SetActive(true);
    //                    return temp;
    //                }
    //            }
    //        default:
    //            {
    //                return null;
    //            }
    //    }
    //
    //}

    public void pushObject( GameObject obj, EntityType entityType)
    {
        obj.SetActive(false);
        pool.Add(new KeyValuePair<EntityType, GameObject>(entityType,obj));
        return;
        //switch (entityType)
        //{
        //    case (EntityType.BU_SHORT):
        //        {
        //            obj.SetActive(false);
        //            buShortList.Enqueue(obj);
        //            return;
        //        }
        //    case (EntityType.EN_PATROLLER):
        //        {
        //            obj.SetActive(false);
        //            enTestList.Enqueue(obj);
        //            return;
        //        }
        //    case (EntityType.BU_LONG):
        //        {
        //            obj.SetActive(false);
        //            buLongList.Enqueue(obj);
        //            return;
        //        }
        //    case (EntityType.SELECTED_ICON):
        //        {
        //            obj.SetActive(false);
        //            selectedIconList.Enqueue(obj);
        //            return;
        //        }
        //    case (EntityType.HP_BAR):
        //        {
        //            obj.SetActive(false);
        //            HPBarList.Enqueue(obj);
        //            return;
        //        }
        //    case (EntityType.DAMAGE_NUM):
        //        {
        //            obj.SetActive(false);
        //            DamageNumList.Enqueue(obj);
        //            return;
        //        }
        //    default:
        //        {
        //            return;
        //        }
        //}
    }
}
