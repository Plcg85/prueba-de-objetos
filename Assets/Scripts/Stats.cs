using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private AudioManager audioManager;
    private int sonidoPerderVida = 4;
    private float volumenSonidoPerderVida = 0.6f;

    //En esta clase va todo lo relacionado con la vida, energia, llaves, etc del jugador
    
    //Vidas
    public GameObject[] corazonVida;
    private int vidasIniciales = 3;
    private int vidas = 0; // luego se actualizara

    //Energia
    public GameObject[] energiaRayo;
    private int energiaInicial = 3;
    private int energia = 0; //la energia actual

    //Llaves
    public GameObject[] llavesArray;
    private int llaves = 0; //este no hace falta iniciarlo porque al iniciar siempre es 0

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        transform.GetChild(0).gameObject.SetActive(false);//desactivar corazon menos
    }
    // Start is called before the first frame update
    void Start()
    {
        vidas = vidasIniciales;
        energia = energiaInicial;
        RellenarEnergiaInicial();
        RellenarVidaInicial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RellenarVidaInicial() 
    {
        for (int i = 0; i < vidasIniciales; i++)
        {
            corazonVida[i].SetActive(true);
        }
    }
    public void SumarVida()
    {
        if (vidas == 5)
        {
            //no se hace nada
        }
        else
        {
            vidas = vidas + 1;
            corazonVida[vidas - 1].SetActive(true);
        }
    }
    public void RestarVida() 
    {
        if (vidas > 0)
        {
            vidas = vidas - 1;
            corazonVida[vidas].SetActive(false);
            audioManager.SeleccionAudio(sonidoPerderVida, volumenSonidoPerderVida);
            transform.GetChild(0).gameObject.SetActive(true); //particulas perder vida
            if (vidas == 0) { Morir(); }
        }
    }
    public int DecirNumeroVidas() 
    {
        return vidas;
    }
    private void RellenarEnergiaInicial()
    {
        for (int i = 0; i < energiaInicial; i++)
        {
            energiaRayo[i].SetActive(true);
        }
    }
    public int DecirNumeroEnergia()
    {
        return energia;
    }
    public void SumarEnergia()
    {
        if (energia == 5)
        {
            //no se hace nada
        }
        else
        {
            energia = energia + 1;
            energiaRayo[energia - 1].SetActive(true);
        }
    }
    public void RestarEnergia() 
    {
        if (energia > 0) 
        {
            energia = energia - 1;
            energiaRayo[energia].SetActive(false);
        }
    }
    public int DecirNumeroLlaves() 
    {
        return llaves;
    }
    public void SumarLlaves() 
    {
        if (llaves == 3)
        {
            //no se hace nada
        }
        else
        {
            llaves = llaves + 1;
            llavesArray[llaves - 1].SetActive(true);
        }
    }
    public void Morir() 
    {
        Debug.Log("El player debe morir");
    }
}
