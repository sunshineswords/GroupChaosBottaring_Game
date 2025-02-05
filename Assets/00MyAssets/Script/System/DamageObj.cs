using TMPro;
using UnityEngine;
using static DataBase;
public class DamageObj : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DamText;
    [SerializeField] float HoriSpeed;
    [SerializeField] float VertSpeed;
    [SerializeField] float RemTime;
    [SerializeField] float FallTimePer;
    Vector3 MoveVect;
    int times = 0;
    static public void DamageSet(Vector3 Pos,int Val,Color Col)
    {
        var DamObj = Instantiate(DB.DamageObjs, Pos, Quaternion.identity);
        DamObj.DamText.text = Val.ToString();
        DamObj.DamText.color = Col;
        DamObj.MoveVect = new Vector3(Random.value - 0.5f,0f, Random.value - 0.5f);
    }
    void FixedUpdate()
    {
        times++;
        var Pos = transform.position;
        Pos += MoveVect.normalized * HoriSpeed * 0.01f;
        if (times <= RemTime * 60*(FallTimePer*0.01f)) Pos.y += VertSpeed * 0.01f;
        else Pos.y -= VertSpeed * 0.01f;
        transform.position = Pos;
        if (times >= RemTime * 60) Destroy(gameObject);
    }
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
