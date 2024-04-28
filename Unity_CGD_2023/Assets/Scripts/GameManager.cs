using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance
    public GameObject explosions;
    public GameObject gameOverTransition;
    public Animator gameOverAnimator;

    private List<PlayerStats> players = new List<PlayerStats>(); // List of player characters

    private bool gameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one GameManager exists
            return;
        }
    }
    public void CheckPlayers()
    {
        PlayerStats[] playerObjects = FindObjectsOfType<PlayerStats>();
        foreach (PlayerStats player in playerObjects)
        {
            players.Add(player);
        }
    }

    private void Update()
    {
        if (!gameOver && players.Count > 0)
        {
            CheckPlayersStatus();
        }
    }

    private void CheckPlayersStatus()
    {
        bool allPlayersDead = true;

        foreach (PlayerStats player in players)
        {
            if (!player.isDead)
            {
                allPlayersDead = false;
                break;
            }
        }

        if (allPlayersDead)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOver = true;
        explosions.SetActive(true);
        StartCoroutine(GameOverTransition(1));
        // Add your game over logic here, such as showing a game over screen or restarting the level.
    }

    IEnumerator GameOverTransition(int index)
    {
        //gameOverTransition.SetActive(true);
        gameOverAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("ArcadeGameOver");
    }
}
