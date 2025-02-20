using System;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.UI.Button;
using UnityEngine.Serialization;

public class ChangeButtonSettings : MonoBehaviour
{
    public ImageAnimation_Gabu[] targets;
    public NEWSettings[] settings;

    // Event delegates triggered on click.
    [FormerlySerializedAs("onSet")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    public void SetSettings(int index)
    {
        if (index < 0 || index >= settings.Length)
        {
            return;
        }
        m_OnClick.Invoke();
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