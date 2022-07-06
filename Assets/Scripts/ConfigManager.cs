using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ConfigManager : MonoBehaviour
{
    private static ConfigManager _instance;

    public Config Config;
    public ScenarioManager ScenarioManager;
    private HashSet<string> _possibleSceneNames;

    public static ConfigManager Instance
    {
        get { return _instance; }
    }

    public HashSet<string> PossibleSceneNames { get => _possibleSceneNames; }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        LoadConfigs();
        InitPossibleScenes();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // There seems to be a bug concerning the loading of locales, Invoke is necessary until fix.
        Invoke("LoadLocale", 0.1f);
        if (Config.ScenarioActive)
        {
            UnityEngine.Debug.Log("Starting scenario!");
            ScenarioManager = new ScenarioManager(Config.ScenarioConfig.ToolsOrder);
            ScenarioManager.InitScenario();
        }
    }

    public T GetToolConfig<T>(string name) where T: IToolConfig
    {
        var conf = Config.ToolConfigs.All().Where(tc => tc.Name == name).First();
        return (T)conf;
    }

    private void LoadConfigs()
    {
        using var r = new StreamReader("appsettings.json");
        Config = JsonUtility.FromJson<Config>(r.ReadToEnd());
    }

    public void SaveConfigs()
    {
        var json = JsonUtility.ToJson(Config);
        using var w = new StreamWriter("appsettings.json");
        w.Write(json);
    }

    public void LoadLocale()
    {
        LocaleIdentifier localeCode = new LocaleIdentifier(Config.Locale); // can be "en" "de" "ja" etc.
        LocalizationSettings.SelectedLocale = LocalizationSettings.Instance.GetAvailableLocales().Locales.First(x => x.Identifier == localeCode);
    }

    public IEnumerable<(string, string)> GetMenuOptions()
    {
        return Config.ToolConfigs.All().Select(tc => (tc.MenuLabel, tc.SceneName));
    }


    public void InitPossibleScenes()
    {
        int count = SceneManager.sceneCountInBuildSettings;
        _possibleSceneNames = new HashSet<string>();
        for (var i = 0; i < count; i++)
        {
            _possibleSceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
    }
}
