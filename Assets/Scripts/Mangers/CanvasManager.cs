using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class CanvasManager : MonoBehaviour
{
    public AudioMixer mixer;

    [Header("Buuttons")]
    public Button startBtn;
    public Button quitBtn;
    public Button settingsBtn;
    public Button backBtn;
    public Button returnToMenu;

    [Header("Menu Canvases")]
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject hudCanvas;

    [Header("Text")]
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;
    public TMP_Text livesText;

    [Header("Sliders")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startBtn) startBtn.onClick.AddListener(() => ChangeScene("Game"));
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);
        if (settingsBtn) settingsBtn.onClick.AddListener(() => SetMenus(settingsCanvas, mainMenuCanvas));
        
        if (backBtn)
        {
            if (SceneManager.GetActiveScene().name == "Game") backBtn.onClick.AddListener(() => {
                SetMenus(hudCanvas, pauseMenuCanvas);
                Time.timeScale = 1f; // Resume the game
            });
            else backBtn.onClick.AddListener(() => SetMenus(mainMenuCanvas, settingsCanvas));
        }

        if (returnToMenu) returnToMenu.onClick.AddListener(() => 
        { 
            ChangeScene("Title");
            Time.timeScale = 1f; // Ensure time scale is reset when returning to the main menu
        });

        if (livesText)
        {
            GameManager.Instance.OnLivesChanged += UpdateLivesText;
            UpdateLivesText(GameManager.Instance.Lives); // Initialize the text with the current lives value
        }

        if (masterVolSlider)
        {
            SetupSliderInformation(masterVolSlider, masterVolSliderText, "MasterVol");
            OnSliderValueChanged(masterVolSlider.value, masterVolSlider, masterVolSliderText, "MasterVol"); // Initialize the text with the current value
        }
        if (musicVolSlider)
        {
            SetupSliderInformation(musicVolSlider, musicVolSliderText, "MusicVol");
            OnSliderValueChanged(musicVolSlider.value, musicVolSlider, musicVolSliderText, "MusicVol"); // Initialize the text with the current value
        }
        if (sfxVolSlider)
        {
            SetupSliderInformation(sfxVolSlider, sfxVolSliderText, "SFXVol");
            OnSliderValueChanged(sfxVolSlider.value, sfxVolSlider, sfxVolSliderText, "SFXVol"); // Initialize the text with the current value
        }

    }

    private void SetupSliderInformation(Slider slider, TMP_Text sliderText, string parameterName)
    { 
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        value = (value == 0) ? -80 : Mathf.Log10(slider.value) * 20; // Convert to decibels
        sliderText.text = (value == -80) ? "0%" : $"{(int)(slider.value * 100)}%";
        mixer.SetFloat(parameterName, value);
    }

    private void UpdateLivesText(int value) => livesText.text = $"Lives: {value}";

    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is null or empty.");
            return;
        }
        SceneManager.LoadScene(sceneName);
        //Debug.Log("Changing scene to: " + sceneName);
    }

    //platform specific so that if you are in the editor, it will stop the editor play mode otherwise it will quit the application
    private void QuitGame()
    { 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void SetMenus(GameObject canvasToActivate, GameObject canvasToDeactivate)
    {
        if (canvasToActivate) canvasToActivate.SetActive(true);
        if (canvasToDeactivate) canvasToDeactivate.SetActive(false);
    }

    private void Update()
    {
        if (!pauseMenuCanvas) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuCanvas.activeSelf)
            {
                SetMenus(hudCanvas, pauseMenuCanvas);
                Time.timeScale = 1f; // Resume the game
            }
            else
            {
                SetMenus(pauseMenuCanvas, hudCanvas);
                Time.timeScale = 0f; // Pause the game
            }
        }
    }
}
