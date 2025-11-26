using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossBulletHell : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public Slider healthSlider;
    public GameObject healthBarContainer;

    public float vidaMaxima = 800f;
    public float vidaActual;

    public float timeBetweenAttacks = 2f;
    public float bulletSpeed = 5f;
    public float spiralSpeed = 3f;

    public AudioSource audioSource;   
    public AudioClip sonidoDano;  

    public float moveSpeed = 2f;
    public float distanciaStop = 4f;

    private Gradient healthGradient;
    private int currentPhase = 0;
    private int patternIndex = 0;
    private float attackTimer;
    private bool isAttacking = false;
    private bool isDead = false;
    private Vector2 targetPosition;
    private Transform player;

    void Start()
    {
        vidaActual = vidaMaxima;
        attackTimer = 1.5f;
        targetPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        InitializeHealthBar();
    }

    void InitializeHealthBar()
    {
        healthGradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        colorKeys[0] = new GradientColorKey(Color.red, 0f);
        colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f);
        colorKeys[2] = new GradientColorKey(Color.green, 1f);
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);
        healthGradient.SetKeys(colorKeys, alphaKeys);
        if (healthSlider != null)
        {
            healthSlider.maxValue = vidaMaxima;
            healthSlider.value = vidaMaxima;
        }
        if (healthBarContainer != null)
            healthBarContainer.SetActive(true);
    }


    void Update()
    {
        if (isDead) return;
        move();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && !isAttacking)
        {
            ExecutePhasePattern();
            attackTimer = timeBetweenAttacks;
        }
    }

    void move()
    {
        if (player == null) return;
        float distancia = Vector2.Distance(transform.position, player.position);
        if (distancia > distanciaStop)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void ExecutePhasePattern()
    {
        if (currentPhase == 0)
        {
            switch (patternIndex)
            {
                case 0: StartCoroutine(CircularBurstPattern()); break;
                case 1: StartCoroutine(DirectedShotgun()); break;
                case 2: StartCoroutine(SpinningSpiral()); break;
                case 3: StartCoroutine(StarPattern()); break;
            }
            patternIndex = (patternIndex + 1) % 4;
        }
        else if (currentPhase == 1)
        {
            switch (patternIndex)
            {
                case 0: StartCoroutine(DoubleSpiral()); break;
                case 1: StartCoroutine(DirectedShotgun()); break;
                case 2: StartCoroutine(WavePattern()); break;
                case 3: StartCoroutine(StarPattern()); break;
                case 4: StartCoroutine(CircularBurstPattern()); break;
            }
            patternIndex = (patternIndex + 1) % 5;
        }
        else if (currentPhase == 2)
        {
            switch (patternIndex)
            {
                case 0: StartCoroutine(RageSpiral()); break;
                case 1: StartCoroutine(BulletHellRain()); break;
                case 2: StartCoroutine(DoubleStarPattern()); break;
                case 3: StartCoroutine(DoubleSpiral()); break;
                case 4: StartCoroutine(DirectedShotgun()); break;
            }
            patternIndex = (patternIndex + 1) % 5;
        }
    }

    IEnumerator StarPattern()
    {
        isAttacking = true;
        int waves = 3;
        int points = 5;
        int bulletsPerPoint = 8;
        for (int w = 0; w < waves; w++)
        {
            float offsetAngle = w * 12f;
            for (int p = 0; p < points; p++)
            {
                float baseAngle = (p * 360f / points) + offsetAngle;
                for (int b = 0; b < bulletsPerPoint; b++)
                {
                    float spread = (b - bulletsPerPoint / 2f) * 3f;
                    float finalAngle = baseAngle + spread;
                    float speedVariation = 1f + (b * 0.05f);
                    ShootBullet(finalAngle, bulletSpeed * speedVariation);
                }
            }
            yield return new WaitForSeconds(0.6f);
        }
        isAttacking = false;
    }

    IEnumerator DoubleStarPattern()
    {
        isAttacking = true;
        int waves = 4;
        int points = 6;
        int bulletsPerPoint = 10;
        for (int w = 0; w < waves; w++)
        {
            float offsetAngle1 = w * 15f;
            float offsetAngle2 = w * -15f + 30f;
            for (int p = 0; p < points; p++)
            {
                float baseAngle = (p * 360f / points) + offsetAngle1;
                for (int b = 0; b < bulletsPerPoint; b++)
                {
                    float spread = (b - bulletsPerPoint / 2f) * 2.5f;
                    float finalAngle = baseAngle + spread;
                    float speedVariation = 1f + (b * 0.04f);
                    ShootBullet(finalAngle, bulletSpeed * speedVariation * 1.1f);
                }
            }
            for (int p = 0; p < points; p++)
            {
                float baseAngle = (p * 360f / points) + offsetAngle2;
                for (int b = 0; b < bulletsPerPoint; b++)
                {
                    float spread = (b - bulletsPerPoint / 2f) * 2.5f;
                    float finalAngle = baseAngle + spread;
                    float speedVariation = 1f + (b * 0.04f);
                    ShootBullet(finalAngle, bulletSpeed * speedVariation * 0.9f);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        isAttacking = false;
    }

    IEnumerator CircularBurstPattern()
    {
        isAttacking = true;
        int bursts = 3;
        int bulletsPerBurst = 16;
        for (int i = 0; i < bursts; i++)
        {
            float offsetAngle = i * 11.25f;
            for (int j = 0; j < bulletsPerBurst; j++)
            {
                float angle = (j * 360f / bulletsPerBurst) + offsetAngle;
                ShootBullet(angle, bulletSpeed);
            }
            yield return new WaitForSeconds(0.5f);
        }
        isAttacking = false;
    }

    IEnumerator DirectedShotgun()
    {
        isAttacking = true;
        for (int i = 0; i < 5; i++)
        {
            if (player != null)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                for (int j = -3; j <= 3; j++)
                    ShootBullet(baseAngle + (j * 15f), bulletSpeed * 1.2f);
            }
            yield return new WaitForSeconds(0.3f);
        }
        isAttacking = false;
    }

    IEnumerator SpinningSpiral()
    {
        isAttacking = true;
        float angle = 0f;
        int totalBullets = 60;
        for (int i = 0; i < totalBullets; i++)
        {
            for (int j = 0; j < 3; j++)
                ShootBullet(angle + (j * 120f), bulletSpeed * 0.8f);
            angle += spiralSpeed * 8f;
            yield return new WaitForSeconds(0.06f);
        }
        isAttacking = false;
    }

    IEnumerator DoubleSpiral()
    {
        isAttacking = true;
        float angle1 = 0f;
        float angle2 = 180f;
        int totalBullets = 50;
        for (int i = 0; i < totalBullets; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                ShootBullet(angle1 + (j * 90f), bulletSpeed);
                ShootBullet(angle2 + (j * 90f), bulletSpeed * 0.9f);
            }
            angle1 += spiralSpeed * 10f;
            angle2 -= spiralSpeed * 10f;
            yield return new WaitForSeconds(0.07f);
        }
        isAttacking = false;
    }

    IEnumerator WavePattern()
    {
        isAttacking = true;
        for (int wave = 0; wave < 4; wave++)
        {
            int bulletsInWave = 20;
            float waveSpeed = bulletSpeed * (0.6f + wave * 0.2f);
            for (int i = 0; i < bulletsInWave; i++)
                ShootBullet(i * 360f / bulletsInWave, waveSpeed);
            yield return new WaitForSeconds(0.4f);
        }
        isAttacking = false;
    }

    IEnumerator RageSpiral()
    {
        isAttacking = true;
        float angle = 0f;
        int totalBullets = 80;
        for (int i = 0; i < totalBullets; i++)
        {
            for (int j = 0; j < 5; j++)
                ShootBullet(angle + (j * 72f), bulletSpeed * 1.1f);
            angle += spiralSpeed * 12f;
            yield return new WaitForSeconds(0.04f);
        }
        isAttacking = false;
    }

    IEnumerator BulletHellRain()
    {
        isAttacking = true;
        for (int i = 0; i < 30; i++)
        {
            int bullets = Random.Range(8, 12);
            float baseAngle = Random.Range(0f, 360f);
            for (int j = 0; j < bullets; j++)
                ShootBullet(baseAngle + (j * 360f / bullets), bulletSpeed * Random.Range(0.8f, 1.3f));
            yield return new WaitForSeconds(0.15f);
        }
        isAttacking = false;
    }

    void ShootBullet(float angle, float speed)
    {
        if (firePoint == null) return;
        GameObject bullet = null;
        if (BulletPool.Instance != null)
        {
            BulletHell pooledBullet = BulletPool.Instance.requestBullet();
            if (pooledBullet != null)
            {
                bullet = pooledBullet.gameObject;
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = Quaternion.identity;
            }
        }
        else if (bulletPrefab != null)
            bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        if (bullet == null) return;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        BulletHell script = bullet.GetComponent<BulletHell>();
        if (script != null)
        {
            script.SetDirection(dir);
            script.speed = speed;
            script.ResetTimer();
        }
    }

    public void RecibirDano(float dano)
    {
        vidaActual -= dano;
        if (healthSlider != null)
            healthSlider.value = vidaActual;
 
        if (audioSource != null && sonidoDano != null)
        audioSource.PlayOneShot(sonidoDano);

        float porcentajeVida = vidaActual / vidaMaxima;
        if (porcentajeVida <= 0.30f && currentPhase < 2)
        {
            currentPhase = 2;
            timeBetweenAttacks = 1.2f;
            moveSpeed = 3.5f;
            patternIndex = 0;
        }
        else if (porcentajeVida <= 0.60f && currentPhase < 1)
        {
            currentPhase = 1;
            timeBetweenAttacks = 1.6f;
            moveSpeed = 2.8f;
            patternIndex = 0;
        }
        if (vidaActual <= 0)
        {
            isDead = true;
        }
    }

}
