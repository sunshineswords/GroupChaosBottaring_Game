using UnityEngine;
[CreateAssetMenu(menuName ="DataCre/Stage")]
public class Data_Stage : ScriptableObject
{
    public string Name;
    [TextArea]public string Info;
    public Texture Icon;
    public int SceneID;
}
