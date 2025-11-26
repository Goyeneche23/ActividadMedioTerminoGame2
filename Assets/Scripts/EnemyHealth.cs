using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public float vidaMax = 10f;
    private float vidaActual;

    [Header("Flash de daño")]
    public Color colorDano = Color.red;
    private Color colorOriginal;
    private SpriteRenderer sr;

    [Header("Drop al morir")]
    public GameObject dropPrefab;
    [Range(0f, 1f)] public float probabilidadDrop = 0.1f;

    [Header("Audio")]
    public AudioClip sonidoMuerte; 
    private AudioSource audioSource;

    private void Start()
    {
        vidaActual = vidaMax;
        audioSource = GetComponent<AudioSource>(); 
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            colorOriginal = sr.color;
    }

    public void RecibirDano(float dano)
    {
        vidaActual -= dano;

        if (sr != null)
            StartCoroutine(FlashRojo());

        if (vidaActual <= 0)
            StartCoroutine(AnimacionMorir());
    }

    private IEnumerator FlashRojo()
    {
        sr.color = colorDano;
        yield return new WaitForSeconds(0.1f);
        sr.color = colorOriginal;
    }

    private IEnumerator AnimacionMorir()
    {
        if (audioSource != null && sonidoMuerte != null)
        {
            audioSource.PlayOneShot(sonidoMuerte);
        }
        if (dropPrefab != null && Random.value <= probabilidadDrop)
            Instantiate(dropPrefab, transform.position, Quaternion.identity);

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
}
