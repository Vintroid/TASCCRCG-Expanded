using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))] // We want an audioSource
public class PlayerManager : MonoBehaviour
{
    [Header("Score & Wave UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource mainCameraAudioSource;
    [SerializeField] private AudioClip gameOverAudio;

    // UnityEvent for the mode change
    [Header("Event")]
    [SerializeField] private UnityEvent onModeChanged;
    public UnityEvent OnModeChanged => onModeChanged;

    
    // Fields
    public int Score { get; private set; }
    public int WaveNumber { get; private set; }
    public bool IsGameOver { get; private set; }
    public PlayerMode Mode { get; private set; } = PlayerMode.Basic;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ValidateReferences();
    }

    private void Start()
    {
        UpdateScoreText();
        UpdateWaveText();

        // Hide game over on startup
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            ReturnToTitleScreen();
        }
    }

    public void AddScore(int amount)
    {
        if(!IsGameOver)
        {
            Score += amount;
            UpdateScoreText();
        }
    }

    public void SetWave(int waveNumber)
    {
        if (IsGameOver)
        {
            return;
        }

        WaveNumber = waveNumber;
        UpdateWaveText();
        PlayWaveAudio();
        AddScore(10);




    }

    private void PlayWaveAudio()
    {
        if(audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    private void PlayGameOverAudio()
    {
        if(mainCameraAudioSource != null)
        {
            mainCameraAudioSource.Stop();

            if(gameOverAudio != null)
            {
                mainCameraAudioSource.PlayOneShot(gameOverAudio);
            }
        }
    }

    private void ShowGameOverUI()
    {
        if(gameOverText != null) {

            gameOverText.text = "Game Over\n\nScore: " + Score + "\nWave: " + WaveNumber + "\n\nPress Space to exit";
        }

        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    public void TriggerGameOver()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        ShowGameOverUI();
        PlayGameOverAudio();
    }

    public void ChangeMode(PlayerMode newMode)
    {
        Mode = newMode;
        Debug.Log($"Player mode changed to: {Mode}");

        OnModeChanged.Invoke();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Score}";
        }
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {WaveNumber}";
        }
    }

    private static void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    private void ValidateReferences()
    {
        if (scoreText == null)
        {
            Debug.LogWarning(
                $"{name}: Score Text has not been assigned.",
                this
            );
        }

        if (waveText == null)
        {
            Debug.LogWarning(
                $"{name}: Wave Text has not been assigned.",
                this
            );
        }

        if (gameOverText == null)
        {
            Debug.LogWarning(
                $"{name}: Game Over Text has not been assigned.",
                this
            );
        }

        if (gameOverPanel == null)
        {
            Debug.LogWarning(
                $"{name}: Game Over Panel has not been assigned.",
                this
            );
        }

        if (mainCameraAudioSource == null)
        {
            Debug.LogWarning(
                $"{name}: Main Camera Audio Source has not been assigned.",
                this
            );
        }
    }
}
