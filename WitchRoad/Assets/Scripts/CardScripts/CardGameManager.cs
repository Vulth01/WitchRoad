using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Quaternion> enemyCardsRotations = new List<Quaternion>();
    private int playerCardCounter = 0;
    private int enemyCardCounter = 0;

    private int cardCap;

    public delegate void CanProgress(bool decider);
    public static event CanProgress canProgress;

    private void Start()
    {
        cardCap = playerPlacements.Length;
    }

    private void PlaceCard(Card card)
    {
        card.played = true;
        
        StartCoroutine(card.SmoothLerpPlace(
            card.transform.position, 
            playerPlacements[playerCardCounter].position, 
            1f, 1));
        
        card.transform.SetParent(playedCards);
        playerCards.Add(card);
        playerCardCounter++;
    }

    private void PlaceEnemyCard(Card card)
    {
        StartCoroutine(card.SmoothLerpPlace(card.transform.position, enemyPlacements[enemyCardCounter].position, 1f, -1));
        enemyCards.Add(card);
        enemyCardsRotations.Add(card.transform.rotation);
        enemyCardCounter++;
        
        if (playerCardCounter >= cardCap) GameEnd();
    }

    private void GameEnd()
    {
        hand.SetActive(false);
        StartCoroutine(GameEndCoroutine());
    }

    private IEnumerator GameEndCoroutine()
    {
        float time = 1f;
        int counter = -1;
        foreach (Card enemyCard in enemyCards)
        {
            float elapsedTime = 0;
            counter++;
            if (enemyCard.cardSO == playerCards[counter].cardSO)
            {
                enemyCard.score = 0;
                playerCards[counter].score = 0;
            }

            enemyCard.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = enemyCard.score + "";
            playerCards[counter].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                playerCards[counter].score + "";
                
            enemyCard.transform.GetChild(1).gameObject.SetActive(true);
            playerCards[counter].transform.GetChild(1).gameObject.SetActive(true);
            
            enemyCard.transform.GetChild(2).gameObject.SetActive(false);
            
            while (elapsedTime < time)
            {
                Quaternion startPos = enemyCardsRotations.ElementAt(counter);
                Quaternion endPos = Quaternion.Euler(new Vector3(180, 0, 0));
                
                enemyCard.transform.localRotation = Quaternion.Lerp(startPos, endPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        float playerScore = CalculateScore(1);
        float enemyScore = CalculateScore(-1);
        
        
        
        yield return new WaitForSeconds(5f);

        TextMeshProUGUI winOrLose = winOrLoseCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        winOrLose.text = playerScore > enemyScore ? winOrLose.text = "Victory" : winOrLose.text = "Defeat";
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
        PlayerCards.enemyTurnEvent += PlaceEnemyCard;
    }

    private void OnDisable()
    {
        Card.cardClicked -= PlaceCard;
        PlayerCards.enemyTurnEvent -= PlaceEnemyCard;
    }
}
