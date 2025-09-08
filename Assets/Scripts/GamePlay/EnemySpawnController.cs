using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnController : MonoBehaviour
{
    public float basicSpawnSpeed;
    public float spawnSpeedCoefficient;
    public float basicAmountLimit;
    public float AmountLimitCoefficient;

    [HideInInspector]
    public float timer;
    [HideInInspector]
    public static int curAmount;
    [HideInInspector]
    public float curSpawnSpeed;
    [HideInInspector]
    public float spawnTimer;
    [HideInInspector]
    public float curAmountLimit;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = InGameData.instance.player;
    }

    private void OnEnable()
    {
        timer = 0;
        curAmount = 0;
        spawnTimer = 0;
        curAmountLimit = 0;
    }

    // Update is called once per frame
    void Update()
    {
        this.timer += Time.deltaTime;
        setSpawnSpeed();
        spawnTimer += Time.deltaTime * curSpawnSpeed;
        while (spawnTimer >= 1 && curAmount < this.curAmountLimit - 1)
        {
            spawnMonsters();
        }
    }

    private void setSpawnSpeed()
    {
        float speed = basicSpawnSpeed + this.timer * spawnSpeedCoefficient;
        curAmountLimit = this.basicAmountLimit + timer * this.AmountLimitCoefficient;
        float percentage = 1 - curAmount / this.curAmountLimit;
        this.curSpawnSpeed = speed * percentage;
    }

    private void spawnMonsters()
    {
        curAmount++;
        GameObject enemy = ObjectPool.instance.getObject(EntityType.EN_PATROLLER);
        float randRadius = Random.Range(15, 18);
        float randAngle = Random.Range(-Mathf.PI, Mathf.PI);
        enemy.transform.position = player.transform.position + new Vector3(randRadius * Mathf.Cos(randAngle), randRadius * Mathf.Sin(randAngle), 0);
        enemy.GetComponentInChildren<NavMeshAgent>().Warp(enemy.transform.position);
        spawnTimer -= 1;
        InfoCanvas.instance.addHPBar(enemy, TargetType.ENEMY);
    }
}
