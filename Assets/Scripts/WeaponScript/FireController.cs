using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireController : MonoBehaviour
{
    float fireTimer;
    float reloadTimer;
    [HideInInspector]
    public WeaponBasic weaponBasic;
    GameObject bulletEffect;
    public int ammo;
    bool reloading;
    Transform firetrans;
    public UnityEvent<GameObject> fireEvent;
    public BuffCalculator calculator;

    public GameObject burstEffectPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        calculator = GetComponentInParent<BuffCalculator>();
        fireTimer = 10f;
        reloadTimer = 0;
        weaponBasic = transform.parent.GetComponentInParent<WeaponBasic>();
        if (!weaponBasic)
        {
            weaponBasic = GetComponent<WeaponBasic>();
        }
        ammo = weaponBasic.magazing;
        reloading = false;
        firetrans = transform.GetChild(0);
        bulletEffect = Instantiate(burstEffectPrefab, firetrans);
        bulletEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (fireTimer < weaponBasic.fireInterval) fireTimer += Time.deltaTime;
        if (fireTimer > 0.1f) bulletEffect.SetActive(false);

        if (ammo <= 0 && reloading == false) { reloading = true; reloadTimer = 0f; }

        if (reloadTimer < weaponBasic.reloadTime) reloadTimer += Time.deltaTime;
        else if (ammo <= 0)
        {

            ammo = weaponBasic.magazing;
            reloading = false;
        }
    }

    public void FireAuto(GameObject target = null)
    {
        if (!reloading && fireTimer >= weaponBasic.fireInterval)
        {
            GameObject temp = ObjectPool.instance.getObject(weaponBasic.bulletType);
            fireTimer = 0;
            if (transform.lossyScale.x > 0)
            {
                temp.transform.position = firetrans.position;
                temp.transform.rotation = transform.rotation;
                temp.GetComponent<BulletBasic>().setBulletInfo(weaponBasic);
                if (CommonTools.compareTarget(weaponBasic.targetType, TargetType.TARGET)) temp.GetComponent<BulletBasic>().setBulletTarget(target);
            }
            else
            {
                temp.transform.position = firetrans.position;
                temp.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 180));
                temp.GetComponent<BulletBasic>().setBulletInfo(weaponBasic);
                if (CommonTools.compareTarget(weaponBasic.targetType, TargetType.TARGET)) temp.GetComponent<BulletBasic>().setBulletTarget(target);
            }
            bulletEffect.SetActive(true);
            ammo--;
            fireEvent.Invoke(temp);
        }
    }

    public void freeAmmo()
    {
        float? temp = calculator.getItem(CalculateItem.FREE_AMMO);
        if (temp != null && Random.value < temp)
        {
            ammo++;
        }
    }
    
    /// <summary>
    /// 判断当前是否可以开火（未在换弹且冷却结束且有子弹）
    /// </summary>
    public bool CanFire()
    {
        return !reloading && fireTimer >= weaponBasic.fireInterval && ammo > 0;
    }
}
