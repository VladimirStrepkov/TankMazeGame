using UnityEngine;
using UnityEngine.SceneManagement;

public class players_mode : MonoBehaviour
{
    private void Awake()
    {
        // Заполняем массив с названиями уровней
        for (int i = 1; i <= main_game_script.countLevels; i++)
        {
            string s = "maze" + i.ToString();
            main_game_script.Levels[i - 1] = s;
        }
    }

    // режим игры выбирается с помощью кнопок в начале, функции кнопок прописаны тут
    public void Select2PlayerMode()
    {
        main_game_script.TwoPlayers = true;
        SceneManager.LoadScene("howscore");
    }
    public void Select3PlayerMode()
    {
        main_game_script.TwoPlayers = false;
        SceneManager.LoadScene("howscore");
    }
    public void Score10()
    {
        main_game_script.numberScoreForFinish = 10;
        SceneManager.LoadScene("keypads");
    }
    public void Score25()
    {
        main_game_script.numberScoreForFinish = 25;
        SceneManager.LoadScene("keypads");
    }
    public void Score50()
    {
        main_game_script.numberScoreForFinish = 50;
        SceneManager.LoadScene("keypads");
    }
    public void Score100()
    {
        main_game_script.numberScoreForFinish = 100;
        SceneManager.LoadScene("keypads");
    }
}