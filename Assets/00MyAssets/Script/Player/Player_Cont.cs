using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Cont : MonoBehaviourPun
{
    [SerializeField] int AIID = 0;
    [SerializeField] PlayerInput PI;
    public Vector2 Move;
    public Vector2 Look;
    public bool Jump_Enter;
    public bool Dash_Enter;
    public bool NAtk_Enter;
    public bool NAtk_Stay;
    public bool S1Atk_Enter;
    public bool S1Atk_Stay;
    public bool EAtk_Enter;
    public bool EAtk_Stay;

    void Update()
    {
        if (!photonView.IsMine) return;
        ResetInput();
        switch (AIID)
        {
            case 0:PICont();break;
            case 1: AILV0(); break;

        }
        PICont();
    }
    void ResetInput()
    {
        Move = Vector2.zero;
        Look = Vector2.zero;
        Jump_Enter = false;
        Dash_Enter = false;
        NAtk_Enter = false;
        NAtk_Stay= false;
        S1Atk_Enter= false;
        S1Atk_Stay= false;
        EAtk_Enter= false;
        EAtk_Stay= false;
    }
    void PICont()
    {
        Move = PI.actions["Move"].ReadValue<Vector2>();
        Look = PI.actions["Look"].ReadValue<Vector2>();
        Jump_Enter = PI.actions["Jump"].triggered;
        Dash_Enter = PI.actions["Dash"].triggered;
        NAtk_Enter = PI.actions["N_Atk"].triggered;
        NAtk_Stay = PI.actions["N_Atk"].IsPressed();
        S1Atk_Enter = PI.actions["S1_Atk"].triggered;
        S1Atk_Stay = PI.actions["S1_Atk"].IsPressed();
        EAtk_Enter = PI.actions["E_Atk"].triggered;
        EAtk_Stay = PI.actions["E_Atk"].IsPressed();
    }
    void AILV0()
    {
        Move = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
        NAtk_Enter = Random.value <= 0.7f;
        S1Atk_Enter = Random.value <= 0.1f;
        EAtk_Enter = Random.value <= 0.05f;
    }
}
