using UnityEngine;


public enum ControllerType
{
    Enemy,
    Player,
    Agent,
    Fortification
}

// This class acts as a proxy to forward animation events from the Animator component
// to the Controller script. It automatically finds the Controller in the parent
// hierarchy if not explicitly set.
public class AnimatorEventProxy : MonoBehaviour
{
    public ControllerType controllerType;
    public BaseAnimEventController controller;

    void Awake()
    {
        // 自动查找父物体上的对应Controller
        controller = transform.parent.GetComponentInChildren<BaseAnimEventController>();
    }

    // 供动画事件调用
    public void OnAttackAnimationEnd()
    {
        if (controller != null)
            controller.OnAttackAnimationEnd();
    }

    public void OnDeathAnimationEnd()
    {
        if (controller != null)
            controller.OnDeathAnimationEnd();
    }
}