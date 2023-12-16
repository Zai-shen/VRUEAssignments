using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRUEAssignments.Utils
{
    public class RandomWeightedValue<T> : MonoBehaviour
    {
        public List<WeightedValue<T>> WeightedValues;

        public T GetRandomValue(List<WeightedValue<T>> weightedValueList)
        {
            T output = default;

            //Getting a random weight value
            var totalWeight = 0;
            foreach (var entry in weightedValueList)
            {
                totalWeight += entry.Weight;
            }
            var rndWeightValue = Random.Range(1, totalWeight + 1);

            //Checking where random weight value falls
            var processedWeight = 0;
            foreach (var entry in weightedValueList)
            {
                processedWeight += entry.Weight;
                if (rndWeightValue <= processedWeight)
                {
                    output = entry.Value;
                    break;
                }
            }

            return output;
        }

        public T[] Generate()
        {
            T[] combination = GenerateCombination(WeightedValues.Count);

            // Debug.Log("Generated Combination: " + string.Join(", ", combination));
            return combination;
        }

        private T[] GenerateCombination(int length)
        {
            HashSet<T> combination = new HashSet<T>();

            int maxTries = 10000;
            
            for (int i = 0; i < maxTries; i++)
            {
                T randomValue = GetRandomValue(WeightedValues);
                combination.Add(randomValue);
                if (combination.Count == length)
                {
                    break;
                }
            }

            return combination.ToArray();
        }
    }
}