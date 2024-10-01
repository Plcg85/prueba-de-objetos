using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PequeniaExplosion : MonoBehaviour
{
    private float tiempoEnDestruir = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruir", tiempoEnDestruir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Destruir() 
    {
        Destroy(gameObject);
    }
}
