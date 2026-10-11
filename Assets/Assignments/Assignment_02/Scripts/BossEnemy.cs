using UnityEngine;

public class BossEnemy : Enemy
{
    private int revivesRemaining = 1; 
    private bool isDefeated = false; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TakeDamage(60);
        TakeDamage(60);
        TakeDamage(100);
    }

    public override void TakeDamage(int amount)
    {
        if (isDefeated) 
            return; 

        base.TakeDamage(amount);

        if (health > 0) 
            return; 
        if (revivesRemaining > 0 )
        {
        revivesRemaining--; 
        health = 100; 
        Debug.Log(gameObject.name + " resurrected.");
        }
        else 
        {
            isDefeated = true; 
        }
    }
}

