using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [Header("Laser Settings")] [SerializeField]
    private float maxDistance = 20f;

    [SerializeField] private LayerMask collisionMask; // Must include Player, Ground, and DeadCube
    [SerializeField] private float timeToKill = 0.25f;
    [SerializeField] private Vector2 direction = Vector2.right; // Local direction

    [Header("Visual Feedback")] [SerializeField]
    private Color safeColor = Color.red;

    [SerializeField] private Color burningColor = Color.white;
    [SerializeField] private float widthMultiplier = 1.5f;

    [Header("Particles")] [SerializeField] private Transform laserParticles;
    private ParticleSystem particleSystem;

    private LineRenderer lineRenderer;
    private float exposureTimer = 0f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;

        if (laserParticles != null)
        {
            particleSystem = laserParticles.GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        // Calculate direction in world space so we can rotate the object
        Vector2 worldDirection = transform.TransformDirection(direction);

        // 1. Raycast
        // This naturally handles "not getting behind objects". The ray stops at the first thing it hits.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, worldDirection, maxDistance, collisionMask);

        Vector3 endPoint;

        // 2. Logic
        if (hit.collider != null)
        {
            endPoint = hit.point;

            if (hit.collider.CompareTag("Player"))
            {
                // We are hitting the player directly
                HandleExposure(hit.collider);
            }
            else
            {
                // We hit a Wall or a DeadCube. 
                // The laser stops here, protecting anyone behind it.
                ResetExposure();
            }
        }
        else
        {
            // We hit nothing, extend to max range
            endPoint = transform.position + (Vector3)(worldDirection * maxDistance);
            ResetExposure();
        }

        // 3. Draw Laser
        DrawLaser(endPoint);
    }

    void HandleExposure(Collider2D player)
    {
        PlayerDeath deathScript = player.GetComponent<PlayerDeath>();

        // Don't accumulate exposure on already dead players
        if (deathScript == null || deathScript.IsDead)
        {
            ResetExposure();
            return;
        }

        exposureTimer += Time.deltaTime;

        // Visual Juice: Flash to white as death approaches
        float t = Mathf.Clamp01(exposureTimer / timeToKill);
        Color currentColor = Color.Lerp(safeColor, burningColor, t);
        
        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;
        lineRenderer.startWidth = Mathf.Lerp(0.1f, 0.1f * widthMultiplier, t);

        UpdateParticleColor(currentColor);

        if (exposureTimer >= timeToKill)
        {
            deathScript.Die(1);
            exposureTimer = 0f;
        }
    }

    void ResetExposure()
    {
        if (exposureTimer > 0)
        {
            exposureTimer = 0f;
            lineRenderer.startColor = safeColor;
            lineRenderer.endColor = safeColor;
            lineRenderer.startWidth = 0.1f;
            UpdateParticleColor(safeColor);
        }
    }

    void UpdateParticleColor(Color color)
    {
        if (particleSystem != null)
        {
            var main = particleSystem.main;
            main.startColor = color;
        }
    }

    void DrawLaser(Vector3 endPos)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPos);

        // Move particles to laser hit point
        if (laserParticles != null)
        {
            laserParticles.position = endPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = transform.TransformDirection(direction);
        Gizmos.DrawRay(transform.position, dir * maxDistance);
    }
}