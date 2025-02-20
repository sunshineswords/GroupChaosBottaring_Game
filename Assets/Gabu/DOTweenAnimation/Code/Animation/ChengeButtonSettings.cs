using System;
using UnityEngine;
using DG.Tweening;

public class ChengeButtonSettings : MonoBehaviour
{
    public ImageAnimation_Gabu[] targets;
    public NEWSettings[] settings;

    public void SetSettings(int index)
    {
        if (index < 0 || index >= settings.Length)
        {
            return;
        }
        for (int i = 0; i < targets.Length; i++)
        {
            if (i < settings[index].settings.Length)
            {
                targets[i].UpdateSettings(settings[index].settings[i]);
            }
        }
    }
}

[Serializable]
public class NEWSettings
{
    public ImageAnimation_Gabu[] settings;
}