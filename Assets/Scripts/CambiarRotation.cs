using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarRotation : MonoBehaviour
{
    private Transform objetivo;

    private void Start()
    {
        objetivo = GameObject.Find("Jugador").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        
      float anguloRadianes = Mathf.Atan2(objetivo.position.y - transform.position.y, objetivo.position.x - transform.position.x);
        float anguloGrados = (180 / Mathf.PI) * anguloRadianes ;
        transform.rotation = Quaternion.Euler(0, 0, anguloGrados);
    }
}
