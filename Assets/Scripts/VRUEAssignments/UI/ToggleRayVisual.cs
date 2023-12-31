using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRUEAssignments.UI
{
    public class ToggleRayVisual : MonoBehaviour
    {
        private XRInteractorLineVisual _xrInteractorLineVisual;

        private void Start()
        {
            _xrInteractorLineVisual = GetComponent<XRInteractorLineVisual>();
            Hide();
        }

        public void Show()
        {
            _xrInteractorLineVisual.enabled = true;
        }
    
        public void Hide()
        {
            _xrInteractorLineVisual.enabled = false;
        }
    }
}
