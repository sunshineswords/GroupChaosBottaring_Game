using UnityEngine;

public class CursorSystem_Gabu : MonoBehaviour
{
    public bool setCursorVisible = false;
    private void Start()
    {
        Cursor.visible = setCursorVisible;
    }
    private void OnEnable()
    {
        Cursor.visible = setCursorVisible;
    }
    private void OnDisable()
    {
        Cursor.visible = !setCursorVisible;
    }   
}
