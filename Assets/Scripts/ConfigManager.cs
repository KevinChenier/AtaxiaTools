using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{
    private static ConfigManager _instance;

    public Config Config;
    private HashSet<string> _possibleSceneNames;
    private EventBus bus;
    private Stopwatch sw;

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
        sw = new Stopwatch();
        LoadConfigs();
        InitPossibleScenes();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        bus = EventBus.Instance;
        sw.Start();
    }

    private void Update()
    {
        var controllers = OVRInput.GetConnectedControllers().GetFlags();
        if (controllers is null) return;
        var time = sw.ElapsedTicks;
        foreach(var controller in controllers)
        {
            if (controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.LHand)
            {
                var pos = OVRInput.GetLocalControllerPosition(controller);
                bus.Push(Assets.Scripts.Model.EventType.LeftControllerPosition, new { Ticks = time, Type=Assets.Scripts.Model.EventType.LeftControllerPosition, Value = new { x = pos.x, y = pos.y, z = pos.z } });
            } 
            else if (controller == OVRInput.Controller.RTouch || controller == OVRInput.Controller.RHand)
            {
                var pos = OVRInput.GetLocalControllerPosition(controller);
                bus.Push(Assets.Scripts.Model.EventType.RightControllerPosition, new { Ticks = time, Type=Assets.Scripts.Model.EventType.RightControllerPosition, Value = new { x = pos.x, y = pos.y, z = pos.z } });
            }
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
