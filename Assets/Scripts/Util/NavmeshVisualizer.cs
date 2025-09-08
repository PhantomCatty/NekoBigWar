using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavmeshVisualizer : MonoBehaviour
{
    private NavMeshAgent agent;
    public LineRenderer lineRenderer;

    public bool visualizeVelocity;
    public bool visualizePath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //initialization for path visualizer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = 0;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

    }

    void Update()
    {
        // for velocity visualizer
        if (visualizeVelocity && agent.velocity.sqrMagnitude > 0.01f)
        {
            Debug.DrawLine(
                agent.transform.position,
                agent.transform.position + agent.velocity.normalized * 2f,
                Color.red
            );
        }

        //for path visualizer
        if (visualizePath)
        {
            if (agent.hasPath)
            {
                var corners = agent.path.corners;
                lineRenderer.positionCount = corners.Length;
                for (int i = 0; i < corners.Length; i++)
                {
                    lineRenderer.SetPosition(i, corners[i]);
                }
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }

    }
}