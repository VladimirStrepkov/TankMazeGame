using UnityEngine;
public class exit_from_game : MonoBehaviour
{
    // ������ ��� ���� ����� ����� ���� ����� �� ���� ����� escape
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}