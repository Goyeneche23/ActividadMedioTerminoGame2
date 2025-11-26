using UnityEngine;
using TMPro;

public class ContadorBalas : MonoBehaviour
{
    public static ContadorBalas Instance;

    [SerializeField] private TextMeshProUGUI textoContador;
    private int balasActivas = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementarBalas()
    {
        balasActivas++;
        ActualizarUI();
    }

    public void DecrementarBalas()
    {
        balasActivas--;
        if (balasActivas < 0) balasActivas = 0;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (textoContador != null)
        {
            textoContador.text = $"Balas Enemigas: {balasActivas}";
            
            // Cambiar color según cantidad
            if (balasActivas > 200)
                textoContador.color = Color.red;
            else if (balasActivas > 100)
                textoContador.color = Color.yellow;
            else
                textoContador.color = Color.white;
        }
    }
}