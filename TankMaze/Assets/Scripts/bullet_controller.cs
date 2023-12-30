using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10f;
    Rigidbody2D bulletRigidBody;

    CircleCollider2D ColliderBullet;

    public int time_before_destroy = 500;
    public bool bullet_destroy = false; // ���������� �� ����

    public player_fire Player; // ����� �-� ���������

    private void Start()
    {
        ColliderBullet = GetComponent <CircleCollider2D>();
        // ������� ���� ���� ������ ����� ��� �������� �� ���� �����
        transform.Translate(new Vector3(0, 1.25f , 0)); 
        // ���������� ���� �������� ��������� (����� ��� ��������� ��� �� �������
        // ����), �� ����� ����������� � ����������� ���� ��� ���������� ������������
        ColliderBullet.isTrigger = false;
        bulletRigidBody = GetComponent<Rigidbody2D>();
        bulletRigidBody.velocity = transform.TransformDirection(new Vector2(0, bulletSpeed));
    }

    private void FixedUpdate()
    {
        if (time_before_destroy > 0) // ��������� ������� ������� �������� ����
        {
            time_before_destroy--;
        }
        else bullet_destroy = true;

        if (bullet_destroy) BulletDestroy(); // ��������� ����
    }

    public void BulletDestroy() 
    {
        Player.bullets_fired--; // ��������� ����� ���������� ���� � ������ �-� ���������
        // ����������� ���� ��������� ���� ��� ������������ ����
        if (time_before_destroy > 0) main_game_script.bullet_punch = true;
        else main_game_script.bullet_over = true;
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����������� ���� �������� ���� �� ����� ��� ������ ����
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Bullet")
            main_game_script.bullet_ricoshet = true;
    }
}