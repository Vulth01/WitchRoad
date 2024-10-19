using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playMat;
    [SerializeField] private Transform[] playerPlacements;
    [SerializeField] private Transform[] enemyPlacements;
    [SerializeField] private Transform playedCards;
    private int playerCardCounter = 0;
    private int enemyCardCounter = 0;

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
            
            playerCardCounter++;
        }
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
