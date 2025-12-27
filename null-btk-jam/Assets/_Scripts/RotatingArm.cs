using UnityEngine;

public class RotatingArm : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private LayerMask stopOnLayers;

    private Rigidbody2D rb;
    private bool isStopped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure the rigidbody is set up to detect collisions
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;
    }

    private void FixedUpdate()
    {
        if (isStopped) return;
        rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void CheckCollision(GameObject other)
    {
        if (isStopped) return;

        if (((1 << other.layer) & stopOnLayers) != 0)
        {
            Debug.Log($"RotatingArm stopped by: {other.name} (Layer: {LayerMask.LayerToName(other.layer)})");
            isStopped = true;
            rb.bodyType = RigidbodyType2D.Static;
            rb.angularVelocity = 0f;
        }
    }
}

