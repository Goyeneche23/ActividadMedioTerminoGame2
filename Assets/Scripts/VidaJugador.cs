using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;

    [Header("UI")]
    [SerializeField] private Slider barraVida;
    [SerializeField] private TextMeshProUGUI textoVida;

    [Header("Game Over")]
    [SerializeField] private GameObject panelGameOver;

    [Header("Visual")]
    [SerializeField] private Color colorDano = Color.red;

    private Color colorOriginal;
    private SpriteRenderer sr;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoDano; 
    private AudioSource audioSource;

    private bool estaMuerto = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        vidaActual = vidaMaxima;
        ActualizarUI();

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            colorOriginal = sr.color;

        if (panelGameOver != null)
            panelGameOver.SetActive(false);
    }

    public void RecibirDano(float dano)
    {
        if (estaMuerto) return;

        vidaActual -= dano;
        ActualizarUI();

        if (sr != null)
            StartCoroutine(FlashRojo());

        if (audioSource != null && sonidoDano != null)
        {
            audioSource.PlayOneShot(sonidoDano);
        } 

        if (vidaActual <= 0)
        {
            estaMuerto = true;
            Morir();
            StartCoroutine(AnimacionMorir());
        }
    }

    private IEnumerator FlashRojo()
    {
        sr.color = colorDano;
        yield return new WaitForSeconds(0.1f);
        sr.color = colorOriginal;
    }

    private IEnumerator FlashVerde()
    {
        sr.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sr.color = colorOriginal;
    }

    private IEnumerator AnimacionMorir()
    {
        float duracion = 0.25f;
        float t = 0f;

        Vector3 escalaInicial = transform.localScale;

        while (t < duracion)
        {
            t += Time.deltaTime;

            transform.localScale = Vector3.Lerp(escalaInicial, Vector3.zero, t / duracion);

            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, t / duracion);
                sr.color = c;
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    public void Curar(float cantidad)
    {
        if (estaMuerto) return;

        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMaxima);
        ActualizarUI();

        if (sr != null)
            StartCoroutine(FlashVerde());
    }

    private void ActualizarUI()
    {
        if (barraVida != null)
            barraVida.value = vidaActual / vidaMaxima;

        if (textoVida != null)
            textoVida.text = $"Vida: {Mathf.RoundToInt(vidaActual)}/{vidaMaxima}";
    }

    private void Morir()
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            Time.timeScale = 0f;
        }

        CambiarRotacion movimiento = GetComponent<CambiarRotacion>();
        if (movimiento != null) movimiento.enabled = false;

        Disparo disparoScript = GetComponent<Disparo>();
        if (disparoScript != null) disparoScript.enabled = false;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
