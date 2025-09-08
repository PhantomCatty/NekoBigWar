using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������Ϸ�͹ؿ�����,��Щ����;���gameplay��ϵ����,������ͳ��ؿ���,����˵ʤ����ʧ�ܵĹؿ�ת��
/// �����߼����ܻ�ŵ�InGameData����
/// </summary>
public class InGameController : MonoBehaviour
{
    public static InGameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ʤ��,������Ҫ�Ĳ�������Ϸģʽ����ʲô��Ĳ���
    //public void gameWin()

    //ʧ��,������Ҫ�Ĳ�������Ϸģʽ����ʲô��Ĳ���
    //public void gameFail()

    //������Ϸʱ�䱶��,0��ʾ��ȫ����,ʱ�䲻����Ӱ�쵽���Ͻ�ɫ����Ӱ��ͳ��ʱ��ı���
    public void changeTimeScale(float multiplier)
    {

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        UIControllerPause.instance.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        UIControllerPause.instance.gameObject.SetActive(false);
    }

    public void Slow()
    {
        Time.timeScale = 0.3f;
    }

    public void RecoverSpeed()
    {
        Time.timeScale = 1f;
    }
}