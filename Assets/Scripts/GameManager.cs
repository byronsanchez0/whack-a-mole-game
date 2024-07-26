using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Mole> moles;
   

    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject playButton1;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private GameObject bombText1;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    // Hardcoded variables you may want to tune,
    private float startingTime = 30f;
    // Global variables
    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;
    private bool playing = false;

    //// the hammer
    public GameObject hammerPrefab;
    public Transform canvasTransform;
    private Animator animator;

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }


    
    private void Start()
    
    {

        animator = GetComponent<Animator>();

    }

    // This is public so the play button can see it.
    public void StartGame()
    {
        // Hide/show the UI elements we don't/do want to see.
        playButton.SetActive(false);
        playButton1.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        bombText1.SetActive(false);
        gameUI.SetActive(true);
        // Hide all the visible moles.
        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        // Remove any old game state.
        currentMoles.Clear();
        // Start with 30 seconds.
        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "0";
        playing = true;




    }

  //private void OnMouseDown()
  //{
  //      RaycastHit2D Ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up * laserLength);
  //      if (Ray.collider != null)
  //      {
  //          Debug.Log("si clickea");
  //          Ray.collider.gameObject.GetComponent<Animator>().SetBool("rayo", true);

  //      }
  //  }
    private IEnumerator AnimateHammer(GameObject hammer)
    {
        // Set initial position and rotation
        RectTransform hammerRectTransform = hammer.GetComponent<RectTransform>();
        hammerRectTransform.anchoredPosition = new Vector2(2.5f, 0); // Adjust initial position as needed
        hammerRectTransform.rotation = Quaternion.Euler(0, 0, 0);

        float animationDuration = 5f; // Duration of the animation
        float elapsedTime = 0f;

        Vector2 startPosition1 = new Vector2(2.5f, 0);
        Vector2 midPosition = new Vector2(2.5f, 0);
        Vector2 endPosition = new Vector2(2.5f, 0); // Adjust end position as needed

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // Animate position and rotation
            hammerRectTransform.anchoredPosition = Vector2.Lerp(startPosition1, endPosition, t);
            hammerRectTransform.rotation = Quaternion.Euler(0.8f, 0, Mathf.Lerp(0.5f, 45, t));

            yield return null;
        }
        // Reset elapsed time for the return animation
        elapsedTime = 0f;

        // Animate back to the start position
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // Animate position and rotation back to the start position
            hammerRectTransform.anchoredPosition = Vector2.Lerp(midPosition, endPosition, t);
            hammerRectTransform.rotation = Quaternion.Euler(0.8f, 0, Mathf.Lerp(35, 0.5f, t));

            yield return null;
        }

        // Destroy the hammer after animation
        Destroy(hammer);
    }


    public void GameOver(int type)
    {
        // Show the message.
        if (type == 0)
        {
            outOfTimeText.SetActive(true);
        }
        else
        {
            bombText.SetActive(true);
            bombText1.SetActive(true);
        }
        // Hide all moles.
        foreach (Mole mole in moles)
        {
            mole.StopGame();
        }
        // Stop the game and show the start UI.
        playing = false;
        playButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {


            // Update time.
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            // Check if we need to start any more moles.
            if (currentMoles.Count <= (score / 10))
            {
                // Choose a random mole.
                int index = Random.Range(0, moles.Count);
                // Doesn't matter if it's already doing something, we'll just try again next frame.
                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
            }
        }

        //if(Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit2D Ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up * laserLength);
        //    if(Ray.collider != null)
        //    {
        //        Debug.Log("si clickea");
        //        Ray.collider.gameObject.GetComponent<Animator>().SetBool("rayo", true);

        //    }
        //}
    }


    public void AddScore(int moleIndex)
    {
        // Add and update score.
        score += 1;
        scoreText.text = $"{score}";
        // Increase time by a little bit.
        timeRemaining += 1;
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }

    public void Missed(int moleIndex, bool isMole)
    {
        if (isMole)
        {
            // Decrease time by a little bit.
            timeRemaining -= 2;
        }
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }
}
