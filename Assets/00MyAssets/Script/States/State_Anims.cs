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
        if (Anim.avatar != null)
        {
            SetTrans.TryAdd(0, Anim.GetBoneTransform(HumanBodyBones.RightHand));
            SetTrans.TryAdd(1, Anim.GetBoneTransform(HumanBodyBones.LeftHand));

        }
        var WeponKeys = Sta.WeponSets.Keys.ToArray();
        for (int i = 0; i < WeponKeys.Length; i++)
        {
            var WepKey = WeponKeys[i];
            WeponSets.TryAdd(WepKey, -1);
            WeponSObjs.TryAdd(WepKey, null);
            if (WeponSets[WepKey] != Sta.WeponSets[WepKey])
            {
                WeponSets[WepKey] = Sta.WeponSets[WepKey];
                if (WeponSObjs[WepKey] != null) Destroy(WeponSObjs[WepKey]);
                if (WeponSets[WepKey] >= 0)
                {
                    var InsWepon = Instantiate(DB.Wepons[WeponSets[WepKey]]);
                    var Trans = SetTrans.TryGetValue(WepKey,out var oTrans) ? oTrans : null;
                    InsWepon.transform.parent = Trans != null ? Trans : transform;
                    WeponSObjs[WepKey] = InsWepon;
                }
            }
            var SetWep = WeponSObjs[WepKey];
            if (WeponSObjs[WepKey] != null)
            {
                SetWep.transform.localPosition = Vector3.zero;
                SetWep.transform.localPosition += Sta.WeponPoss[WepKey];
                SetWep.transform.localRotation = Quaternion.identity;
                SetWep.transform.localEulerAngles += Sta.WeponRots[WepKey];
            }
        }
    }
}
