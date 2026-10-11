using UnityEngine;

public class MoverDemo : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.right * 2f * Time.deltaTime);
    }
}

