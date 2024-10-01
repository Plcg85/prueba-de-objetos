using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoRodilloPinchos : MonoBehaviour
{
    private AudioManager audioManager;
    private int sonidoRodillo = 16;
    private float volumenRodillo = 1f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
