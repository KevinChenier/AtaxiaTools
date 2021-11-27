using TMPro;
using UnityEngine;

public class Tips : MonoBehaviour
{
    private TextMeshPro tipsText;

    // Start is called before the first frame update
    void Awake()
    {
        tipsText = GetComponent<TextMeshPro>();
        deactivateTip();
    }

    public void giveTip(string text)
    {
        tipsText.text = text;
    }

    private void deactivateTip()
    {
        tipsText.text = "";
    }
}
