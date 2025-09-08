using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData", order = 0)]
public class StageData : ScriptableObject
{
    public GameMode mode;
    public float ALLY_DIES;
    public float TIME;
    public float ENEMY_BASE;
    public float ALLY_BASE;
    public float ENEMY_DIES;
    public float PLAYER_DIES;
    public float ALLY_SCORE;
    public float ENEMY_SCORE;

    public List<float> TimeNodes;
    public List<WaveItem> Waves; 
}