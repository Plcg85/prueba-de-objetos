using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanionMultiple : MonoBehaviour
{
    private AudioManagerCanionMultiple audioManager;
    public GameObject particulasHumo; //Las particulas que se emiten al disparar

    private int sonidoCanion = 0;
    private float volumenSonidoCanion = 1.0f;
   
    public GameObject bomba; //la bomba que se dispara
    public GameObject player;
    private float tiempoEnCalcularDistanciaAlPlayer = 1.1f;
    private float distancia = 50.0f;
    private Animator animador;
    private float velocidadRotacionY = 50;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerCanionMultiple>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
        animador = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (distancia < 50)
        {
            animador.enabled = true;
            transform.Rotate(0, velocidadRotacionY * Time.deltaTime, 0);
        }
        else { animador.enabled = false; }
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
            yield return new WaitForSeconds(tiempoEnCalcularDistanciaAlPlayer);   
        }
    }
    void disparar() 
    {
        volumenSonidoCanion = 1f - ((distancia / 100) * 2);
        audioManager.SeleccionAudio(sonidoCanion, volumenSonidoCanion);

        Instantiate(bomba, transform.GetChild(0).position, transform.GetChild(0).rotation);
        Instantiate(particulasHumo, transform.GetChild(0).position, transform.GetChild(0).rotation);
        
        Instantiate(bomba, transform.GetChild(1).position, transform.GetChild(1).rotation);
        Instantiate(particulasHumo, transform.GetChild(1).position, transform.GetChild(1).rotation);

        Instantiate(bomba, transform.GetChild(2).position, transform.GetChild(2).rotation);
        Instantiate(particulasHumo, transform.GetChild(2).position, transform.GetChild(2).rotation);

        Instantiate(bomba, transform.GetChild(3).position, transform.GetChild(3).rotation);
        Instantiate(particulasHumo, transform.GetChild(3).position, transform.GetChild(3).rotation);
    }

}
