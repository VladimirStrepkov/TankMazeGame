using UnityEngine;
using System.Collections.Generic;
using System;

public class moving_wall_controller : MonoBehaviour
{
    public float x_speed = 1.5f; // Скорости перемещения стены по осям
    public float y_speed = 1.5f;

    public List <GameObject> movePoints = new List <GameObject>(); // Точки перемещения стены (маркеры)

    private int i = 0; // Индекс точки, к к-й сейчас движется стена

    private bool xIsHere = false; // Достигли ли мы координат точки
    private bool yIsHere = false;

    public int timeGoToNextPoint = 0; // Сколько времени стена будет стоять в каждой точке
    private int thisTimeGoToNextPoint;

    private void Awake()
    {
        thisTimeGoToNextPoint = timeGoToNextPoint;
    }
    private void FixedUpdate()
    {
        // Двигаемся к точке если её не достигли
        xIsHere = false;
        yIsHere = false;
        if (Math.Abs(gameObject.transform.position.x - movePoints[i].transform.position.x) >= x_speed * Time.fixedDeltaTime)
        {
            if (gameObject.transform.position.x < movePoints[i].transform.position.x)
                gameObject.transform.Translate(new Vector2(x_speed * Time.fixedDeltaTime, 0));
            else gameObject.transform.Translate(new Vector2(-x_speed * Time.fixedDeltaTime, 0));
        }
        else xIsHere = true;
        if (Math.Abs(gameObject.transform.position.y - movePoints[i].transform.position.y) >= y_speed * Time.fixedDeltaTime)
        {
            if (gameObject.transform.position.y < movePoints[i].transform.position.y)
                gameObject.transform.Translate(new Vector2(0, y_speed * Time.fixedDeltaTime));
            else gameObject.transform.Translate(new Vector2(0, -y_speed * Time.fixedDeltaTime));
        }
        else yIsHere = true;

        // Идём к следующей точке. Если текущая последняя, то снова идём к первой и по кругу
        if (xIsHere && yIsHere)
        {
            if (thisTimeGoToNextPoint > 0) thisTimeGoToNextPoint--;
            else
            {
                if (i < movePoints.Count - 1) i++;
                else i = 0;
                thisTimeGoToNextPoint = timeGoToNextPoint;
            }
        }
    }
}