using UnityEngine;
using static Manifesto;
[CreateAssetMenu(menuName ="DataCre/Buf")]
public class Data_Buf : ScriptableObject
{
    public Enum_Bufs Buf;
    [TextArea]public string Info;
    public Color Col;
    public Texture Icon;
    public GameObject EffectObj;
}
