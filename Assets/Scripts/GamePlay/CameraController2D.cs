using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController2D : MonoBehaviour
{
    [Header("Cinemachine 虚拟相机")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("边界设置")]
    [Tooltip("包含所有边界碰撞体的父物体（可包含多个子碰撞体）")]
    public GameObject limitCollidersRoot;

    [Header("移动设置")]
    public float dragSpeed = 5f;          // 中键拖拽速度
    public float edgeMoveSpeed = 5f;      // 屏幕边缘移动速度
    public float edgeThreshold = 10f;     // 边缘触发像素

    [Header("快捷键目标（F1~F5）")]
    public Transform[] focusTargets = new Transform[5];

    private Camera mainCam;
    private Transform followTarget;
    private Vector3 dragOrigin;
    private bool isDragging;
    private Collider2D[] limitColliders;

    void Start()
    {
        mainCam = Camera.main;

        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // 确保有 Follow Target
        followTarget = virtualCamera.Follow;
        if (followTarget == null)
        {
            GameObject t = new GameObject("FollowTarget");
            t.transform.position = transform.position;
            followTarget = t.transform;
            virtualCamera.Follow = followTarget;
        }

        // 从父物体收集所有 Collider2D
        if (limitCollidersRoot != null)
            limitColliders = limitCollidersRoot.GetComponentsInChildren<Collider2D>();
    }

    void Update()
    {
        HandleMiddleMouseDrag();
        HandleEdgeMove();
        HandleQuickFocus();
    }

    // 鼠标中键拖拽
    void HandleMiddleMouseDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = mainCam.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(2))
            isDragging = false;

        if (isDragging)
        {
            Vector3 diff = dragOrigin - mainCam.ScreenToWorldPoint(Input.mousePosition);
            MoveFollowTarget(diff);
        }
    }

    // 屏幕边缘移动
    void HandleEdgeMove()
    {
        Vector3 moveDir = Vector3.zero;
        Vector3 m = Input.mousePosition;

        if (m.x <= edgeThreshold) moveDir.x = -1;
        else if (m.x >= Screen.width - edgeThreshold) moveDir.x = 1;

        if (m.y <= edgeThreshold) moveDir.y = -1;
        else if (m.y >= Screen.height - edgeThreshold) moveDir.y = 1;

        if (moveDir != Vector3.zero)
        {
            Vector3 currentPos = followTarget.position;
            Vector3 desiredPos = currentPos + moveDir * edgeMoveSpeed * Time.deltaTime;
            Vector3 clampedPos = ClampToColliders(desiredPos);

            // 检查是否被边界限制
            if (Mathf.Approximately(clampedPos.x, currentPos.x) && moveDir.x != 0)
                moveDir.x = 0;
            if (Mathf.Approximately(clampedPos.y, currentPos.y) && moveDir.y != 0)
                moveDir.y = 0;

            // 重新归一化剩余方向，保证速度不被稀释
            if (moveDir != Vector3.zero)
                moveDir.Normalize();

            followTarget.position = ClampToColliders(currentPos + moveDir * edgeMoveSpeed * Time.deltaTime);
        }
    }

    // 快捷键跳转
    void HandleQuickFocus()
    {
        for (int i = 0; i < focusTargets.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.F1 + i) && focusTargets[i] != null)
            {
                followTarget.position = ClampToColliders(focusTargets[i].position);
            }
        }
    }

    // 移动并限制 Follow Target
    void MoveFollowTarget(Vector3 delta)
    {
        followTarget.position = ClampToColliders(followTarget.position + delta);
    }

    // 按相机可视范围限制位置
    Vector3 ClampToColliders(Vector3 targetPos)
    {
        if (limitColliders != null && limitColliders.Length > 0)
        {
            float halfH = mainCam.orthographicSize;
            float halfW = halfH * mainCam.aspect;

            foreach (var col in limitColliders)
            {
                Bounds b = col.bounds;

                float minX = b.min.x + halfW;
                float maxX = b.max.x - halfW;
                float minY = b.min.y + halfH;
                float maxY = b.max.y - halfH;

                // 如果边界比相机视野还小，直接居中
                float cx = (minX <= maxX) ? Mathf.Clamp(targetPos.x, minX, maxX) : b.center.x;
                float cy = (minY <= maxY) ? Mathf.Clamp(targetPos.y, minY, maxY) : b.center.y;

                return new Vector3(cx, cy, targetPos.z);
            }
        }
        return targetPos;
    }
}