using UnityEngine;

public class player_health : MonoBehaviour
{
    public const int MAX_HEALTH = 3;
    public int health = MAX_HEALTH;

    private const int GREEN_COLOR_TIME_AFTER_HEALTH_UP = 25;
    private const int WHITE_COLOR_TIME_AFTER_DAMAGE = 20;
    private int before_original_color = 0;
    private SpriteRenderer SP;

    private const int MAX_TIME_WITHOUT_DAMAGE = 700; // макс врем€ неу€звимости
    public int timeWithoutDamage = 0;

    public int is_death = 0; // умер ли игрок
    private void Start()
    {
        SP = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (timeWithoutDamage <= 0)
            {
                health--;
                before_original_color = WHITE_COLOR_TIME_AFTER_DAMAGE;
                SP.color = Color.white;
            }
            // ѕолучаем доступ к управл€ющему пулей скрипту и говорим ему уничтожить пулю
            BulletController bullet_script = collision.gameObject.GetComponent<BulletController>();
            bullet_script.bullet_destroy = true;
        }
        if (collision.gameObject.tag == "Health_up")
        {
            health = MAX_HEALTH;
            if (timeWithoutDamage <= 0)
            {
                before_original_color = GREEN_COLOR_TIME_AFTER_HEALTH_UP;
                SP.color = Color.green;
            }
            bonus_controller bonus_script = collision.gameObject.GetComponent<bonus_controller>();
            bonus_script.bonusDestroy = true;
        }
        if (collision.gameObject.tag == "Without_damage")
        {
            timeWithoutDamage = MAX_TIME_WITHOUT_DAMAGE;
            SP.color = Color.white;
            before_original_color = MAX_TIME_WITHOUT_DAMAGE;
            bonus_controller bonus_script = collision.gameObject.GetComponent<bonus_controller>();
            bonus_script.bonusDestroy = true;
        }
    }
    private void FixedUpdate()
    {
        // танк становитс€ белым когда в него попадает пул€
        if (before_original_color > 0) before_original_color--;
        else if (gameObject.name == "Player_1") SP.color = Color.red;
        else if (gameObject.name == "Player_2") SP.color = Color.yellow;
        else if (gameObject.name == "Player_3") SP.color = Color.blue;

        if (timeWithoutDamage > 0) timeWithoutDamage--;

        // —мерть игрока
        if (health <= 0)
        {
            is_death = 1;
            gameObject.SetActive(false);
        }
    }
}