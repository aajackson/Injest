using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        // Inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate direction to move the character
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = cameraTransform.right;
        Vector3 playerMove = (v * cameraForward + h * cameraRight);
        if (playerMove.sqrMagnitude > 1.0f)
        {
            playerMove.Normalize();
        }

        // TODO: Project onto ground plane for slopes
        transform.position += playerMove * MoveSpeed * Time.fixedDeltaTime;

    }
}
