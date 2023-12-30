using UnityEngine;

public class player_fire : MonoBehaviour
{
    public GameObject bullet;
    private const int UPGRADE_RELOAD = 5;  // �����������
    private const int DEFAULT_RELOAD = 20;
    private int _reload = DEFAULT_RELOAD;
    private int _time_to_shoot = 0;

    private const int UPGRADE_MAX_BULLETS_FIRED = 20; // ������� ���� ����� ���������
    private const int DEFAULT_MAX_BULLETS_FIRED = 6;
    private int maxBulletsFired = DEFAULT_MAX_BULLETS_FIRED;
    public int bullets_fired = 0;

    private const int MAX_TIME_UPGRADE = 500; // ����. ����� ��������
    public int timeUpgrade = 0;

    // ����� ������������ ���� ��� ��������
    private const int UPGRADE_TIME_BEFORE_DESTROY = 300;
    // �������� ���� ��� ��������
    private const float UPGRADE_BULLET_SPEED = 14f;

    private void FixedUpdate()
    {
        if      (((Input.GetKey(KeyCode.Q) && _time_to_shoot == 0 && gameObject.name == "Player_1") ||
            (Input.GetKey(KeyCode.RightControl) && _time_to_shoot == 0 && gameObject.name == "Player_2") ||
                  (Input.GetKey(KeyCode.Keypad7) && _time_to_shoot == 0 && gameObject.name == "Player_3")) 
                  && bullets_fired < maxBulletsFired)
        {
            // ���������� �����������
            _time_to_shoot = _reload;
            // ������� ���� �� ����� ������ � ��� ���������
            GameObject bul = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
            bul.transform.rotation = gameObject.transform.rotation;
            bul.transform.Rotate(new Vector3(0, 0 ,-90));
            bullets_fired++;
            // �������� ������ � ������� �-� ������������ ����
            BulletController bullet_script = bul.GetComponent<BulletController>();
            // �������� ��� ������ �� ����������� ������ ������ �-� ���������
            bullet_script.Player = gameObject.GetComponent<player_fire>();

            // �������
            // ���� ���������� ��������� ����� � ����� �� ��� ������, �� ����� �������
            if (timeUpgrade > 0)
            {
                bullet_script.time_before_destroy = UPGRADE_TIME_BEFORE_DESTROY;
                bul.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                SpriteRenderer sp = bul.GetComponent<SpriteRenderer>();
                sp.color = Color.white;
                bullet_script.bulletSpeed = UPGRADE_BULLET_SPEED;
            }
            main_game_script.bullet_fire = true; // ����������� ���� ��������
        }
        if (_time_to_shoot > 0) _time_to_shoot--;
        if (timeUpgrade > 0)
        {
            timeUpgrade--;
            _reload = UPGRADE_RELOAD;
            maxBulletsFired = UPGRADE_MAX_BULLETS_FIRED;
        }
        else
        {
            _reload = DEFAULT_RELOAD;
            maxBulletsFired = DEFAULT_MAX_BULLETS_FIRED;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Upgrade")
        {
            timeUpgrade = MAX_TIME_UPGRADE;
            bonus_controller bonus_script = collision.gameObject.GetComponent<bonus_controller>();
            bonus_script.bonusDestroy = true;
        }
    }
}