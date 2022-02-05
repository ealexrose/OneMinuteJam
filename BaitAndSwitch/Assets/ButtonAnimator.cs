using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public RectTransform line;
    public float lineScaleTarget = 0;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustLineScaleTarget();
    }

    private void AdjustLineScaleTarget()
    {
        Vector3 lineScale = line.localScale;

        if ( Mathf.Abs(lineScaleTarget - line.localScale.y) < 0.05f)
        {
            lineScale = line.localScale;
            lineScale.y = lineScaleTarget;
            line.localScale = lineScale;
        }
        else 
        {
            lineScale.y += Mathf.Sign(lineScaleTarget - line.localScale.y) * Time.deltaTime * speed;
            line.localScale = lineScale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        lineScaleTarget = 1f;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        lineScaleTarget = 0f;
    }

}
