using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkrullController : MonoBehaviour
{
    private AudioManagerSkrull audioManager;
    private int sonidoAlarma = 0;
    private float volumenAlarma = 1f;
    private bool sePuedeAlarma = true;

    private int sonidoGirar = 1;
    private float volumenSonidoGirar = 0.5f;

    private int sonidoAtaqueSkrull = 2;
    private float volumenAtaqueSkrull = 1.0f;

    private int sonidoPajaritosSkrull = 3;
    private float volumenSonidoPajaritosSkrull = 0.5f;

    public GameObject player; //el jugador principal
    public GameObject bolaAtaqueScrull; // la bola de ataque del skrull
    private Animator animador;
    private float velocidadAngulo = 50f;
    private bool viendoAlPlayer = false;
    private float tiempoEnCambiarDeSentido = 3.0f;
    private float tiempoEnCalcularDistanciaAlPlayer = 0.5f;
    private float distancia = 50;//inicialmente se pone alto
    private int longitudRayo = 15; //la longitud del raycast desde el skrull al player
    private int distanciaVisionSkrull = 15;
    private int distanciaDibujado = 45;
    private bool sePuedeDisparar = true;
    private bool golpeado = false;
    private bool primerDisparo = true; //cuando se puede disparar nada mas ver al player
    private float tiempoEntreDisparos = 2.0f;//el tiempo que tarda el skrull en disparar de nuevo

    Stats stats;//El scripts de stats del player

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerSkrull>();
        stats = FindObjectOfType<Stats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animador = GetComponent<Animator>();
        StartCoroutine("CambiarSentido", tiempoEnCambiarDeSentido);
        StartCoroutine("DistanciaAlPlayer", tiempoEnCalcularDistanciaAlPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (distancia <= distanciaDibujado) //si la distancia es mayor no hace nada
        {
            animador.enabled = true;//se anima el skrull
            if (distancia <= distanciaVisionSkrull && !golpeado)
            {
                viendoAlPlayer = ViendoAlPlayer();//se comprueba si esta viendo al player una vez solamente

                if (viendoAlPlayer)
                {
                    if (sePuedeAlarma) //emitir el sonido de alarma se reinicia en GotoNextPoint();
                    {
                        SonidoAlarma();
                    }
                    if (primerDisparo)
                    {
                        Disparo();
                    }

                    transform.GetChild(5).gameObject.SetActive(true);//Activar exclamaciones
                    //transform.LookAt(player.transform); // de esta forma la cabeza se inclinaba
                    // rotar skrull sin que la cabeza se incline
                    Vector3 lookPos = player.transform.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                    //se ataca al player
                    if (sePuedeDisparar)
                    {
                        sePuedeDisparar = false;
                        Invoke("Disparo", tiempoEntreDisparos);
                    }
                }
                if (!viendoAlPlayer)
                {
                    transform.Rotate(Vector3.up, velocidadAngulo * Time.deltaTime);
                }

            }
            else if (distancia > distanciaVisionSkrull && !golpeado)
            {
                sePuedeAlarma = true;//se puede volver a tocar el sonido de alarma
                transform.Rotate(Vector3.up, velocidadAngulo * Time.deltaTime);
                transform.GetChild(5).gameObject.SetActive(false); //desactivar exclamaciones
                primerDisparo = true; //se puede disparar en cuanto lo vea
            }
        }
        else if (distancia > distanciaDibujado) 
        {
            animador.enabled = false;
        }
    }
    IEnumerator CambiarSentido()
    {
        while (true)
        {
            SonidoGirar();
            velocidadAngulo *= -1;
            yield return new WaitForSeconds(tiempoEnCambiarDeSentido);
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
    float CalcularDistancia() 
    {
        Vector3 posicionScrull = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionScrull); //distancia entre scrull y player
        return distancia;
    }
    bool ViendoAlPlayer() //devuelve true o false si el crab esta viendo al player 
    {
        bool cRayo1 = false, cRayo2 = false , cRayo3 = false;
        RaycastHit hit;

        Ray ray1 = new Ray(transform.GetChild(2).position + new Vector3(0, 1, 0), transform.GetChild(2).forward);
        Debug.DrawRay(ray1.origin, ray1.direction * 15f, Color.red);

        Ray ray2 = new Ray(transform.GetChild(3).position + new Vector3(0, 1, 0), transform.GetChild(3).forward);
        Debug.DrawRay(ray2.origin, ray2.direction * 15f, Color.red);

        //El scrull lleva un tercer rayo de frente
        Ray ray3 = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
        Debug.DrawRay(ray3.origin, ray3.direction * 15f, Color.red);


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
        if (Physics.Raycast(ray3, out hit, longitudRayo))
        {
            if (hit.transform.name == "Player")
            {
                cRayo3 = true;
            }
        }

        if (cRayo1 || cRayo2 || cRayo3)
        {
            return true;
        }
        else return false;
    }
    void Disparo() 
    {
        primerDisparo = false;
        sePuedeDisparar = true;
        audioManager.SeleccionAudio(sonidoAtaqueSkrull, volumenAtaqueSkrull);
        Instantiate(bolaAtaqueScrull, player.transform.position + new Vector3(0,25,0), player.transform.rotation);
        Instantiate(bolaAtaqueScrull, player.transform.position + new Vector3(Random.Range(-5,5), 25, Random.Range(-5,5)),player.transform.rotation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAtack")) //entra cuando la bola que lanza el player choca contra el crab
        {
            audioManager.SeleccionAudio(sonidoPajaritosSkrull, volumenSonidoPajaritosSkrull);
            transform.GetChild(5).gameObject.SetActive(false); //desactivar Exclamaciones
            Invoke("DejarDeEstarGolpeado", 10.0f);
            golpeado = true;
            animador.SetBool("golpeado", true);
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (other.CompareTag("Player")) { stats.RestarVida(); }
    }
    private void DejarDeEstarGolpeado()
    {
        audioManager.PararSonido();//para sonido pajaritos
        animador.SetBool("golpeado",false);
        golpeado = false;
        transform.GetChild(4).gameObject.SetActive(false);
    }
    void SonidoAlarma()
    {
        audioManager.SeleccionAudio(sonidoAlarma, volumenAlarma);
        sePuedeAlarma = false;
    }
    void SonidoGirar() //se llama en CambiarSentido
    {
        if (distancia < distanciaVisionSkrull && !golpeado && !viendoAlPlayer) 
        {
            audioManager.SeleccionAudio(sonidoGirar, volumenSonidoGirar);
        }
    }
}
