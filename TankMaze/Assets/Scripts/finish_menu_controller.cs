using UnityEngine;
using UnityEngine.SceneManagement;

public class finish_menu_controller : MonoBehaviour
{
    public GameObject Player1Sprite;
    public GameObject Player2Sprite;
    public GameObject Player3Sprite;
    public GameObject Player1Text;
    public GameObject Player2Text;
    public GameObject Player3Text;

    private void Start()
    {
        // Высвечивается надпись кто победил и появляется картинка соответствующего игрока
        if (main_game_script.scorePlayer1 == main_game_script.numberScoreForFinish)
        {
            Player1Sprite.SetActive(true);
            Player1Text.SetActive(true);
        }
        else if (main_game_script.scorePlayer2 == main_game_script.numberScoreForFinish)
        {
            Player2Sprite.SetActive(true);
            Player2Text.SetActive(true);
        }
        else if (main_game_script.scorePlayer3 == main_game_script.numberScoreForFinish)
        {
            Player3Sprite.SetActive(true);
            Player3Text.SetActive(true);
        }
    }
    private void Update()
    {
        // Рестарт игры
        if (Input.GetKeyDown(KeyCode.E))
        {
            main_game_script.scorePlayer1 = 0;
            main_game_script.scorePlayer2 = 0;
            main_game_script.scorePlayer3 = 0;
            main_game_script.win_player = 0;
            SceneManager.LoadScene("how_much_players");
        }
    }
}