using UnityEngine;

public class CursorSystem_Gabu : MonoBehaviour
{
    public bool setCursorVisible = false;
    private void Start()
    {
        Cursor.visible = setCursorVisible;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}
