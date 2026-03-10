using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;
        transform.position = mouseWorld;
    }
}