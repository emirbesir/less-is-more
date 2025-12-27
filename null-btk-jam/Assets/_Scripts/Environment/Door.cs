using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3, 0);
    [SerializeField] private float duration = 0.5f;

    private Vector3 closedPosition;
    private bool isOpen = false;

    void Start()
    {
        closedPosition = transform.position;
    }

    // Call this from PressurePlate -> OnPressed
    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        transform.DOMove(closedPosition + openOffset, duration).SetEase(Ease.OutCubic);
    }

    // Call this from PressurePlate -> OnReleased
    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        transform.DOMove(closedPosition, duration).SetEase(Ease.OutBounce);
    }
}