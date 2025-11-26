using UnityEngine;

public class BulletHell : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 5f;
    public float lifetime = 10f;
    public int damage = 1;
    
    [Header("Visual Settings")]
    public bool rotateWithMovement = true;
    public float rotationSpeed = 0f;
    
    private Vector2 direction;
    private float timer;
    private float currentRotation;

    void OnEnable()
    {
        ResetTimer();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        
        if (rotationSpeed != 0)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ReturnToPool();
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        
        if (rotateWithMovement)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            currentRotation = angle - 90;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
    }
    
    public void SetRotationSpeed(float rotSpeed)
    {
        rotationSpeed = rotSpeed;
    }


    public void ResetTimer()
    {
        timer = lifetime;
    }


    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();

            if (vidaJugador != null)
                Debug.Log("Player hit by bullet");
                vidaJugador.RecibirDano(damage);

            ReturnToPool();
            return;
        }
        
        if (other.CompareTag("Obstaculo"))
        {
            ReturnToPool();
            return;
        }
    }
}