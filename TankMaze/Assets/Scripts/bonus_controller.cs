using UnityEngine;

public class bonus_controller : MonoBehaviour
{
    public bool bonusDestroy = false;    // уничтожитс€ ли бонус
    public main_game_script main_script; // ссылка на главный игровой скрипт
    private int time_before_audio = 5;   // ¬рем€ до звука после по€влени€ бонуса
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ѕонус исчезает при столкновении со стеной только если по€вилс€ в ней, а не просто столкнулс€
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
        // ѕроигрываем звук получени€ бонуса
        else if (collision.gameObject.tag == "Player") main_game_script.bonusGeting = true;
    }

    private void Update()
    {
        // проигрываем звук по€влени€ бонуса с задержкой
        if (time_before_audio > 0) time_before_audio--;
        else if (time_before_audio == 0)
        {
            time_before_audio = -1;
            main_game_script.bonusCreated = true;
        }
        // ”ничтожаем бонус
        if (bonusDestroy) BonusDestroy();
    }

    private void BonusDestroy()
    {
        main_script.numberBonuses--;
        Destroy(gameObject);
    }
}