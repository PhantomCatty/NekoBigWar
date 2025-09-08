using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRange : MonoBehaviour
{
    public Color color;
    private LineRenderer lr;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        //���ÿ��
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
        //ps:color should be preset because I havent figured out how to set the color od line
    }

    void Update()
    {
    
    }

    /// <summary>
    /// ��ԲȦ
    /// </summary>
    /// <param name="transform"></param>//lineRenderer���
    /// <param name="localPosition"></param>//lineRenderer���Ҫ������λ��
    /// <param name="attackDistance"></param>//ԲȦ�뾶
    public void ToDrawCircle(Vector3 center, float radius)
    {
        int pointAmount = 100;//����������Խ��Խ˿��
        float eachAngle = 360f / pointAmount;//�Ƕȣ�ԽСԽ˿��
        Vector3 forward = transform.up;//Z�᷽��
        lr.positionCount = (pointAmount + 1);//���ö��㡾SetVertexCount��pointAmount + 1�����á�
        for (int i = 0; i <= pointAmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, eachAngle * i) * forward * radius + center;//�������õ�
            lr.SetPosition(i, pos);
        }
    }
}
