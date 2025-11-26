using UnityEngine;

public class EnemigoDisparador : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] private GameObject balaEnemiga;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float intervaloDisparo = 2f;
    [SerializeField] private int balasporRafaga = 3;
    [SerializeField] private float anguloDispersion = 20f;

    private Transform jugador;
    private float tiempoUltimoDisparo;
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float distanciaStop = 2f;

    private void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;

        tiempoUltimoDisparo = Time.time + Random.Range(0f, 1f);
    }

    private void Update()
    {
        Mover();
        if (jugador != null && Time.time >= tiempoUltimoDisparo + intervaloDisparo)
        {
            DispararAlJugador();
            tiempoUltimoDisparo = Time.time;
        }
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

    private void DispararAlJugador()
{
    // Dirección hacia el jugador
    Vector2 dir = (jugador.position - puntoDisparo.position).normalized;

    // Angulo hacia el jugador (para que Vector2.up de la bala apunte hacia allá)
    float anguloCentral = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

    // Disparo tipo escopeta
    for (int i = 0; i < balasporRafaga; i++)
    {
        float offset = 0f;

        if (balasporRafaga > 1)
        {
            float paso = anguloDispersion / (balasporRafaga - 1);
            offset = -anguloDispersion / 2 + paso * i;
        }

        Quaternion rot = Quaternion.Euler(0, 0, anguloCentral + offset);

        Instantiate(balaEnemiga, puntoDisparo.position, rot);
    }
}



}
