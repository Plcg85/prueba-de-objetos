using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CilindroPinchos : MonoBehaviour
{
    private AudioManagerCilindroPinchos audioManager;
    private int sonidoRodillo = 0;
    private float volumenRodillo= 0.3f;
    private bool puedeSonar = true;
    private float duracionSonidoRodillo = 1.8f;

    public GameObject player; 
    NavMeshAgent agent;
    public Transform[] points; //puntos de navegacion
    private int destPoint = 0;//primer destino es el punto 0;
    GameObject cilindroHijo;
    float velocidadGiro = 500.0f;
    float distancia = 50.0f; //la distancia al player
    float tiempoEnCalcularDistanciaAlPlayer = 1.4f;
    float tiempoEnReactivarParticulas = 1.0f;

    Stats stats;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerCilindroPinchos>();
        stats = FindObjectOfType<Stats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cilindroHijo = transform.GetChild(0).gameObject;
        transform.GetChild(1).gameObject.SetActive(false); //desactivar el efecto particulas

        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer); //calcular distancia al player                                                                  
    }

    // Update is called once per frame
    void Update()
    {
        if (distancia < 50) 
        {
            CaminoNormal();
            RotarPinchos();
            if (distancia < 20 && puedeSonar) 
            {
                audioManager.SeleccionAudio(sonidoRodillo, volumenRodillo);
                puedeSonar = false;
                Invoke("ResetearSonido", duracionSonidoRodillo);
            }
        }  
    }
    void CaminoNormal()//se asegura que el llegue al siguiente punto
    {
        if ((agent.remainingDistance < 0.5f)) //esta distancia se refiere al objetivo de los puntos (transforms)
        {
            GotoNextPoint();
        }
    }
    void GotoNextPoint()//siguiente punto en el array de puntos del crab
    {
        if (points.Length == 0) { return; } //si no hay puntos donde ir pues nada
        agent.destination = points[destPoint].position;//lo destina a donde debe ir
        destPoint = (destPoint + 1) % points.Length;
        transform.GetChild(1).gameObject.SetActive(true); //activar las particulas
        Invoke("ReactivarParticulas", tiempoEnReactivarParticulas);
    }
    void RotarPinchos() 
    {
        cilindroHijo.transform.Rotate(0, 0, 1 * velocidadGiro * Time.deltaTime);//El cilindro hijo tiene los ejes cambiados
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
        Vector3 posicionCrab = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionCrab); //distancia entre crab y player
        return distancia;
    }
    void ResetearSonido() 
    {
        puedeSonar = true;
    }
    void ReactivarParticulas() 
    {
        transform.GetChild(1).gameObject.SetActive(false); //desactivar el efecto particulas
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stats.RestarVida();
        }
    }
}
