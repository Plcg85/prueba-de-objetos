using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AudioManagerPlayer audioManager;
    private int sonidoAtaquePlayer = 0;
    private float volumenAtaquePlayer = 1.0f;

    private int sonidoDash = 1;
    private float volumenDash = 1.0f;

    Rigidbody playerRigidbody;
    public GameObject camara;
    public GameObject bolaAtaquePlayer;
    private Vector3 offset = new Vector3(0, 54, -30);//offset de la camara

    private int velocidadMovimiento = 5;
    private float velocidadGiro = 2000;

    private Vector3 velocity = Vector3.zero;

    private Animator animador;
    private bool disparando = false;
    private bool corriendo = false;
    private float movimientoHorizontal;
    private float movimientoVertical;

    //dash variables
    private bool puedeLanzarse = true;
    private float duracionDash = 0.3f;
    private bool enDash = false;
    private int fuerzaDash = 30;

    Stats stats; //el script de estadisticas
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerPlayer>();
        //se desactivan las particulas del dash
        transform.GetChild(7).gameObject.SetActive(false);
        stats = FindObjectOfType<Stats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animador = gameObject.GetComponent<Animator>();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        if (stats.DecirNumeroVidas() > 0)//si esta vivo
        {
            movimientoHorizontal = Input.GetAxis("Horizontal");
            movimientoVertical = Input.GetAxis("Vertical");

            playerRigidbody.velocity = Vector3.zero; //para que despues de una colision no siga moviendose a lo loco

            if (!disparando)
            {
                //movimiento y giro ////////////////////////////////////////////////////////////////////////////////////////////////////
                Vector3 direccion = new Vector3(movimientoHorizontal, 0, movimientoVertical);
                direccion.Normalize();

                //Movimiento Player
                //transform.Translate(direccion * velocidadMovimiento * Time.deltaTime, Space.World);
                playerRigidbody.MovePosition(playerRigidbody.position + Time.deltaTime * velocidadMovimiento * direccion);

                //movimiento de la camara
                camara.transform.position = gameObject.transform.position + offset;

                if (direccion != Vector3.zero)
                {
                    //Se activan las particulas al andar 
                    transform.GetChild(6).gameObject.SetActive(true);

                    //se rota el personaje
                    Quaternion toRotation = Quaternion.LookRotation(direccion, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, velocidadGiro * Time.deltaTime);

                    //animaciones
                    animador.SetBool("corriendo", true);
                    corriendo = true;

                    //el dash si procede
                    //puede lanzarse lo controla el boton del mando
                    if (puedeLanzarse && enDash)
                    {
                        playerRigidbody.AddForce(playerRigidbody.transform.forward * fuerzaDash, ForceMode.Impulse);
                    }
                }
                else //aqui entra si el personaje no se esta moviendo
                {
                    //se desactiva las particulas de andar
                    transform.GetChild(6).gameObject.SetActive(false);

                    //se desactivan las animaciones de andar
                    animador.SetBool("corriendo", false);
                    corriendo = false;
                }
            }


            //BOTONES//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (Input.GetButton("Fire1"))//A 
            {
                //Dash. Esto controla las variables y la fuerza se añade en la parte de if (direccion != Vector3.zero)
                puedeLanzarse = true;
                if (puedeLanzarse && !enDash && corriendo)
                {
                    if (stats.DecirNumeroEnergia() > 0)
                    {
                        stats.RestarEnergia();
                        audioManager.SeleccionAudio(sonidoDash, volumenDash);
                        //se activan las particulas del dash
                        transform.GetChild(7).gameObject.SetActive(true);
                        puedeLanzarse = false;
                        enDash = true;
                        Invoke("PararLanzamiento", duracionDash);
                    }
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
            }
            if (Input.GetButton("Jump"))//Y
            {
                Debug.Log("Jump pulsado");
            }
            if (Input.GetButton("Fire2"))//B
            {
                Debug.Log("Fire2 pulsado");
            }
            if (Input.GetButton("Fire3"))//X  && !corriendo
            {
                if (!disparando && !enDash) //!corriendo && 
                {
                    if (stats.DecirNumeroEnergia() > 0)
                    {
                        stats.RestarEnergia();
                        //se desactiva las particulas de andar
                        transform.GetChild(6).gameObject.SetActive(false);

                        disparando = true;
                        animador.SetBool("disparando", true);
                        Invoke("DejarDeDisparar", 1.0f);
                        Invoke("Disparo", 0.6f);
                    }
                }
            }
        }
        else //si esta muerto
        {
            transform.GetChild(6).gameObject.SetActive(false);//se desactivan las particulas de andar
            animador.SetBool("muerto", true);
        }
    }
    void DejarDeDisparar() 
    {
        animador.SetBool("disparando", false);
        disparando = false;
    }
    void Disparo() 
    {
        Instantiate(bolaAtaquePlayer,  transform.GetChild(0).position + new Vector3(0,1,0) , transform.rotation);
        audioManager.SeleccionAudio(sonidoAtaquePlayer, volumenAtaquePlayer);
    }

    void PararLanzamiento() 
    {
        //para que no gire al chocar con algo en el dash
        playerRigidbody.angularVelocity = Vector3.zero;

        //se desactivan las particulas del dash
        transform.GetChild(7).gameObject.SetActive(false);

        puedeLanzarse = false;
        enDash = false;
    }
}
