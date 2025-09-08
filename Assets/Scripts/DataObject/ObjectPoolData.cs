using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolData", menuName = "ScriptableObject/ObjectPoolData", order = 0)]
public class ObjectPoolData : ScriptableObject
{
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
}