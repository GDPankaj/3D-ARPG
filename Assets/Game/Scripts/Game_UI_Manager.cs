using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game_UI_Manager : MonoBehaviour
{
    [SerializeField] GameManager _GM;
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] Slider _healthSlider;

    [SerializeField] GameObject _pauseUI, _gameOverUI, _gameFinishedUI;

    enum Game_UI_State
    {
        GamePlay, Pause, GameOver, GameFinished
    }

    Game_UI_State _currentState;

    private void Start()
    {
        SwitchUIState(Game_UI_State.GamePlay);
    }
    void Update()
    {
        _healthSlider.value = _GM.GetCharacter().GetComponent<Health>().GetHealthRatio();
        _coinText.text = _GM.GetCharacter().GetCoins().ToString();
    }

    void SwitchUIState(Game_UI_State state)
    {
        _pauseUI.SetActive(false);
        _gameOverUI.SetActive(false);
        _gameFinishedUI.SetActive(false);

        Time.timeScale = 1;

        switch (state)
        {
            case Game_UI_State.GamePlay:
                break;

            case Game_UI_State.Pause:
                Time.timeScale = 0f;
                _pauseUI.SetActive(true);
                break;

            case Game_UI_State.GameOver:
                _gameOverUI.SetActive(true);
                break;

            case Game_UI_State.GameFinished:
                _gameFinishedUI.SetActive(true);
                break;
        }
        _currentState = state;
    }

    public void TogglePauseUI()
    {
        if (_currentState == Game_UI_State.GamePlay) { SwitchUIState(Game_UI_State.Pause); }
        else if(_currentState == Game_UI_State.Pause){ SwitchUIState(Game_UI_State.GamePlay); }
    }

    public void ButtonMainMenu()
    {
        _GM.ReturnToMainMenu();
    }

    public void ButtonRestart()
    {
        _GM.Restart();
    }

    public void ShowGameOverUI()
    {
        SwitchUIState(Game_UI_State.GameOver);
    }

    public void ShowGameFinishedUI()
    {
        SwitchUIState(Game_UI_State.GameFinished);
    }
}
