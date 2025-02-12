using UnityEngine;

public class State_Hit : MonoBehaviour
{
    public State_Base Sta;
    public float DamAdds;
    public Vector3 PosGet()
    {
        return transform.position;
    }
}
