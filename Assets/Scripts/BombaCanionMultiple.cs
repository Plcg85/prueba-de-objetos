using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaCanionMultiple : MonoBehaviour
{
    private GameObject player; //el jugador
    public AudioManager audioManager;
    private int sonidoBomba = 0;
    private float volumenBomba;

    public GameObject particulas;//particulas explosion pequeña
    Rigidbody bombaRigidbody;
    private float fuerzaImpulso = 0.0f;
    private float fuerzaImpulsoY = 5.0f;
    private float tiempoDestruirBomba = 5.0f;

    private float distanciaDanioExplotar = 5;

    Stats stats; //las stats del player

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = GameObject.Find("Player");
        stats = FindObjectOfType<Stats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        fuerzaImpulsoY = Random.Range(2.0f,7.0f);
        tiempoDestruirBomba = Random.Range(3.0f,5.0f);
        bombaRigidbody = GetComponent<Rigidbody>();
        transform.Rotate(90, 90, 0);
        bombaRigidbody.AddForce(transform.forward * fuerzaImpulso, ForceMode.Impulse);
        bombaRigidbody.AddForce(transform.up * fuerzaImpulsoY, ForceMode.Impulse);
        Invoke("DestruirBomba", tiempoDestruirBomba);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestruirBomba()
    {
        float distancia = CalcularDistancia();
        if (distancia < 45) 
        {
            volumenBomba = 1f - ((distancia / 100) * 2);
            EmiteSonidoBomba(sonidoBomba, volumenBomba);
            Instantiate(particulas, transform.position, particulas.transform.rotation);
            float distanciaAlExplotar = CalcularDistancia();//la distancia al player a la hora de explotar
            if (distanciaAlExplotar < distanciaDanioExplotar) 
            {
                stats.RestarVida();
            }
        }
        
        Destroy(gameObject);
    }
    float CalcularDistancia() 
    {
        Vector3 posicionBomba = transform.position;
        Vector3 posicionPlayer = player.transform.position;
        float distancia = Vector3.Distance(posicionPlayer, posicionBomba); //distancia entre bomba y player
        return distancia;
    }
    void EmiteSonidoBomba(int sonidoBomba,float volumenBomba) 
    {
        audioManager.SeleccionAudio(sonidoBomba, volumenBomba);
    }
}
