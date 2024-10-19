using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playMat;
    [SerializeField] private Transform[] playerPlacements;
    [SerializeField] private Transform[] enemyPlacements;
    [SerializeField] private Transform playedCards;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject winOrLoseCanvas;
    private List<Card> playerCards = new List<Card>();
    private List<Card> enemyCards = new List<Card>();
    private int playerCardCounter = 0;
    private int enemyCardCounter = 0;

    private int cardCap;

    public delegate void CanProgress(bool decider);
    public static event CanProgress canProgress;

    private void Start()
    {
        cardCap = playerPlacements.Length;
    }

    private void PlaceCard(Card card, int player)
    {
        card.played = true;

        if (player is 1)
        {
            StartCoroutine(card.SmoothLerpPlace(
                card.transform.position, 
                playerPlacements[playerCardCounter].position, 
                1f));
            
            card.transform.SetParent(playedCards);
            playerCards.Add(card);
            playerCardCounter++;
        }

        if (playerCardCounter >= cardCap) GameEnd();
    }

    private void GameEnd()
    {
        hand.SetActive(false);

        float playerScore = CalculateScore(1);
        float enemyScore = CalculateScore(-1);

        StartCoroutine(GameEndCoroutine(playerScore, enemyScore));
    }

    private IEnumerator GameEndCoroutine(float playerScore, float enemyScore)
    {
        winOrLoseCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            playerScore > enemyScore ? "Victory" : "Defeat";
        winOrLoseCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        canProgress?.Invoke(playerScore > enemyScore);
    }

    private float CalculateScore(int player)
    {
        float score = 0;
        if (player is 1)
        {
            foreach (Card card in playerCards)
            {
                score += card.score;
            }
        }
        else
        {
            foreach (Card card in enemyCards)
            {
                score += card.score;
            }
        }

        return score;
    }


    private void OnEnable()
    {
        Card.cardClicked += PlaceCard;
    }

    private void OnDisable()
    {
        Card.cardClicked -= PlaceCard;
    }
}
