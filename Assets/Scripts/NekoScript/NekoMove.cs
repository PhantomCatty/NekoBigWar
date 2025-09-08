using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.Events;


/*
 ����ű�����ʵ�ֽ�ɫ�Ļ����ƶ��ͽ�ɫ���֣�����˵�����ƶ��Ķ�����������ת��ָ���λ�á�
 */
public class NekoMove : MonoBehaviour
{
    Animator anim;
    public float maxVelocity;
    public float currVelocity;
    public float acceleration;

    [HideInInspector]
    public BuffCalculator calculator;

    public bool isleft;
    float horizontal;
    float vertical;
    Transform weapontr;

    private float hurtTimer;
    private NekoBasic nekoBasic;
    private Vector3 mousepos;

    public UnityEvent<float, float> HPEvent;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        calculator = GetComponent<BuffCalculator>();
        hurtTimer = 0f;
        weapontr = transform.Find("WeaponPlace").GetComponent<Transform>();
        nekoBasic = GetComponent<NekoBasic>();
        nekoBasic.currentHp = nekoBasic.maxHP;
        mousepos = new Vector3();
    }

    void Update()
    {
        if (hurtTimer < 0.1f) hurtTimer += Time.deltaTime;
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = 0;
        move(mousepos);
        aim(mousepos);
    }

    void move(Vector3 mousepos)
    {
        isleft = mousepos.x < transform.position.x;
        if (isleft)//�������ߡ�����
        {
            if (transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else//�ұ�
        {
            if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            MoveCharactor(new Vector3(horizontal, vertical, 0f));
            anim.SetFloat("direction", horizontal);
        }
        else
        {
            anim.SetFloat("direction", horizontal);
            anim.SetFloat("velocity", 0f);
        }
    }

    void aim(Vector3 mousepos)
    {
        if (!isleft)
            weapontr.right = mousepos - weapontr.position;
        else weapontr.right = weapontr.position - mousepos;
    }


    void MoveCharactor(Vector3 direction)
    {
        transform.parent.Translate(direction.normalized * maxVelocity * Time.deltaTime);
        anim.SetFloat("velocity", maxVelocity);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Enemy"&&hurtTimer>0.5f)
    //    {
    //        hurtTimer = 0f;
    //        nekoBasic.currentHp -= collision.GetComponentInChildren<EnemyBasic>().damage;
    //        Debug.Log(nekoBasic.currentHp);
    //    }
    //}

    public void hurt(float damage, float mDamage, float armorPierce=0, float mArmorPierce=0)
    {
        if (damage <= 0f && mDamage <= 0f)//����
        {
            nekoBasic.currentHp -= calculator.calculateFloat(damage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            nekoBasic.currentHp -= calculator.calculateFloat(mDamage, new CalculateItem[] { CalculateItem.HEAL_MULTIPLIER, CalculateItem.HEAL_FINALM });
            if (nekoBasic.currentHp >= nekoBasic.maxHP) nekoBasic.currentHp = nekoBasic.maxHP;
            HPEvent.Invoke(nekoBasic.currentHp, nekoBasic.maxHP);
        }
        else
        {
            if (hurtTimer > 0f)
            {
                hurtTimer = 0f;
                nekoBasic.currentHp -= damage + mDamage;
                HPEvent.Invoke(nekoBasic.currentHp, nekoBasic.maxHP);
                if (nekoBasic.currentHp <= 0f) playerDie();
            }
        }
    }

    private void playerDie()
    {
        InGameData.instance.winCondition.minusCondition(GameCondition.PLAYER_DIES, 1);
    }
}
