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
        //设置宽度
        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;
        //ps:color should be preset because I havent figured out how to set the color od line
    }

    void Update()
    {
    
    }

    /// <summary>
    /// 画圆圈
    /// </summary>
    /// <param name="transform"></param>//lineRenderer组件
    /// <param name="localPosition"></param>//lineRenderer组件要出生的位置
    /// <param name="attackDistance"></param>//圆圈半径
    public void ToDrawCircle(Vector3 center, float radius)
    {
        int pointAmount = 100;//顶点数量，越多越丝滑
        float eachAngle = 360f / pointAmount;//角度，越小越丝滑
        Vector3 forward = transform.up;//Z轴方向
        lr.positionCount = (pointAmount + 1);//设置顶点【SetVertexCount（pointAmount + 1）弃用】
        for (int i = 0; i <= pointAmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, eachAngle * i) * forward * radius + center;//依次设置点
            lr.SetPosition(i, pos);
        }
    }
}
