using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DamageNum : MonoBehaviour
{
    public TextMeshProUGUI damageLabel;
    public EntityType entityType;
    private void Start()
    {
        //damageLabel = GetComponent<TextMeshProUGUI>();
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void startTween(bool isLeft,float damage)
    {
        damageLabel.text = damage.ToString("#0");

        GameObject thisobj = gameObject;
        Sequence sequence = DOTween.Sequence();
        Vector3 vec = transform.position;
        if (isLeft)//数字向左跳
        {
            sequence.Append(transform.DOMoveX(vec.x-0.5f, 0.3f).SetEase(Ease.Linear));
            sequence.Insert(0, transform.DOMoveY(vec.y + 0.1f, 0.06f).SetEase(Ease.OutCirc));
            sequence.Insert(0.1f, transform.DOMoveY(vec.y - 1.5f, 0.24f).SetEase(Ease.InCirc).OnComplete(() => { ObjectPool.instance.pushObject(thisobj, entityType); }));
        }
        else//数字向右跳
        {
            sequence.Append(transform.DOMoveX(vec.x+0.5f, 0.3f).SetEase(Ease.Linear));
            sequence.Insert(0, transform.DOMoveY(vec.y + 0.1f, 0.06f).SetEase(Ease.OutCirc));
            sequence.Insert(0.1f, transform.DOMoveY(vec.y - 1.5f, 0.24f).SetEase(Ease.InCirc).OnComplete(()=> { ObjectPool.instance.pushObject(thisobj,entityType); }));
        }
    }
}

public enum DamageType{
    physics = 1,
    magic = 2,
    truth = 3,
    health = 4
}