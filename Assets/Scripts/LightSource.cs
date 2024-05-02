using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Light pointLight;
    [SerializeField] private float decreaseSpeed = 0.5f;
    private void Update()
    {
        pointLight.spotAngle -= Time.deltaTime * decreaseSpeed;
    }
}
