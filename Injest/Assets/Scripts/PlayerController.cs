using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5.0f;
    public Projectile ItemPrefab = null;
    public float FiringDelay = 0.75f;

    private Transform cameraTransform;
    private float nextFireTime = 0.0f;
    private List<InventoryItem> inventory = new List<InventoryItem>();

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Fire!
        if (Input.GetButtonDown("Fire1") && nextFireTime <= Time.time)
        {
            nextFireTime = Time.time + FiringDelay;
            Projectile newProjectile = Instantiate(ItemPrefab, transform.position + transform.forward * 2.0f, transform.rotation) as Projectile;
            newProjectile.Launch(10.0f);
        }
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
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
        if ((h != 0.0f || v != 0.0f))// && playerMove.sqrMagnitude > 0.0f)
        {
            transform.forward = playerMove;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null)
        {
            // Give the player this item!
            if (pickupItem.inventoryItem != null)
            {
                inventory.Add(pickupItem.inventoryItem);
            }
            Destroy(pickupItem.gameObject);
        }
    }

}
