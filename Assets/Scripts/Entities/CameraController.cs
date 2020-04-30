using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool PerformEffect 
    { 
        get 
        { 
            return _performEffect; 
        } 
        
        set 
        { 
            _performEffect = value;
        }
    }
    public Vector3 CameraDistance { get { return _cameraDistance; } set { _cameraDistance = value; } }

    public Vector3 CameraPosition { get { return transform.position + _cameraDistance; } }

    [Header("Tweaking")]
    [Range(0, 100)]
    public float cameraSpeed;

    [Header("References")]
    public Transform cameraman;
    public Camera playerCamera;

    private Vector3 _cameraDistance;

    private bool _performEffect;

    // Start is called before the first frame update
    void Start()
    {
        _cameraDistance = playerCamera.transform.position;
        _performEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        cameraman.position = Vector3.Lerp(playerCamera.transform.position, transform.position + _cameraDistance, cameraSpeed * Time.unscaledDeltaTime);
    }


}
