using UnityEngine;
using Zenject;

public class CursorManager : IInitializable
{
    public void Initialize()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}