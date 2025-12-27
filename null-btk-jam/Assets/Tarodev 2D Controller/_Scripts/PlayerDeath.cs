using System.Collections.Generic;
using TarodevController;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PhysicsMaterial2D _corpsePhysicsMaterial;
    [SerializeField] private int _maxCorpses = 50;

    private static readonly List<GameObject> _corpses = new();
    private static Vector3 _respawnPosition;
    private static bool _hasRespawnPos;

    private IPlayerController _controller;
    private Rigidbody2D _rb;
    private Collider2D _col;
    private PlayerAnimator _playerAnimator;
    private Animator _animator;

    private void Awake()
    {
        _controller = GetComponent<IPlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
        _animator = _playerAnimator?.GetComponentInChildren<Animator>();

        // Initialize respawn position if it's the first player
        if (!_hasRespawnPos)
        {
            _respawnPosition = transform.position;
            _hasRespawnPos = true;
        }
    }

    private void OnEnable()
    {
        if (_controller != null) _controller.GroundedChanged += OnGroundedChanged;
    }

    private void OnDisable()
    {
        if (_controller != null) _controller.GroundedChanged -= OnGroundedChanged;
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        if (grounded)
        {
            _respawnPosition = transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }

    public void Die()
    {
        // 1. Disable Controller & Animations
        if (_controller is MonoBehaviour mb) mb.enabled = false;
        if (_animator != null) _animator.enabled = false;

        // 2. Change Layer to Ground
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer != -1) gameObject.layer = groundLayer;
        else Debug.LogWarning("Layer 'Ground' not found. Corpse layer not changed.");

        // 3. Handle Physics (Ragdoll & Friction)
        if (_corpsePhysicsMaterial != null)
        {
            _col.sharedMaterial = _corpsePhysicsMaterial;
        }
        
        // Freeze in place to act as a solid block
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Static;

        // 4. Manage Corpses
        _corpses.Add(gameObject);
        if (_corpses.Count > _maxCorpses)
        {
            GameObject old = _corpses[0];
            _corpses.RemoveAt(0);
            if (old != null) Destroy(old);
        }

        // 5. Respawn
        if (_playerPrefab != null)
        {
            var newPlayer = Instantiate(_playerPrefab, _respawnPosition, Quaternion.identity);
            // Ensure the controller is enabled on the new player
            if (newPlayer.TryGetComponent(out IPlayerController newController) && newController is MonoBehaviour newMb)
            {
                newMb.enabled = true;
            }

            if (newPlayer.TryGetComponent<Rigidbody2D>(out var newRb))
            {
                newRb.bodyType = RigidbodyType2D.Dynamic;
            }

            var playerAnimator = newPlayer.GetComponentInChildren<PlayerAnimator>();
            var animator = playerAnimator?.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
            }

        }
        else
        {
            Debug.LogError("Player Prefab not assigned in PlayerDeath!");
        }

        // 6. Remove this script so it's no longer a "player"
        Destroy(this);
    }
}
