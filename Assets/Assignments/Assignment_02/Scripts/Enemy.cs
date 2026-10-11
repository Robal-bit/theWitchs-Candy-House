using UnityEngine;

public class Enemy : Character
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TakeDamage(60);
        TakeDamage(60);

        TestAccess();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (health <= 0) Debug.Log(gameObject.name + "defeated.");
    }

    public Character otherCharacter;
    
        void TestAccess(){
            if (otherCharacter != null)
        {
            otherCharacter.Heal(60);
        }
        }
}
