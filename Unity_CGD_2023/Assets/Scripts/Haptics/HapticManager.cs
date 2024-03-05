using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : MonoBehaviour
{
    public static HapticManager Instance { get; private set; } = null;

    List<HapticEffectSO> ActiveEffects = new List<HapticEffectSO>();

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Attempting to create second HapticManager on " + gameObject.name);
            Destroy(gameObject);
            return;
        }
    }


    void Update()
    {
        float lowSpeedMotor = 0f;
        float highSpeedMotor = 0f;
        for(int i = 0; i < ActiveEffects.Count; i++)
        {
            var effect = ActiveEffects[i];
            //tick effect and clean up if finished
            float lowSpeedComponent = 0f;
            float highSpeedComponent = 0f;
            if (effect.Tick(Camera.main.transform.position, out lowSpeedComponent, out highSpeedComponent))
            {
                ActiveEffects.RemoveAt(i);
                --i;
            }

            lowSpeedMotor = Mathf.Clamp01(lowSpeedComponent + lowSpeedMotor);
            highSpeedMotor = Mathf.Clamp01(highSpeedComponent + highSpeedMotor);
        }

        Gamepad.current.SetMotorSpeeds(lowSpeedMotor, highSpeedMotor);
    }

    void OnDestroy()
    {
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    public static void PlayEffect(HapticEffectSO effect, Vector3 location)
    {
        Instance.PlayEffect_Internal(effect, location);
    }

    void PlayEffect_Internal(HapticEffectSO effect, Vector3 location)
    {
        var ActiveEffect = ScriptableObject.Instantiate(effect);
        ActiveEffect.Initialise(location);

        ActiveEffects.Add(ActiveEffect);
    }
}
