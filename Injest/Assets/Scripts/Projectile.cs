using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidBody;
    private float duration;
    [SerializeField]
    private Renderer projectileRenderer;

    public float speed = 10.0f;

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(float timeToLive)
    {
        duration = timeToLive;
        rigidBody.velocity = transform.forward * speed;
        gameObject.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {

        Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        if (projectileRenderer != null)
        {
            projectileRenderer.material.color = color;
        }
    }
}
