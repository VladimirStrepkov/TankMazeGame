using UnityEngine;
public class exit_from_game : MonoBehaviour
{
    // Скрипт для того чтобы можно было выйти из игры нажав escape
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}