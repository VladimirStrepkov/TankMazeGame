using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10f;
    Rigidbody2D bulletRigidBody;

    CircleCollider2D ColliderBullet;

    public int time_before_destroy = 500;
    public bool bullet_destroy = false; // уничтожить ли пулю

    public player_fire Player; // игрок к-й выстрелил

    private void Start()
    {
        ColliderBullet = GetComponent <CircleCollider2D>();
        // Двигаем пулю чуть вперед чтобы она вылетала из дула танка
        transform.Translate(new Vector3(0, 1.25f , 0)); 
        // Изначально пуля является триггером (чтобы при появлении она не двигала
        // танк), но после перемещения к координатам дула она становится материальной
        ColliderBullet.isTrigger = false;
        bulletRigidBody = GetComponent<Rigidbody2D>();
        bulletRigidBody.velocity = transform.TransformDirection(new Vector2(0, bulletSpeed));
    }

    private void FixedUpdate()
    {
        if (time_before_destroy > 0) // проверяем сколько времени осталось пуле
        {
            time_before_destroy--;
        }
        else bullet_destroy = true;

        if (bullet_destroy) BulletDestroy(); // разрушаем пулю
    }

    public void BulletDestroy() 
    {
        Player.bullets_fired--; // уменьшаем число выпущенных пуль у игрока к-й выстрелил
        // Проигрываем звук исчезания пули или столкновения пули
        if (time_before_destroy > 0) main_game_script.bullet_punch = true;
        else main_game_script.bullet_over = true;
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проигрываем звук рикошета пули от стены или другой пули
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bullet")
            main_game_script.bullet_ricoshet = true;
    }
}