using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 3f;
    public float distanciaStop = 4f;

    [Header("Disparo")]
    public GameObject balaPrefab;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 1.5f;
    public float velocidadBala = 5.5f;

    private float tiempoDisparoActual;
    private Transform jugador;

    private void Start()
    {
        jugador = GameObject.Find("Jugador").transform;
    }

    private void Update()
    {
        Mover();
        Disparar();
    }

    private void Mover()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia > distanciaStop)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                jugador.position,
                velocidad * Time.deltaTime
            );
        }
    }

    private void Disparar()
    {
        tiempoDisparoActual -= Time.deltaTime;

        if (tiempoDisparoActual <= 0)
        {
            Vector2 direccion = (jugador.position - puntoDisparo.position).normalized;

            GameObject bala = Instantiate(
                balaPrefab,
                puntoDisparo.position,
                Quaternion.LookRotation(Vector3.forward, direccion)
            );

            Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direccion * velocidadBala;

            tiempoDisparoActual = tiempoEntreDisparos;
        }
    }

}
