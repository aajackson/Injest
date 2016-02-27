using UnityEngine;
using System.Collections;

public class InteractionTarget : MonoBehaviour
{
    public GameObject ItemDropPrefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        // Do a thing!
        if (other.GetComponent<Projectile>() != null)
        {
            Instantiate(ItemDropPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
}
