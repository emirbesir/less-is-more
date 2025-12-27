using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect; // 0 = moves with camera (static), 1 = doesn't move (far background)

    void Start()
    {
        startpos = transform.position.x;
        // Get the length of the sprite to know when to loop
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate() // Using FixedUpdate or LateUpdate helps reduce jitter with Cinemachine
    {
        // How far we have moved relative to the camera
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        
        // How far we should move the background object
        float dist = (cam.transform.position.x * parallaxEffect);

        // Move the background
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Infinite Scrolling Logic: Check if camera moved past the bounds
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}