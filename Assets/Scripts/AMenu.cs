using UnityEngine;
using UnityEngine.SceneManagement;

public class AMenu : MonoBehaviour
{
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}