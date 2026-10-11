using System.Collections.Generic;
using UnityEngine;

// WEEK 3: Create objects once; reuse inactive ones; grow only if necessary.
public class WitchCandyPool : MonoBehaviour
{
    [SerializeField] private WitchCandyProjectile candyPrefab;
    [SerializeField] private int initialPoolSize = 12;
    private readonly List<WitchCandyProjectile> candies = new List<WitchCandyProjectile>();

    private void Awake()
    {
        if (candyPrefab == null)
        {
            Debug.LogError("Assign Candy Prefab on CandyPool in the Inspector.");
            return;
        }
        for (int i = 0; i < initialPoolSize; i++) CreateOne();
    }

    private WitchCandyProjectile CreateOne()
    {
        WitchCandyProjectile item = Instantiate(candyPrefab, transform);
        item.gameObject.SetActive(false);
        candies.Add(item);
        return item;
    }

    public void Spawn(Vector2 position, Vector2 direction, WitchCandyCard card)
    {
        foreach (WitchCandyProjectile item in candies)
        {
            if (!item.gameObject.activeSelf)
            {
                item.Launch(position, direction, card);
                return;
            }
        }
        // If every candy is currently active, add one to the pool.
        CreateOne().Launch(position, direction, card);
    }
}
