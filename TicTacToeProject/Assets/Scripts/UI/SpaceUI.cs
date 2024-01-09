using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SpaceUI : MonoBehaviour, IPointerClickHandler
{
    public (byte, byte) coordinates;
    [SerializeField] private Image signImage;
    public Image backgroundImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (signImage.sprite != null || GameManager.Instance.CurrentPlayer.GetType() != typeof(PhysicalPlayer))
        {
            return;
        }

        GameEvents.SpaceSelect(coordinates, GameManager.Instance.CurrentPlayer.signType);
        GameEvents.AfterSpaceSelect(coordinates);
    }

    public void Select(SignType signType)
    {
        SetImageSign(signType);
    }

    public void Highlight()
    {        
        DOTween.Kill("highlightHint");

        var defaultColor = backgroundImage.color;
        var tween = DOTween.To(() => backgroundImage.color, x => backgroundImage.color = x, Color.yellow, 1f)
        .SetId("highlightHint")
        .SetLoops(3, LoopType.Yoyo)
        .OnKill( ()=> { backgroundImage.color = defaultColor; })
        .OnComplete(() => { backgroundImage.color = defaultColor; });
    }


    public void SetImageSign(SignType sign)
    {
        if (sign == SignType.X)
        {
            signImage.sprite = GameManager.Instance.xSprite;
        }
        else if (sign == SignType.O)
        {
            signImage.sprite = GameManager.Instance.oSprite;
        }
        else
        {
            signImage.sprite = null;
        }

        if (sign != SignType.None)
        {
            signImage.transform.DOPunchScale(new Vector3(.5f, .5f, .5f), 1.2f, 5, 1);
        }
    }
}