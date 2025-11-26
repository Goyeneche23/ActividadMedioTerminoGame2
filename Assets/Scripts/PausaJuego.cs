using UnityEngine;

public class PausaJuego : MonoBehaviour
{
    public GameObject panelPausa; 
    private bool estaPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AlternarPausa();
        }
    }

    void AlternarPausa()
    {
        estaPausado = !estaPausado;

        panelPausa.SetActive(estaPausado);
        Time.timeScale = estaPausado ? 0f : 1f;
    }
}
