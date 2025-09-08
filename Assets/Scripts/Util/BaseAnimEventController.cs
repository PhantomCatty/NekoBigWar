using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnimEventController : MonoBehaviour
{
    public virtual void OnAttackAnimationEnd() { }
    public virtual void OnDeathAnimationEnd() { }
}