using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayoEnergia : MonoBehaviour
{
    public GameObject player; //referencia al player
    private AudioManager audioManager;
    private int sonidoEnergia = 3;
    private float volumenSonidoEnergia = 1.0f;

    private float tiempoEnCalcularDistanciaAlPlayer = 5.3f;
    private float distanciaAlPlayer = 50.0f;
    private float velociadRotacionY = 60.0f;
    private float tiempoEnDestruirRayo = 0.7f;

    //para la energia
    Stats stats;


    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        stats = FindObjectOfType<Stats>();//se puede hacer asi porque solo hay uno
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (distanciaAlPlayer < 50)
        {
            transform.Rotate(0, velociadRotacionY * Time.deltaTime, 0, Space.World);
        }
    }
    IEnumerator DistanciaAlPlayer()
    {
        while (true)
        {
            distanciaAlPlayer = CalcularDistancia();
            yield return new WaitForSeconds(tiempoEnCalcularDistanciaAlPlayer);
        }
    }
    float CalcularDistancia() //calcula la distancia entre el crab y el player
    {
        Vector3 posicionCrab = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionCrab); //distancia entre crab y player
        return distancia;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            if (stats.DecirNumeroEnergia() == 5)
            {
                //no se hace nada
            }
            else 
            {
                transform.GetChild(0).gameObject.SetActive(true);
                EmiteSonidoRayo(sonidoEnergia, volumenSonidoEnergia);
                Invoke("DestruirRayo", tiempoEnDestruirRayo);
                stats.SumarEnergia();
            }
            
        }
    }
    private void DestruirRayo()
    {
        Destroy(gameObject);
    }
    private void EmiteSonidoRayo(int sonidoCorazon, float volumenSonidoCorazon)
    {
        audioManager.SeleccionAudio(sonidoCorazon, volumenSonidoCorazon);
    }
}
