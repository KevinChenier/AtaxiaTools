using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private ConfigManager ConfigManager;

    public BaseTool tool;

    private string sceneName;
    private Canvas canvas;
    private bool IsMenu = true;

    private void Start()
    {
        ConfigManager = ConfigManager.Instance;
        sceneName = SceneManager.GetActiveScene().name;
       // ControllerInputEvent.Instance.StartUpEvent += ToggleMenu;
        canvas = GetComponent<Canvas>();
        CreateOptionButtons();
        if (tool != null)
        {
            IsMenu = false;
            tool.Show();
            HideCanvas();
        }
    }

    private void CreateOptionButtons()
    {
        var options = ConfigManager.GetMenuOptions();
        var baseButton = gameObject.transform.GetChild(0);
        var layout = LayoutHelper.Menu(2, options.Count(), (baseButton as RectTransform).rect, 2);
        var count = 0;
        foreach (var option in options)
        {
            var b = Instantiate(baseButton);
            b.name = $"ButtonActivity{option.Item1}";
            b.transform.SetParent(transform);
            b.localScale = new Vector3(1, 1, 1);
            b.localPosition = layout[count++];
            var text = b.GetComponentInChildren<Text>();
            text.text = option.Item1;
            SetOnClick(b.GetComponent<Button>(), option.Item2);
        }
        Destroy(baseButton.gameObject);
    }

    public void ToggleMenu(object source, EventArgs args)
    {
        if (tool == null) return;

        if (IsMenu)
        {
            tool.Show();
            HideCanvas();
            IsMenu = false;
        }
        else
        {
            tool.Pause();
            ShowCanvas();
            IsMenu = true;
        }
    }

    void SetOnClick(Button b, string sceneName)
    {
        if (this.sceneName == sceneName || !ConfigManager.PossibleSceneNames.Contains(sceneName))
        {
            b.enabled = false;
        }
        else
        {
            b.onClick.AddListener(() => GoTo(sceneName));
        }
    }

    void GoTo(string sceneName)
    {
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void ShowCanvas()
    {
        canvas.gameObject.SetActive(true);
    }

    void HideCanvas()
    {
        canvas.gameObject.SetActive(false);
    }
}
