using UnityEngine;
using TMPro;

public class Disparo : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject bala;
    [SerializeField] private float tiempoEntreDisparos = 0.2f;

    [Header("Munición")]
    [SerializeField] private int municionMaxima = 30;
    [SerializeField] private int municionActual = 30;
    [SerializeField] private float tiempoRecarga = 2f;
    private bool recargando = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoMunicion;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoDisparo; 
    private AudioSource audioSource;

    private float tiempoUltimoDisparo;

    private void Start()
    {
        municionActual = municionMaxima;
        ActualizarUI();
    }

    private void Update()
    {
        // Disparar
        if (Input.GetButton("Fire1") && !recargando && municionActual > 0 
            && Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }

        // Recargar manual
        if (Input.GetKeyDown(KeyCode.R) && municionActual < municionMaxima && !recargando)
        {
            StartCoroutine(Recargar());
        }

        // Recarga automática cuando se acaba
        if (municionActual <= 0 && !recargando)
        {
            StartCoroutine(Recargar());
        }

        ActualizarUI();
    }

    private void Disparar()
    {
        Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);
        municionActual--;
        if (audioSource != null && sonidoDisparo != null)
        {
            audioSource.PlayOneShot(sonidoDisparo);
        }
    }

    private System.Collections.IEnumerator Recargar()
    {
        recargando = true;
        yield return new WaitForSeconds(tiempoRecarga);
        municionActual = municionMaxima;
        recargando = false;
    }

    private void ActualizarUI()
    {
        if (textoMunicion != null)
        {
            if (recargando)
            {
                textoMunicion.text = "RECARGANDO...";
                textoMunicion.color = Color.yellow;
            }
            else
            {
                textoMunicion.text = $"Munición: {municionActual}/{municionMaxima}";
                textoMunicion.color = municionActual > 10 ? Color.white : Color.red;
            }
        }
    }
}
