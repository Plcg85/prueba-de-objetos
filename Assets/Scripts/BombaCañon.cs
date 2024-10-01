using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaCañon : MonoBehaviour
{
    Rigidbody bombaRigidbody;
    private float fuerzaImpulso = 20.0f;
    private float fuerzaImpulsoY = 15.0f;
    private float tiempoDestruirBomba = 3.0f;
    private float velociadRotacion = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        bombaRigidbody = GetComponent<Rigidbody>();
        transform.Rotate(-90, 90, 0);
        bombaRigidbody.AddForce(transform.forward * fuerzaImpulso, ForceMode.Impulse);
        bombaRigidbody.AddForce(transform.up * fuerzaImpulsoY, ForceMode.Impulse);
        Invoke("DestruirBomba", tiempoDestruirBomba);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(velociadRotacion * Time.deltaTime, 2,3);
    }
    void DestruirBomba() 
    {
        Destroy(gameObject);
    }
}
