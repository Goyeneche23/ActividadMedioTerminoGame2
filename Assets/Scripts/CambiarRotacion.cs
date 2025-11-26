using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarRotacion : MonoBehaviour
{
    [Header("MovCamara")]
    [SerializeField] private Camera camara;
    [SerializeField] private float anguloInicial;
    private Vector3 objetivo;

    [Header("MovJugador")]
    private Vector2 direccion;
    private Rigidbody2D rb2D;
    private Vector2 input;

    [Header("Velocidades")]
    public float velocidadNormal = 5f;
    public float velocidadLenta = 2.5f; 
    private float velocidadActual;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        velocidadActual = velocidadNormal;
    }

    private void Update()
    {
        objetivo = camara.ScreenToWorldPoint(Input.mousePosition);
        objetivo.z = transform.position.z;

        float anguloRadianes = Mathf.Atan2(objetivo.y - transform.position.y,
                                           objetivo.x - transform.position.x);

        float anguloGrados = anguloRadianes * Mathf.Rad2Deg - anguloInicial;

        transform.rotation = Quaternion.Euler(0, 0, anguloGrados);

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        direccion = input.normalized;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            velocidadActual = velocidadLenta;
        else
            velocidadActual = velocidadNormal;
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direccion * velocidadActual * Time.fixedDeltaTime);
    }
}
