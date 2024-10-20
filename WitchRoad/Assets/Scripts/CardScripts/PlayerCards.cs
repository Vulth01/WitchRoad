using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CardState
{
    Playable,
    Busy
}
public class PlayerCards : MonoBehaviour
{
    [SerializeField] private CardSO[] cardSOs;
    [SerializeField] private GameObject cardTemplate;
    [SerializeField] private GameObject enemyCardTemplate;
    private List<Card> currentHand = new List<Card>();
    private List<Vector3> currentHandBasePos = new List<Vector3>();
    private Vector3 middleCardPos;

    public delegate void EnemyTurnEvent(Card card);
    public static event EnemyTurnEvent enemyTurnEvent;
    
    public CardState state = CardState.Playable;
    
    private void Start()
    {
        DrawCards();
    }

    private void DrawCards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject newCard = Instantiate(cardTemplate, transform.GetChild(i));
            currentHand.Add(GetRandomCard(newCard));
            newCard.GetComponent<Card>().id = 1;
            if (i is 2) middleCardPos = newCard.transform.position;
            
            currentHandBasePos.Add(newCard.transform.parent.position);
        }
    }

    private Card GetRandomCard(GameObject newCard)
    {
        int decider = Random.Range(0, cardSOs.Length);
        newCard.GetComponent<Card>().cardSO = cardSOs[decider];
        newCard.GetComponent<Image>().sprite = cardSOs[decider].cardSprite;
        newCard.transform.localPosition = Vector3.zero;
        newCard.transform.localRotation = Quaternion.identity;

        return newCard.GetComponent<Card>();
    }

    private Card EnemyCard()
    {
        GameObject newCard = Instantiate(enemyCardTemplate, transform.parent.GetChild(2));
        return GetRandomCard(newCard);
    }

    private void DiscardCards()
    {
        StartCoroutine(EnemyTurn(EnemyCard()));
    }

    private IEnumerator EnemyTurn(Card card)
    {
        enemyTurnEvent?.Invoke(card);
        yield return new WaitForSeconds(1f);
        StartCoroutine(DiscardLerp(middleCardPos, 1));
    }
    
    public IEnumerator DiscardLerp (Vector3 endPos, float time)
    {
        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            int counter = 0;
            foreach (Card listCard in currentHand)
            {
                if(listCard.played) continue;
                Vector3 startPos = currentHandBasePos.ElementAt(counter);
                listCard.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
                counter++;
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        foreach (Card listCard in currentHand)
        {
            if(!listCard.played)
                Destroy(listCard.gameObject);
        }
        currentHandBasePos.Clear();
        currentHand.Clear();

        DrawCards();
        state = CardState.Playable;
    }
    
    private void SetState(Card card) => state = CardState.Busy;

    private void OnEnable()
    {
        Card.cardClicked += SetState;
        Card.discardCards += DiscardCards;
    }

    private void OnDisable()
    {
        Card.cardClicked -= SetState;
       Card.discardCards -= DiscardCards;
    }
}
