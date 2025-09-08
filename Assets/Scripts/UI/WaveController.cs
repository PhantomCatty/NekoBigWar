using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

/*
    波数管理器,记录每一波的配置和对应时间
 */

public class WaveController : MonoBehaviour
{
    public StageData stageData;
    public List<float> TimeNodes;
    public List<WaveItem> Waves;
    
    private float timer;
    private float currentTime;
    private int index;

    //private void Awake()
    //{
    //    stageData = InGameData.instance.stageData;
    //}

    private void Start()
    {
        TimeNodes = stageData.TimeNodes;
        Waves = stageData.Waves;
        index = 0;
        currentTime = TimeNodes[0];
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > currentTime && index < TimeNodes.Count)
        {
            spawnWave(index);
            index++;
            if (index < TimeNodes.Count)
            {
                currentTime = TimeNodes[index];
            }
        }
    }

    private void spawnWave(int index)
    {
        WaveItem temp = Waves[index];
        for(int i = 0; i < temp.Nums.Count; i++)
        {
            for(int j = 0; j < temp.Nums[i]; j++)
            {
                spawnMonsters(temp.Enemys[i],temp.Locations[i].transform.position);
            }
        }
    }

    private void spawnMonsters(EntityType type,Vector3 trans)
    {
        GameObject enemy = ObjectPool.instance.getObject(type);
        float randRadius = UnityEngine.Random.Range(0, 1);
        float randAngle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
        enemy.transform.position = trans + new Vector3(randRadius * Mathf.Cos(randAngle), randRadius * Mathf.Sin(randAngle), 0);
        enemy.GetComponentInChildren<NavMeshAgent>().Warp(enemy.transform.position);
        InfoCanvas.instance.addHPBar(enemy, TargetType.ENEMY);
    }
}

[Serializable]
public struct WaveItem
{
    public List<EntityType> Enemys;
    public List<GameObject> Locations;
    public List<int> Nums;
    
    //public WaveItem()
    //{
    //    Enemys = new List<EntityType>();
    //    Locations = new List<Transform>();
    //    Nums = new List<int>();
    //}
}