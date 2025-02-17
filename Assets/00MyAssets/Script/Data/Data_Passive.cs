using UnityEngine;
[CreateAssetMenu(menuName ="DataCre/Passive")]
public class Data_Passive : ScriptableObject
{
    public string Name;
    [TextArea]public string Info;
    public Texture Icon;
}
