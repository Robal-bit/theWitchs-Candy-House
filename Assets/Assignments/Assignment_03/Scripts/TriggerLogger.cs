using UnityEngine;

public class TriggerLogger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("TriggerLogger is running!");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Enemy"))
    {
        Debug.Log("Hit an Enemy!"); 
    }
    else
    {
        Debug.Log($"Triggered by {other.gameObject.name}");
    }
    }


}
