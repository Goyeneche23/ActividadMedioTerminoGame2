using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Valores")]
    [SerializeField] private float cantidadVida = 50f;

    private void OnTriggerEnter2D(Collider2D collision){
        var Jugador = collision.GetComponent<CambiarRotacion>();
        if(collision.CompareTag("Player")){
            AplicarEfecto(collision.gameObject);
            Destroy(gameObject);
        }
    }
    private void AplicarEfecto(GameObject jugador){
       CurarJugador(jugador, cantidadVida);
    }
    private void CurarJugador(GameObject jugador, float cantidad){
        VidaJugador vidaJugador = jugador.GetComponent<VidaJugador>();
        if(vidaJugador != null){
            vidaJugador.Curar(cantidad);
        }
    }
}
