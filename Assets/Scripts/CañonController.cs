using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CañonController : MonoBehaviour
{
    public GameObject player;//player principal
    public GameObject particulasHumo; //Las particulas que se ven al salir la bomba

    private float tiempoEnCalcularDistanciaAlPlayer = 3.0f;
    private float distancia = 50; //distancia al player

    public GameObject bomba; //la bomba que se dispara
    Animator animador;
    private float tiempoEnDisparar = 5f;

    private AudioManagerCanion audioManager;
    private int sonidoDisparo = 0;//indice array de sonidos audiomanager
    private float volumenDisparo = 1f;
    private int distanciaMaximaSonido = 35;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerCanion>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animador = GetComponent<Animator>();
        tiempoEnDisparar = Random.Range(5.0f, 7.0f);
        StartCoroutine("Disparar", tiempoEnDisparar);
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (distancia < distanciaMaximaSonido)
        {
            volumenDisparo = 1 - 2 * (distancia / 100); //se dispara y se hace el sonido en el metodo dejar de disparar que es llamado desde la animacion
        }
        else 
        {
            volumenDisparo = 0;
        }
    }

    IEnumerator Disparar() 
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEnDisparar);
            animador.SetBool("debeDisparar", true);
        }
    }
    void DejarDeDisparar() 
    {
        if (volumenDisparo > 0) 
        {
            audioManager.SeleccionAudio(sonidoDisparo, volumenDisparo);
        }
        Instantiate(bomba, transform.GetChild(0).transform.GetChild(0).position,transform.rotation);
        Instantiate(particulasHumo, transform.GetChild(0).transform.GetChild(0).position, transform.rotation); //instanciar las particulas de humo
        animador.SetBool("debeDisparar", false);
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
        Vector3 posicionCanion = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionCanion); //distancia entre cañon y player
        return distancia;
    }
}
