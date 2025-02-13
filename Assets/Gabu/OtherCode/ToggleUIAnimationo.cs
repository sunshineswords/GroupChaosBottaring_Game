using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIAnimationo : MonoBehaviour
{
    public ColorebleUI myAnimation;
    public Toggle toggle;
    public bool isReverse = true;
    public void UpdateAnimation()
    {
        if (toggle.isOn)
        {
            myAnimation.Play();
        }
        else
        {
            if (isReverse)
            {
                myAnimation.Reverse();
            }
            else
            {
                myAnimation.Pause();
            }
        }
    } 
}
