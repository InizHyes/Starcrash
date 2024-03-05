using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Haptic Effect", fileName = "HapticEffect")]

public class HapticEffectSO : ScriptableObject
{
    public enum EType
    {
        OneShot,
        Continuous
    }

    [SerializeField] EType Type = EType.OneShot;

    [SerializeField] float Duration = 0f;

    [SerializeField] float LowSpeedIntensity = 1f;
    [SerializeField] AnimationCurve LowSpeedMotor;

    [SerializeField] float HighSpeedIntensity = 1f;
    [SerializeField] AnimationCurve HighSpeedMotor;

    [SerializeField] bool VariesWithDistance = false;
    [SerializeField] float MaxDistance = 25f;
    [SerializeField] AnimationCurve FallOffCurve;

    [System.NonSerialized] Vector3 EffectLocation;
    [System.NonSerialized] float Progress;
    public void Initialise(Vector3 _EffectLocation)
    {
        EffectLocation = _EffectLocation;
        Progress = 0f;
    }

    public bool Tick(Vector3 receiverPos, out float lowSpeed, out float highSpeed)
    {
        //update progress
        Progress += Time.deltaTime / Duration;

        //calculate distance for rumble falloff
        float distanceFactor = 1f;
        if (VariesWithDistance)
        {
            float distance = (receiverPos - EffectLocation).magnitude;
            distanceFactor =  distance >= MaxDistance ? 0f : FallOffCurve.Evaluate(distance / MaxDistance);
        }

        lowSpeed = LowSpeedIntensity * distanceFactor * LowSpeedMotor.Evaluate(Progress);
        highSpeed = HighSpeedIntensity * distanceFactor * HighSpeedMotor.Evaluate(Progress);

        //check if complete
        if (Progress >= 1f)
        {
            if (Type == EType.OneShot)
                return true;
            Progress = 0f;
        }
        return false;
    }
}
