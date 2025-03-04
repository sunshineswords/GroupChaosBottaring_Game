using UnityEngine;
using static BattleManager;
public class State_Hit : MonoBehaviour
{
    public State_Base Sta;
    public float DamAdds;

    private void Start()
    {
        BTManager.HitList.Add(this);
    }
    public Vector3 PosGet()
    {
        return transform.position;
    }
}
