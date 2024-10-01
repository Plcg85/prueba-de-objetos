using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaAtaqueSkrull : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 fuerza = new Vector3(Random.Range(-3,3), -10, Random.Range(-3, 3));
        rb.AddForce(fuerza, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAtack"))
        {
          //Si los ataques chocan entre si no pasa nada
        }
        else 
        {
            //si chocan con cualquier otra cosa se eliminan de la escena
            Destroy(gameObject); 
        } 
    }
}
