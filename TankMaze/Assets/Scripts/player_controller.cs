using UnityEngine;
public class player_controller : MonoBehaviour
{
    private float _speed = 200f;
    private float _rotationSpeed = 120f;
    private Rigidbody2D tankRigidBody;
    private void Start()
    {
        tankRigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        float v = 0;
        if       ((Input.GetKey(KeyCode.W) && gameObject.name == "Player_1") ||
            (Input.GetKey(KeyCode.UpArrow) && gameObject.name == "Player_2") ||
                  (Input.GetKey(KeyCode.Keypad8) && gameObject.name == "Player_3")) v = 1;
        else if  ((Input.GetKey(KeyCode.S) && gameObject.name == "Player_1") ||
            (Input.GetKey(KeyCode.DownArrow) && gameObject.name == "Player_2") ||
                  (Input.GetKey(KeyCode.Keypad5) && gameObject.name == "Player_3")) v = -1;
        tankRigidBody.velocity = transform.TransformDirection(new Vector3(v * _speed * Time.fixedDeltaTime, 0, 0));

        float rotation = 0;
        if       ((Input.GetKey(KeyCode.A) && gameObject.name == "Player_1") ||
            (Input.GetKey(KeyCode.LeftArrow) && gameObject.name == "Player_2") ||
                  (Input.GetKey(KeyCode.Keypad4) && gameObject.name == "Player_3")) rotation = 1;
        else if  ((Input.GetKey(KeyCode.D) && gameObject.name == "Player_1") ||
            (Input.GetKey(KeyCode.RightArrow) && gameObject.name == "Player_2") ||
                  (Input.GetKey(KeyCode.Keypad6) && gameObject.name == "Player_3")) rotation = -1;
        transform.Rotate(new Vector3(0, 0, rotation * _rotationSpeed * Time.fixedDeltaTime));
    }
}