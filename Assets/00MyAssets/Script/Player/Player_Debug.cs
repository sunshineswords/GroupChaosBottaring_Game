using UnityEngine;

public class Player_Debug : MonoBehaviour
{
    [SerializeField] State_Base Sta;
    public void HPFull()
    {
        Sta.HP = Sta.MHP;
    }
    public void MPFull()
    {
        Sta.MP = Sta.MMP;
    }
    public void SPFull()
    {
        Sta.SP = 1000000;
    }
    public void CTReset()
    {
        Sta.AtkCTs.Clear();
    }
    public void Death()
    {
        Sta.HP = -Sta.MMP;
    }
}
