using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierraSueloTrigger : MonoBehaviour
{
    Stats stats;

    private void Awake()
    {
        stats = FindObjectOfType<Stats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            stats.RestarVida();
        }
    }
}
