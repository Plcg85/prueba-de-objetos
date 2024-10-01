using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorazonVida : MonoBehaviour
{
    public GameObject player; //referencia al jugador
    private AudioManager audioManager;
    private int sonidoCorazon = 3;
    private float volumenSonidoCorazon = 1.0f;

    private float tiempoEnCalcularDistanciaAlPlayer = 5.2f;
    private float distanciaAlPlayer = 50.0f;
    private float velociadRotacionY = 60.0f;
    private float tiempoEnDestruirCorazon = 0.7f;

    //esto es para la vida
    Stats stats;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        stats = FindObjectOfType<Stats>();//esto se puede hacer porque solo hay un gameobjects con el scripts stats
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
            transform.Rotate(0, velociadRotacionY * Time.deltaTime, 0 , Space.World);
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
            if (stats.DecirNumeroVidas() == 5)
            {
                //no se hace nada porque ya tiene 5 vidas
            }
            else 
            {
                transform.GetChild(0).gameObject.SetActive(true);
                EmiteSonidoCorazon(sonidoCorazon, volumenSonidoCorazon);
                Invoke("DestruirCorazon", tiempoEnDestruirCorazon);
                stats.SumarVida();
            }
            
        }
    }
    private void DestruirCorazon() 
    {
        Destroy(gameObject);
    }
    private void EmiteSonidoCorazon(int sonidoCorazon, float volumenSonidoCorazon) 
    {
        audioManager.SeleccionAudio(sonidoCorazon, volumenSonidoCorazon);
    }
}
