using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������Ϸ�͹ؿ�����,��Щ����;���gameplay��ϵ����,������ͳ��ؿ���,����˵ʤ����ʧ�ܵĹؿ�ת��
/// �����߼����ܻ�ŵ�InGameData����
/// </summary>
public class InGameController : MonoBehaviour
{
    public static InGameController instance;

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

    }

    //refresh data when enabled
    void OnEnable()
    {
        refresh();
    }

    void refresh()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ʤ��,������Ҫ�Ĳ�������Ϸģʽ����ʲô��Ĳ���
    //public void gameWin()

    //ʧ��,������Ҫ�Ĳ�������Ϸģʽ����ʲô��Ĳ���
    //public void gameFail()

    //������Ϸʱ�䱶��,0��ʾ��ȫ����,ʱ�䲻����Ӱ�쵽���Ͻ�ɫ����Ӱ��ͳ��ʱ��ı���
    //public void changeTimeScale(float multiplier)
    //{
    //
    //}

    public void Pause()
    {
        Time.timeScale = 0f;
        UIControllerPause.instance.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        UIControllerPause.instance.gameObject.SetActive(false);
    }

    public void Slow()
    {
        Time.timeScale = 0.3f;
    }

    public void RecoverSpeed()
    {
        Time.timeScale = 1f;
    }

    /*
        shop method
        when upgraded, implete both shuffle shop and show shop.
        these two logic is separated.
    */
    public void shuffleShop()
    {
        // 获取当前等级
        int curLevel = InGameData.instance.curLevel;
        LevelData levelData = InGameData.instance.levelData;

        // 找到已解锁的组中解锁等级最高的一组
        int maxUnlockedIndex = -1;
        int maxRequirement = -1;
        for (int i = 0; i < levelData.ItemPoolLevelRequirement.Count; i++)
        {
            int requirement = levelData.ItemPoolLevelRequirement[i];
            if (curLevel >= requirement && requirement > maxRequirement)
            {
                maxRequirement = requirement;
                maxUnlockedIndex = i;
            }
        }

        // 如果没有找到可用组，直接返回
        if (maxUnlockedIndex == -1) return;

        // 获取该组的item池
        List<ItemBasic> itemPool = levelData.levelItemPools[maxUnlockedIndex].items;
        List<float> weightPool = levelData.levelItemPools[maxUnlockedIndex].weights;

        // 按权重抽取三个不同的道具
        List<ItemBasic> selectedItems = new List<ItemBasic>();
        List<ItemBasic> poolCopy = new List<ItemBasic>(itemPool);
        List<float> weightCopy = new List<float>(weightPool);
        // 计算总权重
        float totalWeight = 0f;
        foreach (var weight in weightCopy)
        {
            totalWeight += weight;
        }

        for (int pick = 0; pick < 3 && poolCopy.Count > 0; pick++)
        {


            // 随机抽取
            float rand = Random.Range(0, totalWeight);
            float accum = 0f;
            ItemBasic chosen = null;
            int i;
            for (i = 0; i < poolCopy.Count; i++)
            {
                accum += weightCopy[i];
                if (rand <= accum)
                {
                    chosen = poolCopy[i];
                    break;
                }
            }
            if (chosen != null)
            {
                selectedItems.Add(chosen);
                poolCopy.Remove(chosen); // 保证不重复
                weightCopy.RemoveAt(i);
            }
        }

        // 赋值给UIControllerShop的ItemList
        UIControllerShop.instance.ItemList = selectedItems;
    }

    public void showShop()
    {
        UIControllerShop.instance.gameObject.SetActive(true);
    }
}