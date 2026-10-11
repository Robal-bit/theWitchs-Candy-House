using UnityEngine;
using UnityEngine.Rendering.Universal;

// WEEK 6: A Point Light 2D casts a visible mint glow across the lit floor.
// PingPong changes its intensity gently; Time.time stops advancing while paused.
public class WitchCauldronGlow : MonoBehaviour
{
    [SerializeField] private Light2D cauldronLight;
    [SerializeField] private float minimumIntensity = 0.9f;
    [SerializeField] private float pulseAmount = 0.3f;

    private void Update()
    {
        if (cauldronLight != null)
            cauldronLight.intensity = minimumIntensity + Mathf.PingPong(Time.time * 0.35f, pulseAmount);
    }
}
