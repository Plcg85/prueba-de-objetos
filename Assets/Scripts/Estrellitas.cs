using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estrellitas : MonoBehaviour
{
    //Este script controla el movimiento de las estrellitas de los enemigos

    private float velocidadRotacion = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,velocidadRotacion,0);
    }
}
