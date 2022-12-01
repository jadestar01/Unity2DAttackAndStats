using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Mouse : MonoBehaviour
{
    public enum CursorType
    {
        Combat,
        UI,
        Upgrade
    }
    public CursorType cursorType = CursorType.Combat;
    public CursorType cursor = CursorType.Combat;

    [SerializeField] Texture2D Combat;
    [SerializeField] Texture2D UI;
    [SerializeField] Texture2D Upgrade;

    private void Start()
    {
        Cursor.SetCursor(Combat, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        if (cursorType != cursor)
        {
            cursor = cursorType;

            if (cursor == CursorType.Combat)
                Cursor.SetCursor(Combat, Vector2.zero, CursorMode.ForceSoftware);
            else if (cursor == CursorType.UI)
                Cursor.SetCursor(UI, Vector2.zero, CursorMode.ForceSoftware);
            else if (cursor == CursorType.Upgrade)
                Cursor.SetCursor(Upgrade, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
