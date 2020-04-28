using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Tweaking")]
    [Range(0, 100)]
    public float cameraSpeed;

    [Header("References")]
    public Camera playerCamera;

    private Vector3 cameraDistance;

    private void Awake()
    {
        cameraDistance = playerCamera.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, transform.position + cameraDistance, cameraSpeed * Time.deltaTime);
    }
}
