using UnityEngine;

public class EnemigoKamikaze : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 4f;

    [Header("Daño por contacto")]
    [SerializeField] private float danoContacto = 100f;

    private Transform jugador;
    private Rigidbody2D rb2D;

    private void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;

        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (jugador != null)
        {
            Vector2 direccion = (jugador.position - transform.position).normalized;
            rb2D.linearVelocity = direccion * velocidad;

            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angulo);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();
            if (vidaJugador != null)
                vidaJugador.RecibirDano(danoContacto);

        }
    }
}
