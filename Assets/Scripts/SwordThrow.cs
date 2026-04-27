using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThrow : MonoBehaviour
{
    public float speed = 20f;
    public float rotationSpeed = 1000f;
    public float lifetime = 10f;
    public float damage = 10f;

    public Transform model;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        if (model != null)
        {
            model.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider thing)
    {
        Health targetHealth = thing.GetComponent<Health>();
        if (targetHealth != null && !thing.CompareTag("Player"))
        {
            targetHealth.TakeDamage(damage);
        }

        if (!thing.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
