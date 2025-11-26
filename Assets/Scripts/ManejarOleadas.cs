using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ManejarOleadas : MonoBehaviour
{
    [Header("Configuración de Spawn")]
    [SerializeField] private GameObject[] enemyPrefabs;   // MULTI ENEMIGOS
    [SerializeField] private int enemiesToSpawn = 5;
    [SerializeField] private Transform[] spawnPoints;     // MULTI SPAWNS
    [SerializeField] private float spawnDelay = 0.5f;

    [Header("UI Enemigos (TMP)")]
    [SerializeField] private TMP_Text spawnedText;   // ← TEXTO ENEMIGOS GENERADOS
    [SerializeField] private TMP_Text aliveText;     // ← TEXTO ENEMIGOS VIVOS

    private int spawnedCount = 0;

    [Header("UI de Victoria")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Image overlayImage;
    [SerializeField] private TMP_Text victoryText;  // ← CAMBIADO A TMP
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private float delayBeforeVictory = 1f;

    private int currentScene;
    private bool levelCompleted = false;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(LoadNextLevel);

        UpdateEnemyUI();

        // Validaciones
        if (enemyPrefabs.Length == 0)
            Debug.LogError(" No hay prefabs de enemigos asignados.");
        if (spawnPoints.Length == 0)
            Debug.LogError(" No hay puntos de spawn asignados.");

        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        int alive = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        UpdateEnemyUI();

        if (!levelCompleted && (alive == 1 || alive == 0) && spawnedCount == enemiesToSpawn)
        {
            Debug.Log("Fase terminada");
            StartCoroutine(ShowVictoryScreen());

        }
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
                yield break;

            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyObj.tag = "Enemigo"; // TAG correcto y consistente

            Debug.Log(" Enemigo spawneado en: " + spawnPoint.position);

            spawnedCount++;
            UpdateEnemyUI();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void UpdateEnemyUI()
    {
        if (spawnedText != null)
            spawnedText.text = $"Spawned: {spawnedCount}/{enemiesToSpawn}";

        if (aliveText != null)
            aliveText.text = $"Vivos: {GameObject.FindGameObjectsWithTag("Enemigo").Length}";
    }

    IEnumerator ShowVictoryScreen()
    {
        levelCompleted = true;

        yield return new WaitForSeconds(delayBeforeVictory);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            if (overlayImage != null)
                overlayImage.color = new Color(1f, 1f, 0f, 0.3f);

            if (victoryText != null)
                victoryText.text = $"¡Fase {currentScene + 1} Terminada!";
        }
    }

    void LoadNextLevel()
    {
        int nextScene = currentScene + 1;

        if (nextScene <= SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextScene);
    }
}
