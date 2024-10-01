using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SierraSuelo : MonoBehaviour
{
    private Animator animador;
    public GameObject player;
    private AudioManagerSierra audioManager;
    private int sonidoSierra = 0;
    private float volumenSonidoSierra = 0.5f;
    private bool sonandoSierra = false;
    private float distanciaSonidoSierra = 25.0f;
    private float tiempoReseteoSonidoSierra = 5.0f;

    float tiempoEnCalcularDistanciaAlPlayer = 3.1f;
    float distancia = 50f; //la distancia que hay al player
    float distanciaVisionSierra = 50.0f; //distancia a la que la sierra empezara a moverse o se parara

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerSierra>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animador = GetComponent<Animator>();
        animador.enabled = false; //para que las animaciones no funcionen al inicidarse
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (distancia < distanciaVisionSierra)
        {
            if (distancia < distanciaSonidoSierra && !sonandoSierra) 
            {
                sonandoSierra = true;
                audioManager.SeleccionAudio(sonidoSierra, volumenSonidoSierra);
                Invoke("ResetearSonidoSierra", tiempoReseteoSonidoSierra);
            }
            animador.enabled = true;
            animador.SetBool("playerCerca", true);
        }
        else //esto es que la distancia es lejana
        { 
            animador.SetBool("playerCerca", false);
        }
    }
    IEnumerator DistanciaAlPlayer()
    {
        while (true)
        {
            distancia = CalcularDistancia();
            yield return new WaitForSeconds(tiempoEnCalcularDistanciaAlPlayer);
        }
    }
    float CalcularDistancia() //calcula la distancia entre el crab y el player
    {
        Vector3 posicionSierra = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionSierra); //distancia entre sierra y player
        return distancia;
    }
    void ResetearSonidoSierra() 
    {
        sonandoSierra = false;//para que vuela a sonar la sierra
    }
   
}
