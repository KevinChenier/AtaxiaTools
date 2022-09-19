using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UserInterfaceManager : MonoBehaviour
{
    public Menu menu;
    public Button GoButton;
    public Button ResetButton;
    public Tips tips;

    private static UserInterfaceManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public static UserInterfaceManager Instance
    {
        get { return _instance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GoButton.gameObject.SetActive(false);
    }

    public void OnClickButtonGo()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
