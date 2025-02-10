using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DataBase;
public class State_Anims : MonoBehaviour
{
    public State_Base Sta;
    [SerializeField] Animator Anim;
    Dictionary<int, Transform> SetTrans = new Dictionary<int, Transform>();
    Dictionary<int, int> WeponSets = new Dictionary<int, int>();
    Dictionary<int, GameObject> WeponSObjs = new Dictionary<int, GameObject>();

    void Update()
    {
        Anim.SetInteger("MoveID", Sta.Anim_MoveID);
        Anim.SetInteger("AtkID", Sta.Anim_AtkID);
        Anim.SetInteger("OtherID", Sta.Anim_OtherID);
        SetTrans.TryAdd(0, Anim.GetBoneTransform(HumanBodyBones.RightHand));
        SetTrans.TryAdd(1, Anim.GetBoneTransform(HumanBodyBones.LeftHand));
        var WeponKeys = Sta.WeponSets.Keys.ToArray();
        for (int i = 0; i < WeponKeys.Length; i++)
        {
            var WepKey = WeponKeys[i];
            Debug.Log("Wep" + WepKey + ":" + Sta.WeponSets[i]);
            WeponSets.TryAdd(WepKey, -1);
            WeponSObjs.TryAdd(WepKey, null);
            if (WeponSets[WepKey] != Sta.WeponSets[WepKey])
            {
                WeponSets[WepKey] = Sta.WeponSets[WepKey];
                if (WeponSObjs[WepKey] != null) Destroy(WeponSObjs[WepKey]);
                if (WeponSets[WepKey] >= 0)
                {
                    var InsWepon = Instantiate(DB.Wepons[WeponSets[WepKey]]);
                    InsWepon.transform.parent = SetTrans[WepKey];
                    InsWepon.transform.localPosition = Vector3.zero;
                    InsWepon.transform.localPosition += Sta.WeponPoss[WepKey];
                    InsWepon.transform.localRotation = Quaternion.identity;
                    InsWepon.transform.localEulerAngles += Sta.WeponRots[WepKey];
                    WeponSObjs[WepKey] = InsWepon;
                }

            }
        }
    }
}
