using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSystem : MonoBehaviour
{
   public FloatValue ringValue;
   public FloatValue ringCapacity;
   public FloatValue coreValue;
   public float regenerationRate;
   public float depletionRate;

   private void Update()
   {
      if (Math.Abs(coreValue.value) < float.Epsilon) return;
      
      coreValue.value = Math.Max(0, coreValue.value - depletionRate * Time.deltaTime);
      
      if(Math.Abs(ringValue.value - ringCapacity.value)< float.Epsilon) return;
      
      float newValue = ringValue.value + coreValue.value * regenerationRate * Time.deltaTime;

      Debug.Log(newValue);
      
      ringValue.value = Math.Min(newValue, ringCapacity.value);
   }
}
