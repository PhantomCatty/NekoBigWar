using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// include the statistics and data/model of the game system in a level,
/// this is the data not the data source,
/// when start a level ,this model read initial data from data source
/// </summary>
public class InGameData : MonoBehaviour
{
    public static InGameData instance;
    
    public int initialCost;
    public float costSpeed;
    public float curCost;
    public float curExperience;
    public float totalExperience;
    public float levelExperience;
    public float remainingExperience;
    public float curLevel;
    public GameMode gameMode;
    public int kills;
    public int totalEnemyNum;
    public int totalAgentDies;
    public GameWinCondition winCondition;
    public StageData stageData;

    public float totalDamage;
    public int[] agentKills;
    public int[] agentDies;
    public float[] agentDamage; 

    private float costRefreshTimer;
    public float gameTimer;

    //global reference
    public GameObject player;
    public List<GameObject> allyBaseList;
    public List<GameObject> enemyBaseList;
    public AgentSkill[] agentSkillList;
    public GameObject[] agentList;

    public UnityEvent<float> CostEvent;
    public UnityEvent<int> KillsEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
        player = GameObject.Find("OriginNeko");
    }

    // Start is called before the first frame update
    void Start()
    {
        kills = 0;
        agentList = new GameObject[8];
        agentSkillList = new AgentSkill[8];
        costRefreshTimer = 0f;
        gameTimer = 0f;
        int i = 1;
        while (true)
        {
            GameObject temp = GameObject.Find("Base" + i);
            if (temp == null) break;
            else allyBaseList.Add(temp);
        }
        i = 1;
        while (true)
        {
            GameObject temp = GameObject.Find("EnemyBase" + i);
            if (temp == null) break;
            else allyBaseList.Add(temp);
        }
        curCost = initialCost;
        winCondition = new GameWinCondition(stageData);
        winCondition.mode = gameMode;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (costRefreshTimer < 0.3f) costRefreshTimer += Time.deltaTime;
        else
        {
            costRefreshTimer = 0f;
            CostEvent.Invoke(curCost);
        }
        gameTimer += Time.deltaTime;
        curCost += Time.deltaTime * costSpeed;
    }

    /// <summary>
    /// both add and subtract use the same function because param can be minus
    /// add cost will simultaneously set the UI but it is wrong to write it in addCost but not in an individual controller class
    /// but I an doing the wrong way.
    /// </summary>
    /// <param name="cost"></param>
    public void addCost(float cost)
    {
        curCost += cost;
        CostEvent.Invoke(curCost);
    }

    public void addKills()
    {
        kills++;
        KillsEvent.Invoke(kills);
        winCondition.minusCondition(GameCondition.ENEMY_DIES, 1);
    }
}

public enum GameMode
{

    DEFENSIVE_WARFARE=0,//����ս
    STORM_FORTIFICATION=1,//����ս
    ENCOUNTER=2,//����ս
    CTF=3//Capture the Flag����ս,����ȷ������淨�Ƿ����
}

//�����ؿ������Ľṹ��,��������ʱ��,���ֵ������������
public struct Wave
{
    //��һ�����ֵ�ʱ��
    public float _time;
}

//��Ϸʤ������
public class GameWinCondition
{
    public GameMode mode;
    public float[] conditionMap;//ʧ������������,��¼ÿ�������Ͷ�Ӧ����ֵ,��ֵ���������仯����С,��������һ���ӿ�,��С��0�Զ�����set����

    //����,Ҫ�����԰�
    private bool _enemyAllDie;
    private bool _allyBaseAllDown;
    private bool _enemyBaseAllDown;
    private bool _playerDie;
    private bool _timeOut;
    private bool _allyAllDie;
    private bool _allyFlagDown;
    private bool _enemyFlagDown;

    public GameWinCondition(StageData data)
    {
        _enemyAllDie = _allyBaseAllDown = _enemyBaseAllDown = _playerDie = _timeOut = _allyAllDie = _allyFlagDown = _enemyFlagDown = false;
        conditionMap = new float[8];
        setData(data);
    }

    public void setData(StageData data)
    {
        mode = data.mode;
        conditionMap[0] = data.ALLY_DIES;
        conditionMap[1] = data.TIME;
        conditionMap[2] = data.ENEMY_BASE;
        conditionMap[3] = data.ALLY_BASE;
        conditionMap[4] = data.ENEMY_DIES;
        conditionMap[5] = data.PLAYER_DIES;
        conditionMap[6] = data.ALLY_SCORE;
        conditionMap[7] = data.ENEMY_SCORE;
    }

    public void minusCondition(GameCondition condition,float value)
    {
        conditionMap[condition.GetHashCode()] -= value;
        if (conditionMap[condition.GetHashCode()] <= 0f)
        {
            switch (condition)
            {
                case GameCondition.ALLY_DIES:
                    allyAllDie = true;
                    break;
                case GameCondition.ALLY_BASE:
                    allyBaseAllDown = true;
                    break;
                case GameCondition.ENEMY_BASE:
                    enemyBaseAllDown = true;
                    break;
                case GameCondition.ALLY_SCORE:
                    allyFlagDown = true;
                    break;
                case GameCondition.ENEMY_DIES:
                    enemyAllDie = true;
                    break;
                case GameCondition.ENEMY_SCORE:
                    enemyFlagDown = true;
                    break;
                case GameCondition.PLAYER_DIES:
                    playerDie = true;
                    break;
                case GameCondition.TIME:
                    timeOut = true;
                    break;
                default:break;
            }
        }
    }
    public bool enemyAllDie
    {
        get { return _enemyAllDie; }
        set { _enemyAllDie = value; judgeWin(mode); }
    }
    public bool allyBaseAllDown
    {
        get { return _allyBaseAllDown; }
        set { _allyBaseAllDown = value;judgeWin(mode); }
    }
    public bool enemyBaseAllDown
    {
        get { return _enemyBaseAllDown; }
        set { _enemyBaseAllDown = value; judgeWin(mode); }
    }
    public bool playerDie
    {
        get { return _playerDie; }
        set { _playerDie = value;judgeWin(mode); }
    }
    public bool timeOut
    {
        get { return _timeOut; }
        set { _timeOut = value; judgeWin(mode); }
    }
    public bool allyAllDie
    {
        get { return _allyAllDie; }
        set { _allyAllDie = value; judgeWin(mode); }
    }
    public bool allyFlagDown
    {
        get { return _allyFlagDown; }
        set { _allyFlagDown = value;judgeWin(mode); }
    }
    public bool enemyFlagDown
    {
        get { return _enemyFlagDown; }
        set { _enemyFlagDown = value; judgeWin(mode); }
    }

    public void judgeWin(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.DEFENSIVE_WARFARE:
            {
                if (allyBaseAllDown || playerDie)
                {
                    gameFail();
                }
                if (enemyAllDie || timeOut) 
                {
                    gameWin();
                }
                break;
            }
            case GameMode.STORM_FORTIFICATION:
            {
                if (timeOut||playerDie)
                {
                    gameFail();
                }
                if (enemyBaseAllDown)
                {
                    gameWin();
                }
                break;
            }
            case GameMode.ENCOUNTER:
            {
                if (allyAllDie || playerDie)
                {
                    gameFail();
                }
                if (enemyAllDie)
                {
                        gameWin();
                }
                break;
            }
            default:break;
        }
    }

    private void gameWin()
    {
        Debug.Log("win");
        Time.timeScale = 0f;
    }
    private void gameFail()
    {
        Debug.Log("fail");
        Time.timeScale = 0f;
    }
}

public enum GameCondition
{
    ALLY_DIES=0,
    TIME=1,
    ENEMY_BASE=2,
    ALLY_BASE=3,
    ENEMY_DIES=4,
    PLAYER_DIES=5,
    ALLY_SCORE=6,
    ENEMY_SCORE=7
}