using UnityEngine;

// WEEK 7: One asset defines a candy card; every hand slot can reference it.
[CreateAssetMenu(fileName = "NewCandyCard", menuName = "Witch Game/Candy Card")]
public class WitchCandyCard : ScriptableObject
{
    [SerializeField] private string candyName = "Gummies";
    [SerializeField, Min(1)] private int fullnessPoints = 1;
    [SerializeField, Min(0)] private int suspicionPoints = 0;
    [SerializeField] private Color projectileColor = new Color(1f, 0.4f, 0.6f);

    // Read-only properties: other scripts may read, but cannot change the asset.
    public string CandyName { get { return candyName; } }
    public int FullnessPoints { get { return fullnessPoints; } }
    public int SuspicionPoints { get { return suspicionPoints; } }
    public Color ProjectileColor { get { return projectileColor; } }
}
