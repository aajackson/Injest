using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    public float DirectionScale = 1.0f;

    private NavMeshAgent navMeshAgent;


    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            float angle = Random.Range(0.0f, 360.0f);
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
            navMeshAgent.destination = transform.position + direction * DirectionScale;
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Cast a ray from the camera
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(cameraRay, out hitInfo))
            {
                navMeshAgent.destination = hitInfo.point;
            }
        }
    }
}
