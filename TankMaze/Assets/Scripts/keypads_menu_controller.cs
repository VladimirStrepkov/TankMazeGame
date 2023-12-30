using UnityEngine;
using UnityEngine.SceneManagement;

public class keypads_menu_controller : MonoBehaviour
{
    public GameObject keypadsPlayer3;
    public GameObject spritePlayer3;
    private void Start()
    {
        if (main_game_script.TwoPlayers == false)
        {
            keypadsPlayer3.SetActive(true);
            spritePlayer3.SetActive(true);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            main_game_script.NextLevel();
        }
    }
}