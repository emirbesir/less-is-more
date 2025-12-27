using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float lengthX, lengthY;
    private float startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffectX; // 0 = moves with camera (static), 1 = doesn't move (far background)
    public float parallaxEffectY; // 0 = moves with camera (static), 1 = doesn't move (far background)
    public bool infiniteScrollX = true;
    public bool infiniteScrollY = false;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        // Get the size of the sprite to know when to loop
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        lengthX = sr.bounds.size.x;
        lengthY = sr.bounds.size.y;
    }

    void LateUpdate() // Using FixedUpdate or LateUpdate helps reduce jitter with Cinemachine
    {
        // How far we have moved relative to the camera
        float tempX = (cam.transform.position.x * (1 - parallaxEffectX));
        float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        
        // How far we should move the background object
        float distX = (cam.transform.position.x * parallaxEffectX);
        float distY = (cam.transform.position.y * parallaxEffectY);

        // Move the background
        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        // Infinite Scrolling Logic: Check if camera moved past the bounds (X-axis)
        if (infiniteScrollX)
        {
            if (tempX > startPosX + lengthX) startPosX += lengthX;
            else if (tempX < startPosX - lengthX) startPosX -= lengthX;
        }

        // Infinite Scrolling Logic: Check if camera moved past the bounds (Y-axis)
        if (infiniteScrollY)
        {
            if (tempY > startPosY + lengthY) startPosY += lengthY;
            else if (tempY < startPosY - lengthY) startPosY -= lengthY;
        }
    }
}