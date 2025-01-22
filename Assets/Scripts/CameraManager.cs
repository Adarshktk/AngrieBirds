using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera idleCam;
    [SerializeField] private CinemachineVirtualCamera followCam;

    private void Awake()
    {

        SwitchToIdleCam();


    }

    public void SwitchToIdleCam()
    {
        idleCam.enabled = true;
        followCam.enabled = false;
    }
    public void SwitchToFollowCam(Transform followTransform)
    {
        followCam.Follow = followTransform;
        followCam.enabled = true;
        idleCam.enabled = false;
    }
}
