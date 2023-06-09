using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class ActivateTeleportationRay : MonoBehaviour
{
    [SerializeField] GameObject leftRay;
    [SerializeField] GameObject rightRay;


    [SerializeField] InputActionProperty leftActivate;
    [SerializeField] InputActionProperty rightActivate;

    [SerializeField] InputActionProperty leftCancel;
    [SerializeField] InputActionProperty rightCancel;

    [SerializeField] InputActionReference primary;
    [SerializeField] bool canTP;
    [SerializeField] XRRayInteractor LeftTP;
    [SerializeField] XRRayInteractor RightTP;

    //public InputActionProperty grab;
    private void Awake()
    {
        canTP = true;
    }

    void Update()
    {
        if(leftRay != null) leftRay.SetActive(leftCancel.action.ReadValue<float>() < 0.1f && leftActivate.action.ReadValue<float>() > 0.1f);

        if(rightRay != null)rightRay.SetActive(rightCancel.action.ReadValue<float>() < 0.1f && rightActivate.action.ReadValue<float>() > 0.1f);

        /*if (primary.action.WasPressedThisFrame())
        {
            Debug.Log("Primary");
            canTP = !canTP;
        }*/

        if (!canTP)
        {
            LeftTP.enabled = false;
            RightTP.enabled = false;
        }
        else
        {
            LeftTP.enabled = true;
            RightTP.enabled = true;

        }
            if(leftActivate.action.WasReleasedThisFrame())AudioManager.instance.PlayClipAt(AudioManager.instance.allAudio["SFX_PasGrotteTP"], this.transform.position, AudioManager.instance.soundEffectMixer, true, false, 1, 1, 360, 1, 10);

    }
}

