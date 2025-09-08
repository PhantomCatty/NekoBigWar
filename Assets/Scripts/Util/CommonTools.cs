using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonTools : MonoBehaviour
{
    public static bool compareTarget(TargetType curEnum, TargetType aimEnum)
    {
        return (curEnum & aimEnum) == aimEnum;
    }
}
