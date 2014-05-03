using UnityEngine;
using System.Collections;

namespace UnityProse {

  public class SmoothDamper {

    float inputSensitivity ;
    public float minValue;
    public float maxValue;
    public float smoothingFactor;
    public bool wrapValue;

    private float currentValue;
    private float smoothedValue;
    private float velocity;
    private float maxRange;

    public SmoothDamper(
      float min = 0f, 
      float max = 360f, 
      float smoothing = 0f, 
      float sensitivity = 1f, 
      float startingValue = 0f,
      bool wrap = false)
    {
      if (min >= max)
        throw new MinValueGreaterThanOrEqualToMaxExcaption();

      minValue = min;
      maxValue = max;
      smoothingFactor = smoothing;
      inputSensitivity = sensitivity;
      currentValue = startingValue;
      wrapValue = wrap;
      maxRange = maxValue - minValue;
    }

    public float SmoothedValue(float deltaValue) {
      currentValue = (currentValue + deltaValue * inputSensitivity);

      // Not sure if this is necessary yet.  Theoretically you could have a really high smoothing value
      // and high deltaValue.  Not sure if that's a practical ussage though.  Commenting out for now.
      // AdjustForMaxRange();

      if (!wrapValue) ClampCurrentValue();
      smoothedValue = Mathf.SmoothDamp(smoothedValue, currentValue, ref velocity, smoothingFactor);
      if (wrapValue) WrapCurrentValue();
      return smoothedValue;
    }

    void AdjustForMaxRange() {
      if (currentValue > smoothedValue && currentValue - smoothedValue > maxRange) {
        currentValue = smoothedValue + maxRange;
      } else if(smoothedValue - currentValue > maxRange) {
        currentValue = smoothedValue - maxRange;;
      }
    }

    void ClampCurrentValue() {
      currentValue =  Mathf.Clamp(currentValue, minValue, maxValue);    
    }

    void WrapCurrentValue() {
      if (smoothedValue < minValue) {
        smoothedValue += maxRange;
        currentValue += maxRange;
      }
      if (smoothedValue > maxValue) {
        smoothedValue -= maxRange;
        currentValue -= maxRange;
      }
    }
  }

  class MinValueGreaterThanOrEqualToMaxExcaption : System.Exception {}
}
