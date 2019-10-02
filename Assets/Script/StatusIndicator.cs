using UnityEngine.UI;
using UnityEngine;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text uiText;
    [SerializeField]
    private Text statusIndicatorText;


    private void Start() {
        if (healthBarRect == null) {
            Debug.LogError("Status Indicator: No health bar object referenced");
        }
    }

    public void SetHealth(float _cur, float _max) {
        float _value = _cur / _max;

                         
                         
        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        statusIndicatorText.text = _cur + "/" + _max + " HP";
        if (transform.parent.tag == "Player") {
            uiText.text = statusIndicatorText.text;
        }


    }

}
