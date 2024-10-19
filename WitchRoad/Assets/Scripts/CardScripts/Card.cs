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
    public CardSO cardSO;
    public float score = 0;
    public bool played = false;

    public delegate void CardClickEvent(Card card, int player);
    public static event CardClickEvent cardClicked;
    
    public delegate void DiscardCards();
    public static event DiscardCards discardCards;

    private void Awake()
    {
        hoverPlacement = transform.GetChild(0).position;
        basePos = transform.position;
    }

    private void Start()
    {
        score = cardSO.score;
    }

    public void OnHoverEnter()
    {
        if (played) return;
        siblingIndex = transform.parent.GetSiblingIndex();
        transform.parent.SetAsLastSibling();
        StartCoroutine(SmoothLerp(transform.position, hoverPlacement, 0.3f));
    }

    public void OnHoverExit()
    {
        if (played) return;
        transform.parent.SetSiblingIndex(siblingIndex);
        StartCoroutine(SmoothLerp(transform.position, basePos, 0.3f));
    }

    public void ClickCard()
    {
        if (played) return;
        cardClicked?.Invoke(this, 1);
    }
    
    public IEnumerator SmoothLerp (Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0;
        
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    
    public IEnumerator SmoothLerpPlace (Vector3 startPos, Vector3 endPos ,float time)
    {
        float elapsedTime = 0;
        Vector3 currentScale = this.transform.localScale;
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);

        Quaternion currentEuler = transform.localRotation;
        
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            transform.localScale = Vector3.Lerp(currentScale, targetScale, (elapsedTime / time));
            transform.localRotation = Quaternion.Lerp(currentEuler, Quaternion.identity, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        discardCards?.Invoke();
    }
}
