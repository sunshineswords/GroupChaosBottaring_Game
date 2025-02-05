using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Atk :MonoBehaviourPun
{
    [SerializeField] Player_State PSta;
    [SerializeField] PlayerInput PI;
    void Update()
    {
        if (!photonView.IsMine) return;
        if (PI.actions["Normal_Atk"].triggered)PSta.AtkStart(0);
        if (PI.actions["Skill_Atk"].triggered) PSta.AtkStart(1);
        if (PI.actions["Extra_Atk"].triggered) PSta.AtkStart(2);

    }
}
