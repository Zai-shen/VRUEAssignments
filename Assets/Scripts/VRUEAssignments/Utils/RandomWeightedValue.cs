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
        
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Generate();
            }
        }

        public T[] Generate()
        {
            // Generate a combination of values
            T[] combination = GenerateCombination(WeightedValues.Count);

            // Print the generated combination
            Debug.Log("Generated Combination: " + string.Join(", ", combination));

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
                Debug.Log("Iteration nr" + i);
                if (combination.Count == length)
                {
                    break;
                }
            }

            return combination.ToArray();
        }
    }
}