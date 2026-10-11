using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void TakeDamage(int amount)
    {
        health -= amount; 
        Debug.Log(gameObject.name + "took" + amount + "damage.");
    }

    public void Heal(int amount)
    {
        health +=amount;
        Debug.Log(gameObject + " healed" + amount + " HP.");
    }
}
