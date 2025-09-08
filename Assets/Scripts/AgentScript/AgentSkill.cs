using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģ����һ�����и߶�Ȩ�޵����ݷ������ͼ�����
/// �������������ͱ�������,��������Щ��λ�Ǽ��ܵ�ʩ�ŵ�λ(����˵,����ѡ��һ��/ָ�������ĵ�λ,��ѡ�ĵ���ͨ�ü���,��Ҫ�����ﴦ��ѡ���߼�)
/// ����,��λ��ʩ�ŵķ�Χ�˺�(������Ⱦ���������ļ���)�������ﴦ��
/// </summary>
public class AgentSkill : MonoBehaviour
{
    public SkillBasic skillBasic;
    public PassiveSkillBasic passiveSkill;
    // Start is called before the first frame update
    void Start()
    {
        skillBasic = GetComponent<SkillBasic>();
        passiveSkill = GetComponent<PassiveSkillBasic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //������˵�������ûɶ�õ�,�����������е����ͬʱ�Ե��Ҷ���Ч�ļ���,��ȷʵ��Ҫ�������
    public bool compareExertType(SkillExertType aimEnum)
    {
        return (skillBasic.exertType & aimEnum) == aimEnum;
    }

    public void releaseSkill(List<GameObject> tempList)
    {
        skillBasic.targetList = new List<GameObject>(tempList);
        skillBasic.startSkill();
    }

    public void releaseSkill(Vector3 position)
    {
        skillBasic.position = position;
        skillBasic.startSkill();
    }

    /// <summary>
    /// ���ܱ������ķ���
    /// </summary>
    //Ŀ��ѡ����,���ݼ�����������ѡ��Ŀ��
    //self:ֱ��ѡ�Լ�, position:ֱ��ѡλ��, ally/enemy/fortification:ѡ���������ĵ�λ
    //������Ҫһ��detector,��������̫���Ѱ�
    //����!�о����Ҫ�����ܶ�̬��ʾ��ѡ�еĵ�λ,����Ҫ�����ѡ��λ���߼��ŵ�uicontroller����,�ͷ�Χָʾ������,�Ҳ�,�Ҳ�
    //��Ȼ���Լ��Ĳ߻�,���Ҿ�Ҫ�Լ�˵����,�Ҿ���ba���ֶ�̬��ʾ�ķ���̫����,�Ҿ�Ҫ��ô��
    //public void selectTarget()
    //{
    //    if (compareExertType(SkillExertType.SELF))//ѡ�Լ�,ֱ�Ӿ�������
    //    {
    //        TargetList.Add(transform.parent.gameObject);
    //    }
    //    if (compareExertType(SkillExertType.ENEMY))
    //    {
    //        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        vec.z = 0f;
    //        //detector.transform.position = vec;
    //        //detector.GetComponent<AgentAimDetector>();
    //    }
    //}
}
