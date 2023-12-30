using UnityEngine;

public class bonus_controller : MonoBehaviour
{
    public bool bonusDestroy = false;    // ����������� �� �����
    public main_game_script main_script; // ������ �� ������� ������� ������
    private int time_before_audio = 5;   // ����� �� ����� ����� ��������� ������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����� �������� ��� ������������ �� ������ ������ ���� �������� � ���, � �� ������ ����������
        if (collision.gameObject.tag == "Wall" && time_before_audio != -1)
        {
            bonusDestroy = true;
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            bonusDestroy = true;
            BulletController bc = collision.gameObject.GetComponent<BulletController>();
            bc.bullet_destroy = true;
        }
        // ����������� ���� ��������� ������
        else if (collision.gameObject.tag == "Player") main_game_script.bonusGeting = true;
    }

    private void Update()
    {
        // ����������� ���� ��������� ������ � ���������
        if (time_before_audio > 0) time_before_audio--;
        else if (time_before_audio == 0)
        {
            time_before_audio = -1;
            main_game_script.bonusCreated = true;
        }
        // ���������� �����
        if (bonusDestroy) BonusDestroy();
    }

    private void BonusDestroy()
    {
        main_script.numberBonuses--;
        Destroy(gameObject);
    }
}