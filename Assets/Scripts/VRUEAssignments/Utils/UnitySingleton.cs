using UnityEngine;

namespace VRUEAssignments.Utils
{
    public abstract class UnitySingleton<T> : MonoBehaviour
        where T : UnitySingleton<T>
    {
        private static T _instance;
        public static T Instance {  get { return _instance; } 
            private set {} }

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this as T;
            }
        }

    }
}
