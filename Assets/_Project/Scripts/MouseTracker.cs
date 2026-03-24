using UnityEngine;

public class MouseTracker : MonoBehaviour
{
    private Camera mainCam;
    public TrainingManager trainingManager; // Arrastra el objeto con el TrainingManager aquí

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        // Si el modo automático está encendido, el script manual se detiene
        if (trainingManager != null && trainingManager.autoMoveMouse) return;

        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;
        transform.position = mouseWorld;
    }
}