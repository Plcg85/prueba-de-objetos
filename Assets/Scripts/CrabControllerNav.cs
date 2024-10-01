using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabControllerNav : MonoBehaviour
{
    private AudioManagerCrab audioManager;
    private int sonidoAlarma = 0;
    private float volumenAlarma = 1f;
    private bool sePuedeAlarma = true;

    private int sonidoAtacarCrab = 1;
    private float volumenAtaqueCrab = 1.0f;

    private int sonidoPajaritosCrab = 2;
    private float volumenSonidoPajaritosCrab = 0.5f;

    public GameObject player;
    public GameObject bolaAtaque;
    public Transform[] points;
    private int destPoint = 0; //El primer destino sera el punto 0
    private int longitudRayo = 15; // la longitud del raycast
    bool destinoPlayer = false; //si el objetivo es el player se pone a true
    //bool estaViendoAlPlayer = false; //si el crab ve al player esta variable se pone a true
    private Animator animador;
    private bool sePuedeDisparar = true;
    private bool golpeado = false;
    private int distanciaDibujado = 45;
    private int distaciaVisionCrub = 15;
    private float distancia = 50; //inicialmente se pone alto
    private float tiempoEnCalcularDistanciaAlPlayer = 0.1f;
    private float tiempoGolpeado = 10.0f;

    NavMeshAgent agent;

    //El scripts de las estadisticas, vida, llaves etc.
    Stats stats;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerCrab>();
        stats = FindObjectOfType<Stats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animador = GetComponent<Animator>();
        animador.enabled = false; //para que las animaciones no funcionen al inicidarse 

        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Distancia crab = " + distancia);

        if (distancia < distanciaDibujado && !golpeado)
        {
            animador.enabled = true;
            if (distancia <= distaciaVisionCrub && ViendoAlPlayer())
            {
                transform.GetChild(5).gameObject.SetActive(true); //Se desactiva dentro de camino normal la exclamacion

                //animador.enabled = true;
                destinoPlayer = true;
                if (sePuedeAlarma) //emitir el sonido de alarma se reinicia en GotoNextPoint();
                {
                    SonidoAlarma();
                }
                agent.destination = player.transform.position;
                if (distancia < distaciaVisionCrub && distancia > 7) //en esta distancia es en la que debe estar atacando
                {
                    animador.SetBool("Atacando", true);
                    if (sePuedeDisparar)
                    {
                        sePuedeDisparar = false;
                        Invoke("Disparo", 1.0f);
                    }
                }
                else { animador.SetBool("Atacando", false); }
            }
            else { animador.SetBool("Atacando", false); }
            if (distancia <= distaciaVisionCrub && !ViendoAlPlayer())
            {
                CaminoNormal();
            }
            if (distancia > distaciaVisionCrub)
            {
                CaminoNormal();
            }
        }
        else if (distancia < distanciaDibujado && golpeado)//este bloque else se ejecuta si la distancia es mayor q 42 unidades
        {
            agent.destination = agent.transform.position;
        }
        else if (distancia > distanciaDibujado) 
        {
            animador.enabled = false;
        }
    }
    float CalcularDistancia() //calcula la distancia entre el crab y el player
    {
        Vector3 posicionCrab = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionCrab); //distancia entre crab y player
        return distancia;
    }
    void GotoNextPoint()//siguiente punto en el array de puntos del crab
    {
        if (points.Length == 0) { return; } //si no hay puntos donde ir pues nada
        agent.destination = points[destPoint].position;//lo destina a donde debe ir
        destPoint = (destPoint + 1) % points.Length;
        sePuedeAlarma = true;
    }
    bool ViendoAlPlayer() //devuelve true o false si el crab esta viendo al player
    {
        bool cRayo1 = false, cRayo2 = false, cRayo3 = false , cRayo4 = false;
        RaycastHit hit;

        Ray ray1 = new Ray(transform.GetChild(2).position + new Vector3(0, 1, 0), transform.GetChild(2).forward);
        Debug.DrawRay(ray1.origin, ray1.direction * 15f, Color.red);

        Ray ray2 = new Ray(transform.GetChild(3).position + new Vector3(0, 1, 0), transform.GetChild(3).forward);
        Debug.DrawRay(ray2.origin, ray2.direction * 15f, Color.red);

        Ray ray3 = new Ray(transform.position + new Vector3(0,1,0), -1 * transform.forward);
        Debug.DrawRay(ray3.origin, ray3.direction * 4, Color.red);

        Ray ray4 = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        Debug.DrawRay(ray4.origin, ray4.direction * 15f, Color.red);


        if (Physics.Raycast(ray1, out hit, longitudRayo))
        {
            if (hit.transform.name == "Player")
            {
                cRayo1 = true;
            }
        }
        if (Physics.Raycast(ray2, out hit, longitudRayo))
        {
            if (hit.transform.name == "Player")
            {
                cRayo2 = true;
            }
        }
        if (Physics.Raycast(ray3, out hit, longitudRayo-9))
        {
            if (hit.transform.name == "Player")
            {
                cRayo3 = true;
            }
        }
        if (Physics.Raycast(ray4, out hit, longitudRayo))
        {
            if (hit.transform.name == "Player")
            {
                cRayo4 = true;
            }
        }
        if (cRayo1 || cRayo2 || cRayo3 || cRayo4)
        {
            return true;   
        }
        else return false;
    }
    void CaminoNormal()//se asegura que el llegue al siguiente punto
    {
        destinoPlayer = false;
        if ((agent.remainingDistance < 2.5) && (!destinoPlayer)) //esta distancia se refiere al objetivo de los puntos (transforms)
        {
            transform.GetChild(5).gameObject.SetActive(false);
            GotoNextPoint();
        }
    }
    void Disparo() 
    {
        audioManager.SeleccionAudio(sonidoAtacarCrab, volumenAtaqueCrab);
        Instantiate(bolaAtaque, agent.transform.position + new Vector3(0,1,0), agent.transform.rotation);
        sePuedeDisparar = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player") //el crab ha chocado contra el player
        {
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAtack")) //entra cuando la bola que lanza el player choca contra el crab
        {
            audioManager.PararSonido();
            audioManager.SeleccionAudio(sonidoPajaritosCrab, volumenSonidoPajaritosCrab);
            animador.SetBool("Golpeado", true);
            Invoke("DejarDeEstarGolpeado", tiempoGolpeado);
            golpeado = true;
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (other.CompareTag("Player")) { stats.RestarVida(); }
    }
    private void DejarDeEstarGolpeado() 
    {
        audioManager.PararSonido(); //parar el sonido pajaritos
        animador.SetBool("Golpeado", false);
        GotoNextPoint();
        golpeado = false;
        transform.GetChild(4).gameObject.SetActive(false);
    }
    IEnumerator DistanciaAlPlayer() 
    {
        while (true)
        {
            distancia = CalcularDistancia();
            yield return new WaitForSeconds(tiempoEnCalcularDistanciaAlPlayer);
        }
    }
    void SonidoAlarma() 
    {
        audioManager.SeleccionAudio(sonidoAlarma, volumenAlarma);
        sePuedeAlarma = false;
    }
}
