using UnityEngine;

public class State_Anims : MonoBehaviour
{
    public State_Base Sta;
    [SerializeField] Animator Anim;

    void Update()
    {
        Anim.SetInteger("MoveID", Sta.Anim_MoveID);
        Anim.SetInteger("AtkID", Sta.Anim_AtkID);
        Anim.SetInteger("OtherID", Sta.Anim_OtherID);
    }
}
