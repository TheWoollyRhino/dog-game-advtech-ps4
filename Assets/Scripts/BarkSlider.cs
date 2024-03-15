using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BarkSlider : MonoBehaviour
{
    private Slider slider;
    private PlayerInput playerInput; // input action asset
    private bool isCharging = false;

    void Awake()
    {
        slider = GetComponent<Slider>();
        playerInput = new PlayerInput();

        playerInput.UI.BarkSliderFill.started += BarkSliderFillStarted;
        playerInput.UI.BarkSliderFill.canceled += BarkSliderFillCancelled;
    }
    void Update()
    {
    }

    private void OnEnable()
    {
        playerInput.UI.Enable();
    }

    private void OnDisable()
    {
        playerInput.UI.Disable();
    }

    private void BarkSliderFillStarted(InputAction.CallbackContext context)
    {
        isCharging = true;
        StartCoroutine(ChargeBarRoutine());
    }   
    
    private void BarkSliderFillCancelled(InputAction.CallbackContext context)
    {
        isCharging = false;
    }

    IEnumerator ChargeBarRoutine()
    {
        while (isCharging == true)
        {
            slider.value += 1.0f * Time.deltaTime;
            yield return null;
        }    
        
        while (isCharging == false)
        {
            slider.value -= 1.0f * Time.deltaTime;
            yield return null;
        }
    }


}
