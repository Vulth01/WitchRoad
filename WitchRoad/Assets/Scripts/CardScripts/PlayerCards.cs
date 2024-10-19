using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] private CardSO[] cardSOs;
    [SerializeField] private GameObject cardTemplate;

    private void Awake()
    {
        DrawCards();
    }

    private void DrawCards()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject newCard = Instantiate(cardTemplate, transform.GetChild(i));
            int decider = Random.Range(0, cardSOs.Length);
            newCard.GetComponent<Card>().cardSO = cardSOs[decider];
            newCard.GetComponent<Image>().sprite = cardSOs[decider].cardSprite;
            newCard.transform.localPosition = Vector3.zero;
            newCard.transform.localRotation = Quaternion.identity;
        }
    }
    
    
}
