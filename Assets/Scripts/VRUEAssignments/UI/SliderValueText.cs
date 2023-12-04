using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRUEAssignments.UI
{
    public class SliderValueText : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The text shown will be formatted using this string.  {0} is replaced with the actual value")]
        private string formatText = "{0}m";
        public int DivideBy = 0;

        private TextMeshProUGUI tmproText;

        private void Start()
        {
            tmproText = GetComponent<TextMeshProUGUI>();
            Slider parentSlider = GetComponentInParent<Slider>();
            parentSlider.onValueChanged.AddListener(HandleValueChanged);
            HandleValueChanged(parentSlider.value);
        }

        private void HandleValueChanged(float value)
        {
            tmproText.SetText(string.Format(formatText, (DivideBy == 0 ? value : value / DivideBy)));
        }
    }
}
