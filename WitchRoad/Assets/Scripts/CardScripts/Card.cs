using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private int siblingIndex;
    private Vector3 hoverPlacement;
    private Vector3 basePos;
    private Vector3 velocity = Vector3.zero;
    public CardSO cardSO;

    public delegate void CardClickEvent(CardSO card);
    public static event CardClickEvent cardClicked;

    private void Awake()
    {
        hoverPlacement = transform.GetChild(0).position;
        basePos = transform.position;
    }

    public void OnHoverEnter()
    {
        siblingIndex = transform.parent.GetSiblingIndex();
        transform.parent.SetAsLastSibling();
        StartCoroutine(SmoothLerp(transform.position, hoverPlacement));
    }

    public void OnHoverExit()
    {
        transform.parent.SetSiblingIndex(siblingIndex);
        StartCoroutine(SmoothLerp(transform.position, basePos));
    }

    public void ClickCard()
    {
        cardClicked?.Invoke(cardSO);
    }
    
    private IEnumerator SmoothLerp (Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0;
        float time = 0.3f;
        
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
