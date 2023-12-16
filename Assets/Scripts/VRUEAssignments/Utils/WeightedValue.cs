using System;

namespace VRUEAssignments.Utils
{
    [Serializable]
    public class WeightedValue<T>
    {
        public T Value;
        public int Weight;
    }
}