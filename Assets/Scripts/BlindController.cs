using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindController : MonoBehaviour
{
    private AudioManagerBlind audioManager;
    private int sonidoPajaritos = 0;
    private float volumenPajaritos = 0.3f;

    private int sonidoAndar = 1;
    private float volumenSonidoAndar = 0.5f;
    private float duracionSonidoAndar = 2.3f;

    NavMeshAgent agent;
    public GameObject player; //el jugador
    private bool golpeado = false;
    private float velocidadBlind = 3.0f;
    private float velocidadRotacionBlind = 25.0f;
    private float tiempoMomento = 0.0f;
    private float tiempoGolpeado = 2.0f;
    private bool noGirarMas = false;
    private Animator animador;
    private float distancia = 50; //inicialmente se pone alto
    private float tiempoEnCalcularDistanciaAlPlayer = 0.4f;
    private int distanciaDibujado = 45;
    private Vector3 posicionInicial;
    private float tiempoEnCalcularDistanciaAPosicionInicial = 5.0f;
    private float distanciaAPosicionInicial = 0;
    private float tiempoActivarColider = 2.0f;

    Stats stats;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerBlind>();
        stats = FindObjectOfType<Stats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;// la posicion inicial
        animador = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Invoke("ActivarCollider", tiempoActivarColider);

        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
        StartCoroutine("DistanciaAPosicionInicial", tiempoEnCalcularDistanciaAPosicionInicial);
        StartCoroutine("SonidoAndar", duracionSonidoAndar);
    }

    // Update is called once per frame
    void Update()
    {
        if (distancia <= distanciaDibujado)
        {
            animador.enabled = true;
            if (golpeado == false) //aqui esta andando adelante
            {
                transform.Translate(Vector3.forward * velocidadBlind * Time.deltaTime);
            }
            if (golpeado == true && noGirarMas == false) //aqui esta girando
            {
                EstaGolpeado();
                tiempoMomento += Time.deltaTime;
                if (tiempoMomento >= tiempoGolpeado)
                {
                    DejarDeEstarGolpeado();
                }
            }
        }
        else //aqui entra cuando esta muy lejos
        { 
            animador.enabled = false;
            if (distanciaAPosicionInicial > 30) 
            {
                transform.position = posicionInicial;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                Invoke("ActivarCollider", tiempoActivarColider);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Suelo")) //si choca con el suelo no hace nada
        {
        }
        else //si choca con algo que no sea el suelo
        {
            if (!golpeado) //esto es para que no reproduzca el sonido multiples veces cuando gira y choca a la vez
            {
                if (distancia < 20) 
                {
                    //si choca con el player
                    if (other.CompareTag("Player")) 
                    {
                        stats.RestarVida();    
                    }//aqui termina si choca con el player
                    
                    audioManager.PararSonido();
                    audioManager.SeleccionAudio(sonidoPajaritos, volumenPajaritos);
                } 
            }
            golpeado = true;
            animador.SetBool("noqueado", true);           
        }
    }
    void DejarDeEstarGolpeado() 
    {
        audioManager.PararSonido(); //para todos los sonidos
        transform.GetChild(4).gameObject.SetActive(false); //desactivar exclamaciones
        tiempoGolpeado = Random.Range(4.0f, 8.0f);
        tiempoMomento = 0;
        noGirarMas = false;
        golpeado = false;
        animador.SetBool("noqueado", false);
    }
    void EstaGolpeado() 
    {
        transform.GetChild(4).gameObject.SetActive(true); //desactivar exclamaciones
        transform.Rotate(Vector3.up, velocidadRotacionBlind * Time.deltaTime);
        
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
    IEnumerator DistanciaAPosicionInicial() 
    {
        while (true)
        {
            CalcularDistanciaAPosicionInicial();
            yield return new WaitForSeconds(tiempoEnCalcularDistanciaAPosicionInicial);
        }
    }
    IEnumerator SonidoAndar()
    {
        while (true)
        {
            if (distancia < 15 && !golpeado) 
            {
                audioManager.SeleccionAudio(sonidoAndar, volumenSonidoAndar);
            }
            yield return new WaitForSeconds(duracionSonidoAndar);
        }
    }
    void CalcularDistanciaAPosicionInicial() 
    {
        distanciaAPosicionInicial = Vector3.Distance(posicionInicial , transform.position);
    }
    void ActivarCollider() 
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
