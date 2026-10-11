using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Main rule controller: UI + C# conditionals + random card hand + PlayerPrefs.
public class WitchGameManager : MonoBehaviour
{
    [Header("References: assign all in Inspector")]
    [SerializeField] private WitchCandyPool candyPool;
    [SerializeField] private WitchCandyCard[] availableCards;
    [SerializeField] private TMP_Text hanselProgressText;
    [SerializeField] private TMP_Text gretelProgressText;
    [SerializeField] private TMP_Text suspicionText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private TMP_Text cardText1;
    [SerializeField] private TMP_Text cardText2;
    [SerializeField] private TMP_Text cardText3;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

    [Header("Visual feedback")]
    [SerializeField] private Image hanselFullnessFill;
    [SerializeField] private Image gretelFullnessFill;
    [SerializeField] private Image suspicionFill;
    [SerializeField] private Image[] cardBackgrounds;

    [Header("Game rules")]
    [SerializeField] private int targetFullness = 10;
    [SerializeField] private int maximumSuspicion = 6;
    [SerializeField] private float gameSeconds = 100f;

    private readonly WitchCandyCard[] hand = new WitchCandyCard[3];
    private int selectedSlot;
    private int hanselFullness;
    private int gretelFullness;
    private int suspicion;
    private int sweetsThrown;
    private float timeLeft;
    private bool gameEnded;

    public bool IsFinished { get { return gameEnded; } }
    public bool CanPlay { get { return !gameEnded && Time.timeScale > 0f; } }

    private void Start()
    {
        Time.timeScale = 1f;
        timeLeft = gameSeconds;
        if (resultPanel != null) resultPanel.SetActive(false);
        if (availableCards == null || availableCards.Length == 0)
        {
            Debug.LogError("GameManager needs at least one Candy Card asset in Available Cards.");
            enabled = false;
            return;
        }
        for (int i = 0; i < hand.Length; i++) hand[i] = DrawCard();
        selectedSlot = 0;
        RefreshUI();
        ShowMessage("Feed BOTH children. Avoid 6 suspicion!");
    }

    private void Update()
    {
        if (gameEnded) return;
        timeLeft -= Time.deltaTime; // Stops automatically when paused.
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            FinishGame(false, "Time ran out! The children escaped.");
        }
        RefreshTime();
    }

    private WitchCandyCard DrawCard()
    {
        return availableCards[Random.Range(0, availableCards.Length)];
    }

    public void SelectFirstCard() { SelectCard(0); }
    public void SelectSecondCard() { SelectCard(1); }
    public void SelectThirdCard() { SelectCard(2); }

    private void SelectCard(int index)
    {
        if (!CanPlay) return;
        selectedSlot = index;
        RefreshCards();
    }

    public void ThrowSelectedCandy(Vector2 position, Vector2 direction)
    {
        if (!CanPlay || candyPool == null || hand[selectedSlot] == null) return;
        WitchCandyCard chosenCard = hand[selectedSlot];
        candyPool.Spawn(position, direction, chosenCard);
        sweetsThrown++;
        ShowMessage("Threw " + chosenCard.CandyName + "! Aim at Hansel or Gretel.");
        hand[selectedSlot] = DrawCard();
        RefreshCards();
    }

    public void RegisterCandyHit(bool hitHansel, int fullness, int addedSuspicion)
    {
        if (gameEnded) return;
        if (hitHansel)
        {
            hanselFullness = Mathf.Min(targetFullness, hanselFullness + fullness);
            ShowMessage("Hansel enjoyed the candy! +" + fullness + " fullness");
        }
        else
        {
            gretelFullness = Mathf.Min(targetFullness, gretelFullness + fullness);
            ShowMessage("Gretel enjoyed the candy! +" + fullness + " fullness");
        }
        suspicion += addedSuspicion;
        RefreshUI();
        if (suspicion >= maximumSuspicion)
            FinishGame(false, "Too suspicious! Hansel and Gretel escaped.");
        else if (hanselFullness >= targetFullness && gretelFullness >= targetFullness)
            FinishGame(true, "The witch wins! Both children are completely full.");
    }

    private void FinishGame(bool won, string message)
    {
        gameEnded = true;
        Time.timeScale = 0f; // Freeze projectiles and the room while the result is shown.
        if (won)
        {
            int wins = PlayerPrefs.GetInt("WitchWins", 0) + 1;
            PlayerPrefs.SetInt("WitchWins", wins);
            float elapsed = gameSeconds - timeLeft;
            if (!PlayerPrefs.HasKey("WitchBestTime") || elapsed < PlayerPrefs.GetFloat("WitchBestTime"))
                PlayerPrefs.SetFloat("WitchBestTime", elapsed);
            PlayerPrefs.Save();
        }
        if (resultPanel != null) resultPanel.SetActive(true);
        if (resultText != null) resultText.text = message + "\nCandies thrown: " + sweetsThrown;
        ShowMessage(message);
    }

    private void RefreshUI()
    {
        if (hanselProgressText != null) hanselProgressText.text = "HANSEL  " + hanselFullness + "/" + targetFullness;
        if (gretelProgressText != null) gretelProgressText.text = "GRETEL  " + gretelFullness + "/" + targetFullness;
        if (suspicionText != null) suspicionText.text = "SUSPICION  " + suspicion + "/" + maximumSuspicion;
        if (hanselFullnessFill != null) hanselFullnessFill.fillAmount = (float)hanselFullness / targetFullness;
        if (gretelFullnessFill != null) gretelFullnessFill.fillAmount = (float)gretelFullness / targetFullness;
        if (suspicionFill != null) suspicionFill.fillAmount = (float)suspicion / maximumSuspicion;
        RefreshTime();
        RefreshCards();
    }

    private void RefreshTime()
    {
        if (timeText != null) timeText.text = "TIME  " + Mathf.CeilToInt(timeLeft) + "s";
    }

    private void RefreshCards()
    {
        UpdateCardLabel(cardText1, 0);
        UpdateCardLabel(cardText2, 1);
        UpdateCardLabel(cardText3, 2);
        if (cardBackgrounds == null) return;
        for (int i = 0; i < cardBackgrounds.Length; i++)
        {
            if (cardBackgrounds[i] != null)
                cardBackgrounds[i].color = i == selectedSlot
                    ? new Color(0.26f, 0.48f, 0.40f)
                    : new Color(0.18f, 0.14f, 0.22f);
        }
    }

    private void UpdateCardLabel(TMP_Text label, int index)
    {
        if (label == null || hand[index] == null) return;
        WitchCandyCard card = hand[index];
        label.text = (index + 1) + "  " + card.CandyName.ToUpper() +
            (selectedSlot == index ? "  [SELECTED]" : "") +
            "\n+" + card.FullnessPoints + " Fullness" +
            (card.SuspicionPoints > 0 ? "   +" + card.SuspicionPoints + " Suspicion" : "");
    }

    private void ShowMessage(string message)
    {
        if (hintText != null) hintText.text = message;
    }
}
