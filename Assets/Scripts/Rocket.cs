using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 1f;
    public float lifetime = 20f;
    public float damage = 20f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider thing)
    {
        Health targetHealth = thing.GetComponent<Health>();
        if (targetHealth != null && !thing.CompareTag("Boss"))
        {
            targetHealth.TakeDamage(damage);
        }

        if (!thing.CompareTag("Boss"))
        {
            Debug.Log("Touched!");
            Destroy(gameObject);
        }
    }
}
