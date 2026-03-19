using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeliverySystem : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject parcelPrefab;
    public GameObject deliveryPrefab;

    [Header("Spawn Points")]
    public Transform[] parcelSpots;
    public Transform[] deliverySpots;

    [Header("Main UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    [Header("Hint UI (Temporary)")]
    public GameObject hintPanel; 
    public TextMeshProUGUI hintText;
    public float hintDuration = 3f;

    public GameObject endPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;

    [Header("Game Settings")]
    public float gameDuration = 30f;
    public int goalScore = 5;

    private int score = 0;
    public bool hasPackage = false;
    private float timeLeft;
    private bool gameEnded = false;
    
    public AudioSource successAudio;

    void Start() {
        timeLeft = gameDuration;
        endPanel.SetActive(false);
        hintPanel.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        SpawnNewParcel();
        ShowHint("FIND THE YELLOW PACKAGE!", Color.yellow);
        UpdateScoreUI();
    }

    void Update() {
        if (gameEnded) return;

        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Ceil(timeLeft).ToString();
            
            if (timeLeft <= 10) timeText.color = Color.red;
        } 
        else {
            GameOver();
        }
    }

    void SpawnNewParcel() {
        int index = Random.Range(0, parcelSpots.Length);
        Vector3 spawnPos = new Vector3(parcelSpots[index].position.x, 0.5f, parcelSpots[index].position.z);
        Instantiate(parcelPrefab, spawnPos, Quaternion.identity);
    }

    void SpawnDeliveryZone() {
        int index = Random.Range(0, deliverySpots.Length);
        Vector3 spawnPos = new Vector3(deliverySpots[index].position.x, 0f, deliverySpots[index].position.z);
        Instantiate(deliveryPrefab, spawnPos, Quaternion.identity);
    }

    void OnTriggerEnter(Collider other) {
        if (gameEnded) return;

        if (other.CompareTag("Parcel") && !hasPackage) {
            hasPackage = true;
            Destroy(other.gameObject);
            SpawnDeliveryZone();
            ShowHint("PACKAGE PICKED UP! GO TO GREEN ZONE", Color.green);
        }

        if (other.CompareTag("DeliveryZone") && hasPackage) {
    hasPackage = false;
    score++;
    
    if (successAudio != null) {
        successAudio.Play();
    }

    UpdateScoreUI();
    Destroy(other.gameObject);
    
    if (score >= goalScore) {
        WinGame();
    } else {
        SpawnNewParcel();
        ShowHint("WELL DONE! GET THE NEXT ONE", Color.green);
    }
}
    }

    public void ShowHint(string message, Color color) {
        StopAllCoroutines();
        StartCoroutine(HintRoutine(message, color));
    }

    IEnumerator HintRoutine(string message, Color color) {
        hintText.text = message;
        hintText.color = color;
        hintPanel.SetActive(true);
        yield return new WaitForSeconds(hintDuration);
        if (!gameEnded) hintPanel.SetActive(false);
    }

    void UpdateScoreUI() {
        if (scoreText != null) scoreText.text = "Packages: " + score + "/" + goalScore;
    }

    void WinGame() {
        gameEnded = true;
        ShowEndScreen("YOU WIN!", Color.cyan);
    }

    void GameOver() {
        gameEnded = true;
        timeLeft = 0;
        timeText.text = "Time: 0";
        ShowEndScreen("GAME OVER!", Color.red);
    }

    void ShowEndScreen(string message, Color color) {
        hintPanel.SetActive(false); 
        endPanel.SetActive(true);
        resultText.text = message;
        resultText.color = color;
        finalScoreText.text = "Final Score: " + score;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }
}