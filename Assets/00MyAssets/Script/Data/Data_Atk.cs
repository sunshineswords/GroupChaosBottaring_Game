using UnityEngine;
[CreateAssetMenu(menuName ="DataCre/Atk")]
public class Data_Atk : ScriptableObject
{
    public string Name;
    public string Info;
    public int EndTime;
    public float CT;
    public bool Extra;

    public ShotC_Base[] Shots;
    public AnimC[] Anims;
    [System.Serializable]
    public class ShotC_Base
    {
        public GameObject Obj;
        public ShotC_Fire[] Fires;
        public ShotC_Hits Hits;
    }
    [System.Serializable]
    public class ShotC_Fire
    {
        public Vector3Int Times;
        public int Count;
        public Vector2 Speed;
        public ShotC_Trans Trans;
    }
    [System.Serializable]
    public class ShotC_Trans
    {
        public Vector3 PosChange;
        public Vector3 PosRand;
        public Vector3 PosWay;
        public Vector3 PosTime;
        public Vector3 RotChange;
        public Vector3 RotRand;
        public Vector3 RotWay;
        public Vector3 RotTime;
    }
    [System.Serializable]
    public class ShotC_Hits
    {
        public int Damage;
        public int HitCT;
    }
    [System.Serializable]
    public class AnimC
    {
        public Vector2Int Times;
        public int ID;
    }
}
