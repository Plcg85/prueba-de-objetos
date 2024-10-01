using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerCanionMultiple : MonoBehaviour
{
    [SerializeField] private AudioClip[] audios;
    private AudioSource controlAudio;

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }
    public void SeleccionAudio(int indice, float volumen)
    {
        controlAudio.PlayOneShot(audios[indice], volumen);
    }
    public void PararSonido()
    {
        controlAudio.Stop();
    }
}
