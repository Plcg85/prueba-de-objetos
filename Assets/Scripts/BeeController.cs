using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeController : MonoBehaviour
{
    private AudioManagerBee audioManager;
    
    private int sonidoAlarma = 0;
    private float volumenAlarma = 1f;
    private bool sePuedeAlarma = true;
    
    private bool sonidoCrecer = true;//puede sonar el sonido de crecer
    int audioCrecer = 1; //indice del sonido de crecer en el array del audiomanager
    float volumenCrecer = 1.0f;
    
    private bool sePuedeSonidoLoco = true;
    private int sonidoLoco = 2;
    private float volumenSonidoLoco = 1.0f;

    private bool sePuedeSonidoExplotar = true;
    private int sonidoExplotar = 3;
    private float volumenSonidoExplotar = 1.0f;

    private float tiempoEnPoderVolverASonarVolando = 2.8f;
    private bool sePuedeSonidoVolar = true;
    private int sonidoVolando = 4;
    private float volumenSonidoVolar = 0.5f;

    private int sonidoEstrellitas = 5;
    private float volumenSonidoEstrellitas = 0.5f;

    public GameObject particulasLocura;
    public GameObject particulasExplosion;
    public GameObject player;
    NavMeshAgent agent;
    public Transform[] points;
    Animator animador;
    private int destPoint = 0; //El primer destino sera el punto 0
    private float distanciaAlPlayer = 50;
    private int distanciaDibujado = 45;
    private int distanciaVisionBee = 15;
    private float tiempoEnCalcularDistanciaAlPlayer = 0.1f;
    private bool destinoPlayer = false;
    private int longitudRayo = 15;
    Vector3 crecimiento = new Vector3(0.25f, 0.25f, 0.25f);
    private bool vaAExplotar = false;
    private bool puedeCrecer = true;
    private float tiempoCreciendo = 2.0f;//el tiempo que esta creciendo desde que te ve
    private float tiempoParaExplotar = 5.0f;
    private bool playerVisto = false; //sera true en cuanto vea al player
    private float velocidadLocura = 10f;
    private float aceleracionLocura = 50f;
    private bool golpeado = false;
    float tiempoGolpeado = 10.0f; //el tiempo que dura el estado golpeado
    Vector3 posicionInicial;
    Vector3 escala;
    float velocidadInicial;
    float aceleracionInicial;

    Stats stats; //el scripts de stats

    float distanciaDanioExplotar = 5.0f;//si estamos mas cerca del bee al explotar nos causara daño

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerBee>();
        stats = FindObjectOfType<Stats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animador = GetComponent<Animator>();
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
        StartCoroutine("VolverAPoderSonarVolando", tiempoEnPoderVolverASonarVolando);
        velocidadInicial = agent.speed;
        posicionInicial = transform.position;
        escala = transform.localScale;
        aceleracionInicial = agent.acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        if (distanciaAlPlayer <= distanciaDibujado)
        {
            animador.enabled = true;
            if (distanciaAlPlayer <= distanciaVisionBee && ViendoAlPlayer() && !golpeado)
            {                
                if (sePuedeAlarma) //emitir el sonido de alarma se reinicia en GotoNextPoint();
                {
                    transform.GetChild(5).gameObject.SetActive(true);
                    SonidoAlarma();
                }
                playerVisto = true;
                agent.destination = agent.transform.position;//para que se quede en el mismo sitio
                destinoPlayer = true;
            
                if (puedeCrecer)
                {
                    agent.transform.LookAt(player.transform);
                    agent.transform.localScale += crecimiento * Time.deltaTime;
                    if (sonidoCrecer) 
                    {
                        sonidoCrecer = false;
                        audioManager.PararSonido();
                        audioManager.SeleccionAudio(audioCrecer, volumenCrecer);
                    }
                }
                if (vaAExplotar == false) 
                {
                    vaAExplotar = true;
                    Invoke("DejarDeCrecer", tiempoCreciendo);
                    Invoke("Explotar", tiempoParaExplotar);
                }
                
            }
            else if (distanciaAlPlayer <= distanciaVisionBee && !ViendoAlPlayer() && !playerVisto) 
            {
                //sonido de volar 
                if (sePuedeSonidoVolar && !playerVisto && !ViendoAlPlayer())
                {
                    audioManager.SeleccionAudio(sonidoVolando, volumenSonidoVolar);
                    sePuedeSonidoVolar = false;
                }
                CaminoNormal();
            }
            else if (distanciaAlPlayer > distanciaVisionBee && !playerVisto) //aqui debe de seguir su camino normal
            {
                CaminoNormal();
            }
        }
        else if (distanciaAlPlayer > distanciaDibujado) 
        {
            animador.enabled = false;
        }
        if (!puedeCrecer) 
        {
            agent.speed = velocidadLocura;
            agent.acceleration = aceleracionLocura;
            agent.destination = player.transform.position;
            if (sePuedeSonidoLoco)
            {
                sePuedeSonidoLoco = false;
                audioManager.SeleccionAudio(sonidoLoco, volumenSonidoLoco);
            }
        }
        if (golpeado) 
        {
            agent.destination = agent.transform.position;
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
    IEnumerator VolverAPoderSonarVolando() 
    {
        while (true)
        {
            sePuedeSonidoVolar = true;
            yield return new WaitForSeconds(tiempoEnPoderVolverASonarVolando);
        }
    }
    float CalcularDistancia() //calcula la distancia entre el crab y el player
    {
        Vector3 posicionCrab = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionCrab); //distancia entre crab y player
        return distancia;
    }
    void CaminoNormal()//se asegura que el llegue al siguiente punto
    {
        transform.GetChild(5).gameObject.SetActive(false);
        destinoPlayer = false;
        if ((agent.remainingDistance < 2.5) && (!destinoPlayer)) //esta distancia se refiere al objetivo de los puntos (transforms)
        {
            GotoNextPoint();
        }
    }
    void GotoNextPoint()//siguiente punto en el array de puntos del crab
    {
        if (points.Length == 0) { return; } //si no hay puntos donde ir pues nada
        agent.destination = points[destPoint].position;//lo destina a donde debe ir
        destPoint = (destPoint + 1) % points.Length;
    }
    bool ViendoAlPlayer() //devuelve true o false si el crab esta viendo al player
    {
        bool cRayo1 = false, cRayo2 = false, cRayo3 = false, cRayo4 = false;
        RaycastHit hit;

        Ray ray1 = new Ray(transform.GetChild(2).position + new Vector3(0, 1, 0), transform.GetChild(2).forward);
        Debug.DrawRay(ray1.origin, ray1.direction * 15f, Color.green);

        Ray ray2 = new Ray(transform.GetChild(3).position + new Vector3(0, 1, 0), transform.GetChild(3).forward);
        Debug.DrawRay(ray2.origin, ray2.direction * 15f, Color.green);

        Ray ray3 = new Ray(transform.position + new Vector3(0, 1, 0), -1 * transform.forward);
        Debug.DrawRay(ray3.origin, ray3.direction * 4, Color.green);

        Ray ray4 = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        Debug.DrawRay(ray4.origin, ray4.direction * 15f, Color.green);


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
        if (Physics.Raycast(ray3, out hit, longitudRayo - 9))
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
    void Explotar() 
    {
        audioManager.PararSonido();
        Instantiate(particulasExplosion, transform.position, transform.rotation);
        
        float distanciaAlExplotar = CalcularDistancia(); //la distancia al player a la hora de explotar
        if (distanciaAlExplotar < distanciaDanioExplotar)
        {
            stats.RestarVida();
        }

        if (sePuedeSonidoExplotar)
        {
            audioManager.PararSonido();//Se paran los sonidos del bee
            sePuedeSonidoExplotar = false;
            audioManager.SeleccionAudio(sonidoExplotar, volumenSonidoExplotar);
        }
        
        ReiniciarBee(); //Devuelve al bee a su posicion inicial y lo pone a empezar de cero
        
        if (sePuedeSonidoExplotar) 
        {
            audioManager.PararSonido();//Se paran los sonidos del bee
            sePuedeSonidoExplotar = false;
            audioManager.SeleccionAudio(sonidoExplotar, volumenSonidoExplotar);
        }
    }
    void DejarDeCrecer() 
    {
        puedeCrecer = false;
        Instantiate(particulasLocura, transform.position, transform.rotation);//aqui se crean particulas
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAtack")) //Cuando es golpeado por la bola ataque del enemigo
        {
            audioManager.PararSonido();
            audioManager.SeleccionAudio(sonidoEstrellitas, volumenSonidoEstrellitas);
            golpeado = true;
            transform.GetChild(4).gameObject.SetActive(true);
            Invoke("DejarDeEstarGolpeado", tiempoGolpeado);
        }
        else if (other.CompareTag("Player")) { stats.RestarVida(); }
    }
    private void DejarDeEstarGolpeado()
    {
        audioManager.PararSonido();
        golpeado = false;
        transform.GetChild(4).gameObject.SetActive(false);
    }
    void SonidoAlarma()
    {
        audioManager.PararSonido();
        audioManager.SeleccionAudio(sonidoAlarma, volumenAlarma);
        sePuedeAlarma = false;
    }
    void ReiniciarBee() 
    {
        sePuedeSonidoVolar = true; // para que pueda volver a sonar volando
        sePuedeSonidoExplotar = true;
        sePuedeSonidoLoco = true; //puede volver a sonar como un loco
        sonidoCrecer = true;// puede volver a sonar
        transform.position = posicionInicial;    
        sePuedeAlarma = true;
        transform.localScale = escala;
        golpeado = false;
        puedeCrecer = true;
        agent.speed = velocidadInicial;
        agent.acceleration = aceleracionInicial;
        animador.enabled = true;
        vaAExplotar = false;
        playerVisto = false;
        GotoNextPoint();
        CaminoNormal();
        audioManager.PararSonido();
    }
}
