using UnityEngine;
using Zenject;

[RequireComponent(typeof(CameraController2D))]
public class CameraController2D : MonoBehaviour, IInitializable
{
    public Camera Camera
    {
        get;
        private set;
    }

    public void Initialize()
    {
        Camera = GetComponent<Camera>();
    }
}