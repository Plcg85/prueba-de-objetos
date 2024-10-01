using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoCrab : MonoBehaviour
{
    public GameObject player;

    private AudioManager audioManager;
    private int sonidoCrabAndando = 2;
    private float volumenCrabAndando = 0.5f;
    private bool sonandoAndando = false;

    private float tiempoEnCalcularDistanciaAlPlayer = 1.7f;

    float distancia = 50f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
    }

    // Update is called once per frame
    void Update()
    {
    }
    float CalcularDistancia() //calcula la distancia entre el crab y el player
    {
        Vector3 posicionCrab = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionCrab); //distancia entre crab y player
        return distancia;
    }

    IEnumerator DistanciaAlPlayer()
    {
        while (true)
        {
            distancia = CalcularDistancia();
           
            if (distancia < 20)
            {
                audioManager.PararSonido();
                audioManager.SeleccionAudio(sonidoCrabAndando, volumenCrabAndando);
                sonandoAndando = true;
            }
            else if (distancia > 20) 
            {
                if (sonandoAndando) 
                {
                    audioManager.PararSonido();
                    sonandoAndando = false;
                }
            }
            yield return new WaitForSeconds(tiempoEnCalcularDistanciaAlPlayer);
        }
    }
}
