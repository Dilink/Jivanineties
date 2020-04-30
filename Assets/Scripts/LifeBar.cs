using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    public Camera m_Camera;

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        //transform.LookAt(transform.position + 
        //    GameManager.Instance.player.effectManager.cameraController.playerCamera.transform.rotation * Vector3.forward,
        //   GameManager.Instance.player.effectManager.cameraController.playerCamera.transform.rotation * Vector3.up);

        transform.LookAt(GameManager.Instance.player.effectManager.cameraController.playerCamera.transform);

        transform.rotation = transform.rotation * Quaternion.Euler(45, 0, 0);
    }
}

