using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{

    public static EffectManager Instance;

    [Header("Tweaking")]
    public SpecialEffect[] effects;

    [Header("Debug")]
    public int debugIndex;
    public bool triggerDebug;

    [Header("References")]
    public CameraController cameraController;

    private void Awake()
    {
        if(EffectManager.Instance == null)
        {
            EffectManager.Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerDebug || Input.GetKeyDown(KeyCode.G))
        {
            triggerDebug = false;
            TriggerEffect(debugIndex);
        }
    }

    public void TriggerEffect(int index)
    {
        if(cameraController.PerformEffect || index < 0 || index > effects.Length)
        {
            return;
        }
        StartCoroutine(Evaluate(effects[index]));        
    }

    IEnumerator Evaluate(SpecialEffect effect)
    {
        float timer = 0f;
        cameraController.PerformEffect = true;
        Quaternion cameraRotation = cameraController.playerCamera.transform.localRotation;
        float cameraFOV = cameraController.playerCamera.fieldOfView;
        bool next = false;
        while(cameraController.PerformEffect && !next)
        {
            Vector3 newRotation = cameraRotation.eulerAngles;
            Vector3 newPosition = Vector3.zero;
            if(effect.timeScale.length > 0 && timer <= effect.timeScale[effect.timeScale.length - 1].time)
            {
                Time.timeScale = effect.timeScale.Evaluate(timer);
            }
            if(effect.camPositionX.length > 0 && timer <= effect.camPositionX[effect.camPositionX.length - 1].time)
            {
                newPosition += new Vector3(effect.camPositionX.Evaluate(timer), 0, 0);
            }
            if(effect.camPositionY.length > 0 && timer <= effect.camPositionY[effect.camPositionY.length - 1].time)
            {
                newPosition += new Vector3(0, effect.camPositionY.Evaluate(timer), 0);
            }
            if(effect.camPositionZ.length > 0 && timer <= effect.camPositionZ[effect.camPositionZ.length - 1].time)
            {
                newPosition += new Vector3(0, 0, effect.camPositionZ.Evaluate(timer));
            }
            if(effect.camRotationX.length > 0 && timer <= effect.camRotationX[effect.camRotationX.length - 1].time)
            {
                newRotation += new Vector3(effect.camRotationX.Evaluate(timer), 0, 0);
            }
            if(effect.camRotationY.length > 0 && timer <= effect.camRotationY[effect.camRotationY.length - 1].time)
            {
                newRotation += new Vector3(0, effect.camRotationY.Evaluate(timer), 0);
            }
            if(effect.camRotationZ.length > 0 && timer <= effect.camRotationZ[effect.camRotationZ.length - 1].time)
            {
                newRotation += new Vector3(0, 0, effect.camRotationZ.Evaluate(timer));
            }
            if(effect.camZoom.length > 0 && timer <= effect.camZoom[effect.camZoom.length - 1].time)
            {
                cameraController.playerCamera.fieldOfView = cameraFOV + effect.camZoom.Evaluate(timer);
            }
            if(timer >= effect.effectDuration)
            {
                if(effect.nextEffect != null)
                {
                    next = true;
                }
                else
                {
                    cameraController.PerformEffect = false;
                }
            }
            cameraController.playerCamera.transform.localRotation = Quaternion.Euler(newRotation);
            cameraController.playerCamera.transform.localPosition = newPosition;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        cameraController.playerCamera.transform.localPosition = Vector3.zero;
        cameraController.playerCamera.transform.localRotation = cameraRotation;
        cameraController.playerCamera.fieldOfView = cameraFOV;
        Time.timeScale = 1f;
        if(next)
        {
            StartCoroutine(Evaluate(effect.nextEffect));
        }
    }
}
