using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Character _playerCharacter;
    [SerializeField] Game_UI_Manager _UI_Manager;

    bool isGameOver;
    bool isGameFinished;

    private void Awake()
    {
        _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

        if(_playerCharacter == null ) 
        {
            Debug.LogError("There is no character with Player Tag. Please add Player Tag to proper character Object");
        }
    }

    public void GameOver()
    {
        _UI_Manager.ShowGameOverUI();
    }

    public void GameFinished()
    {
        _UI_Manager.ShowGameFinishedUI();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _UI_Manager.TogglePauseUI();
        }
        if (_playerCharacter.GetCurrentState() == Character.CharacterState.Dead)
        {
            isGameOver = true;
            GameOver();
        }
    }

    public Character GetCharacter()
    {
        return _playerCharacter;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
