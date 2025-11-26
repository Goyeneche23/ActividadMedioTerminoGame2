using UnityEngine;

public class bala : MonoBehaviour
{
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float dano = 10f;
    [SerializeField] private float tiempoVida = 5f;
    [SerializeField] private bool esDelJugador = true;

    private void Start()
    {
        Destroy(gameObject, tiempoVida);
        
        if (!esDelJugador)
        {
            if (ContadorBalas.Instance != null)
                ContadorBalas.Instance.IncrementarBalas();
        }
    }

    private void OnDestroy()
    {
        if (!esDelJugador)
        {
            if (ContadorBalas.Instance != null)
                ContadorBalas.Instance.DecrementarBalas();
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.up * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
            return;
        }

        if (esDelJugador && other.CompareTag("Enemigo"))
        {
            EnemyHealth vidaEnemigo = other.GetComponent<EnemyHealth>();

            if (vidaEnemigo != null)
                vidaEnemigo.RecibirDano(dano);

            Destroy(gameObject);
            return;
        }

        if (!esDelJugador && other.CompareTag("Player"))
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();

            if (vidaJugador != null)
                vidaJugador.RecibirDano(dano);

            Destroy(gameObject);
            return;
        }
        

        if (esDelJugador && other.CompareTag("Boss"))
        {
            Debug.Log("Boss hit by player bullet");
        BossBulletHell boss = other.GetComponentInParent<BossBulletHell>();


            if (boss != null){
                boss.RecibirDano(dano);
            }

            Destroy(gameObject);
            return;
        }
    }
}
