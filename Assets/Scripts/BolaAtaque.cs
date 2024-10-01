using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaAtaque : MonoBehaviour
{
    Rigidbody bolaRigidbody;
    private float fuerzaImpulso = 15.0f;
    private float fuerzaImpulsoY = 7.0f;

    Stats stats; //el scripts de stats que esta asociado al player

    private void Awake()
    {
        stats = FindObjectOfType<Stats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bolaRigidbody = GetComponent<Rigidbody>();
        bolaRigidbody.AddForce(transform.forward * fuerzaImpulso, ForceMode.Impulse);
        bolaRigidbody.AddForce(transform.up * fuerzaImpulsoY, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stats.RestarVida();
        }
        if (other.CompareTag("Enemy"))
        {

        }
        else { Destroy(gameObject); } 
        
    }
}
