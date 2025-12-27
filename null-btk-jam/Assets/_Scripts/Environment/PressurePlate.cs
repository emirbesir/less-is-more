using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class PressurePlate : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;
    
    [Header("Tags")]
    [SerializeField] private LayerMask acceptableLayers;
    
    [Header("Visuals")]
    [SerializeField] private Transform plateVisual;
    [SerializeField] private float pressedOffset = -0.1f;

    private int objectsOnPlate = 0;
    private Vector3 originalPos;

    void Start()
    {
        if(plateVisual) originalPos = plateVisual.localPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check for Player OR DeadCube
        if (acceptableLayers == (acceptableLayers | (1 << other.gameObject.layer)))
        {
            objectsOnPlate++;
            if (objectsOnPlate >= 1)
            {
                Press();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (acceptableLayers == (acceptableLayers | (1 << other.gameObject.layer)))
        {
            objectsOnPlate--;
            if (objectsOnPlate <= 0)
            {
                objectsOnPlate = 0; // Safety clamp
                Release();
            }
        }
    }

    void Press()
    {
        OnPressed?.Invoke();
        if(plateVisual) plateVisual.DOLocalMoveY(originalPos.y + pressedOffset, 0.1f);
    }

    void Release()
    {
        OnReleased?.Invoke();
        if(plateVisual) plateVisual.DOLocalMoveY(originalPos.y, 0.1f);
    }
}