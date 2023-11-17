using System;
using UnityEngine;

public class CameraRotationSystem : MonoBehaviour, ISystem
{
    [SerializeField] private Camera frontCam;
    [SerializeField] private Camera rightCam;
    [SerializeField] private Camera backCam;
    [SerializeField] private Camera leftCam;
    

    private Camera _camera;

   
    
    private void Awake()
    {
        _camera = frontCam;
        BlackBoard.CameraPosition = CameraPosition.Front;
    }

    public void OnFrameUpdate()
    {
        
    }

    public void OnSystemUpdate()
    {
        if (BlackBoard.cameraRotationKeyPressed)
        {
            if (_camera == frontCam)
            {
                ChangeCamTo(rightCam);
                BlackBoard.CameraPosition = CameraPosition.Right;
            }
            else if (_camera == rightCam)
            {
                ChangeCamTo(backCam);
                BlackBoard.CameraPosition = CameraPosition.Back;
            }
            else if (_camera == backCam)
            {
                ChangeCamTo(leftCam);
                BlackBoard.CameraPosition = CameraPosition.Left;
            }
            else if (_camera == leftCam)
            {
                ChangeCamTo(frontCam);
                BlackBoard.CameraPosition = CameraPosition.Front;
            }
        }
    }

    private void ChangeCamTo(Camera targetCamera)
    {
        _camera.gameObject.SetActive(false);
        _camera = targetCamera;
        _camera.gameObject.SetActive(true);
    }
}

public enum CameraPosition
{
    Front,
    Right,
    Back,
    Left
}