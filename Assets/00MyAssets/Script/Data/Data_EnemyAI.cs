using UnityEngine;
using static Manifesto;
[CreateAssetMenu(menuName ="DataCre/EnemyAI")]
public class Data_EnemyAI : ScriptableObject
{
    public Class_Enemy_AtkAI[] AtkAIs;
    public int TimerLim;
    public bool NoTargetResetTime;
}
