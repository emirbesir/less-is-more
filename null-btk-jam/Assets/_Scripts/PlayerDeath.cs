using System.Collections.Generic;
using TarodevController;
using Unity.Cinemachine;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class PlayerDeath : MonoBehaviour
{
    public event Action<int> OnDeath;

    [Header("Settings")] [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PhysicsMaterial2D _corpsePhysicsMaterial;
    [SerializeField] private Sprite _deadSprite;
    [SerializeField] private int _maxCorpses = 50;
    [SerializeField] private Transform _spawnPoint; // Kept for initial spawn reference if neededp
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private Light2D _light2D;

    public bool IsDead { get; private set; }

    private static readonly List<GameObject> _corpses = new();
    private static Vector3 _respawnPosition;
    private static bool _hasRespawnPos;

    private IPlayerController _controller;
    private Rigidbody2D _rb;
    private Collider2D _col;
    private PlayerAnimator _playerAnimator;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private AudioSource _audioSource;

    private void Awake()
    {
        _controller = GetComponent<IPlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
        _animator = _playerAnimator?.GetComponentInChildren<Animator>();
        _renderer = _playerAnimator?.GetComponentInChildren<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        if (!_hasRespawnPos)
        {
            if (_spawnPoint != null)
                _respawnPosition = _spawnPoint.position;
            else
                _respawnPosition = transform.position;
        }

        transform.position = _respawnPosition;
    }

    private void Start()
    {
        GameManager.Instance.SetPlayerDeathReference(this);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnLevelCompleted += ResetSpawnPoint;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelCompleted -= ResetSpawnPoint;
    }
    
    private void ResetSpawnPoint()
    {
        if (_spawnPoint != null)
        {
            _hasRespawnPos = false;
        }
    }

    public static void SetRespawnPoint(Vector3 pos)
    {
        _respawnPosition = pos;
        _hasRespawnPos = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die(1);
        }
    }

    public void Die(int amount)
    {
        if (IsDead) return;
        IsDead = true;

        // 0. Respawn FIRST (Before modifying this object)
        if (_playerPrefab != null)
        {
            var newPlayer = Instantiate(_playerPrefab, _respawnPosition, Quaternion.identity);

            // Update Cinemachine target
            var cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
            if (cinemachineCamera != null)
            {
                cinemachineCamera.Target.TrackingTarget = newPlayer.transform;
            }

            // Ensure the new player is clean and dynamic
            if (newPlayer.TryGetComponent<Rigidbody2D>(out var newRb))
            {
                newRb.bodyType = RigidbodyType2D.Dynamic;
                newRb.linearVelocity = Vector2.zero;
                newRb.angularVelocity = 0f;
            }

            // Ensure controller is enabled
            if (newPlayer.TryGetComponent(out IPlayerController newController) && newController is MonoBehaviour newMb)
            {
                newMb.enabled = true;
            }

            // Ensure animations are enabled
            var newPlayerAnim = newPlayer.GetComponentInChildren<PlayerAnimator>();
            if (newPlayerAnim != null) newPlayerAnim.enabled = true;

            var newAnim = newPlayerAnim?.GetComponentInChildren<Animator>();
            if (newAnim != null) newAnim.enabled = true;
        }
        else
        {
            Debug.LogError("Player Prefab not assigned in PlayerDeath!");
        }

        // 1. Disable Controller & Animations
        if (_controller is MonoBehaviour mb) mb.enabled = false;
        if (_animator != null)
        {
            _animator.enabled = false;
            _animator.transform.localScale = Vector3.one;
        }

        if (_playerAnimator != null) _playerAnimator.enabled = false;

        // 2. Change Layer to Ground
        int groundLayer = LayerMask.NameToLayer("DeadBody");
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
        _rb.bodyType = RigidbodyType2D.Kinematic;

        // 4. Manage Corpses
        _corpses.Add(gameObject);
        if (_corpses.Count > _maxCorpses)
        {
            GameObject old = _corpses[0];
            _corpses.RemoveAt(0);
            if (old != null) Destroy(old);
        }
        
        // Update Sprite to Dead Sprite if assigned
        _renderer.sprite = _deadSprite;
        // Play Death Sound
        if (_audioSource != null && _deathSound != null)
        {
            _audioSource.PlayOneShot(_deathSound);
        }
        // Disable Light on Death
        if (_light2D != null)
        {
            _light2D.enabled = false;
        }
        
        // Volume Shake on Death
        if (VolumeShaker.Instance != null) VolumeShaker.Instance.ShakeDeath();

        // Trigger OnDeath event
        OnDeath?.Invoke(amount);

        // 5. Remove this script so it's no longer a "player"
        Destroy(this);
    }
}