using System.Collections;
using UnityEngine;

public class DashControl : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 35f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.8f;

    private bool isDashing = false;
    private bool canDash = true;
    private Vector2 dashDirection;
    private Vector2 lastMovementDirection = Vector2.right;

    private void Update()
    {
        if (!isDashing)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input != Vector2.zero)
                lastMovementDirection = input.normalized;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
        {
            dashDirection = lastMovementDirection;
            StartCoroutine(PerformDash());
        }

        if (isDashing)
        {
            transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}
