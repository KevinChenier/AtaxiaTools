using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Components;


public class Tips : MonoBehaviour
{
    public GameObject background;
    public LocalizeStringEvent Localization;

    // Start is called before the first frame update
    void Awake()
    {
        deactivateTip();
    }


    public void giveTip(string tableEntryReference)
    {
        Localization.StringReference.TableEntryReference = tableEntryReference;
        background.SetActive(true);
    }

    public void deactivateTip()
    {
        Localization.StringReference.TableEntryReference = "Nothing";
        background.SetActive(false);
    }
}
