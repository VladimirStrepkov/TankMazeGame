using UnityEngine;
using System.Collections.Generic;
using System;

public class moving_wall_controller : MonoBehaviour
{
    public float x_speed = 1.5f; // �������� ����������� ����� �� ����
    public float y_speed = 1.5f;

    public List <GameObject> movePoints = new List <GameObject>(); // ����� ����������� ����� (�������)

    private int i = 0; // ������ �����, � �-� ������ �������� �����

    private bool xIsHere = false; // �������� �� �� ��������� �����
    private bool yIsHere = false;

    public int timeGoToNextPoint = 0; // ������� ������� ����� ����� ������ � ������ �����
    private int thisTimeGoToNextPoint;

    private void Awake()
    {
        thisTimeGoToNextPoint = timeGoToNextPoint;
    }
    private void FixedUpdate()
    {
        // ��������� � ����� ���� � �� ��������
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

        // ��� � ��������� �����. ���� ������� ���������, �� ����� ��� � ������ � �� �����
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