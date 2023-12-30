using UnityEngine;

public class rotate_wall_controller : MonoBehaviour
{
    public float degreesOfRotate = 360; // Сколько градусов в одном повороте
    public float speedOfRotate = 1;     // Скорость каждого поворота
    public int timeToNextRotate = 0;    // Время между поворотами

    private int thisTimeToNextRotate;
    private float thisDegreesOfRotate;

    private bool correct = false; // произошла ли корректировка поворота
    private void Awake()
    {
        thisTimeToNextRotate = timeToNextRotate;
        thisDegreesOfRotate = 0;
    }

    private void FixedUpdate()
    {
        if (thisDegreesOfRotate < degreesOfRotate)
        {
            thisDegreesOfRotate += speedOfRotate * Time.fixedDeltaTime;
            gameObject.transform.Rotate(new Vector3(0, 0, speedOfRotate * Time.fixedDeltaTime));
        }
        else
        {
            if (!correct) // Корректируем поворот в конце
            {
                gameObject.transform.Rotate(new Vector3(0, 0, degreesOfRotate - thisDegreesOfRotate));
                correct = true;
            }
            if (thisTimeToNextRotate > 0) thisTimeToNextRotate--;
            else
            {
                thisTimeToNextRotate = timeToNextRotate;
                thisDegreesOfRotate = 0;
                correct = false;
            }
        }
    }
}