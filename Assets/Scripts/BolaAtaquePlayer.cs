using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaAtaquePlayer : MonoBehaviour
{
    private float velocidadBola = 15.0f;
    public GameObject particulasExplosionAtaquePlayer;

    private AudioManager audioManager;
    private int sonidoChoque = 1;//indice array de sonidos audiomanager
    private float volumenChoque= 1f;

    private float tiempoEnDesaparecer = 4.0f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Desaparece", tiempoEnDesaparecer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * velocidadBola ,Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //si choca con el player nada
        }
        else 
        {
            audioManager.SeleccionAudio(sonidoChoque, volumenChoque);
            Instantiate(particulasExplosionAtaquePlayer, transform.position, particulasExplosionAtaquePlayer.transform.rotation);
            Destroy(gameObject);
        } 
    }
    private void Desaparece() 
    {
        Instantiate(particulasExplosionAtaquePlayer, transform.position, particulasExplosionAtaquePlayer.transform.rotation);
        Destroy(gameObject);
    }
}
