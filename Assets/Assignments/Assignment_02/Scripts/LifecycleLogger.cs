using UnityEngine;

public class LifecycleLogger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {Debug.Log(gameObject.name + "AWAKE"); }
    void OnEnable() {Debug.Log(gameObject.name + "OnEnable");}
    void Start() {Debug.Log(gameObject.name + "Start");}
    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount < 3) Debug.Log("Update, frame" + Time.frameCount);
    }
    void FixedUpdate()
    {
        if (Time.frameCount < 3) Debug.Log("FixedUpdate, frame" + Time.frameCount);
    }
    void OnDisable() {Debug.Log(gameObject.name + "Disable");}
    void OnDestroy() {Debug.Log(gameObject.name + "OnDestroy");}
}
