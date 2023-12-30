using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class main_game_script : MonoBehaviour
{
    // �������� ������� ��� �������
    public static int countLevels = 35; // ������� ������� � ����
    public static string[] Levels = new string[countLevels];

    // ������� ������� ������� (������������� �������)
    public GameObject Player1, Player2, Player3;

    // �������� ������ � �������� �������� ������� � �������� �� ��������
    private player_health PlayerHealth1, PlayerHealth2, PlayerHealth3;
    private player_fire PlayerFire1, PlayerFire2, PlayerFire3;

    // ������� �������
    public GameObject Health_up, Without_damage, Upgrade;

    // ������� ����� ����� ������� ����� ���� �����������
    // ��������� �������� >0 �.�. ����� ����� �������� �� ���������� (����������� �������� ����)
    public static int numberScoreForFinish = 10;

    // ���� �������
    public static int scorePlayer1 = 0, scorePlayer2 = 0, scorePlayer3 = 0;

    // ������ ��� ������ ��� ���
    public static bool TwoPlayers = false;

    private bool finishRound = false;        // ������������� �� �����
    private int timeBeforeFinishRound = 150; // ����� �� ���������� ������ ����� ���� ����������

    private const int TIME_NEXT_BONUS = 400;   // ������� ������� ����� �� ��������� ������� ������
    public int timeNextBonus = TIME_NEXT_BONUS; // ������� ������� �� ���������� ������

    private const int MAX_NUMBER_BONUSES = 5; // ������������ ����� ������� �� �����
    public int numberBonuses = 0;             // ������� ������ ������� �� �����

    public GameObject marker1; // ������� ��� ����������� ��� ����� ���������� ������
    public GameObject marker2; // ������ ����� ����� (�������), ������ ������ ������ (��������)

    public TMP_Text textPlayer1Health; // ������� �������� �������
    public TMP_Text textPlayer2Health;
    public TMP_Text textPlayer3Health;

    public TMP_Text textPlayer1Score;  // ������� ����� �������
    public TMP_Text textPlayer2Score;
    public TMP_Text textPlayer3Score;

    public GameObject iconUpgradePlayer1;  // ������ ������ ��������
    public GameObject iconUpgradePlayer2;
    public GameObject iconUpgradePlayer3;

    public GameObject iconDefencePlayer1;  // ������ ������ ������������
    public GameObject iconDefencePlayer2;
    public GameObject iconDefencePlayer3;

    public static int win_player = 0; // ����� ����������� � ������� ������ ������ (0 ���� ����� �� �������)

    public AudioClip punch;                  // ���� ��������� ���� �� ���-��
    public static bool bullet_punch = false; // ������ �� ���� �� ���-�� (����� ��� ������������ �����)

    public AudioClip rikoshet;                  // ���� �������� ����
    public static bool bullet_ricoshet = false; // ������������ �� ����

    public AudioClip bullet_over_audio;        // ���� ��������� ����
    public static bool bullet_over = false;    // ������� �� ����

    public AudioClip fire;                  // ���� ��������
    public static bool bullet_fire = false; // ��������� �� �������

    public AudioClip CreateingBonus;         // ���� ��������� ������
    public static bool bonusCreated = false; // �������� �� ����� �����

    public AudioClip GetBonus;              // ���� ��������� ������
    public static bool bonusGeting = false; // ������� �� ���-�� �����
    private void FixedUpdate()
    {
        if (timeNextBonus > 0) timeNextBonus--;
        else
        {
            timeNextBonus = TIME_NEXT_BONUS;
            if (numberBonuses < MAX_NUMBER_BONUSES)
            {
                numberBonuses++;
                int numBon = UnityEngine.Random.Range(1, 4); // ��� ������
                float x = UnityEngine.Random.Range(marker1.transform.position.x, marker2.transform.position.x);
                float y = UnityEngine.Random.Range(marker1.transform.position.y, marker2.transform.position.y);
                GameObject bonus = null;
                if (numBon == 1) bonus = Instantiate(Health_up, new Vector3(x, y, 0), Quaternion.Euler(0f, 0f, 0f));
                else if (numBon == 2) bonus = Instantiate(Without_damage, new Vector3(x, y, 0), Quaternion.Euler(0f, 0f, 0f));
                else if (numBon == 3) bonus = Instantiate(Upgrade, new Vector3(x, y, 0), Quaternion.Euler(0f, 0f, 0f));
                bonus_controller bonus_script = bonus.GetComponent<bonus_controller>();
                bonus_script.main_script = GetComponent<main_game_script>();
            }
        }

        // ���� ������ 1+ ����� �� 2 ��� 2+ �� 3
        if ((PlayerHealth1.is_death + PlayerHealth2.is_death + PlayerHealth3.is_death >= 2 && !TwoPlayers) ||
            (PlayerHealth1.is_death + PlayerHealth2.is_death >= 1 && TwoPlayers))
            finishRound = true;

        // ����� ������
        if (finishRound && timeBeforeFinishRound > 0) timeBeforeFinishRound--;
        else if (finishRound && timeBeforeFinishRound == 0)
        {
            if (PlayerHealth1.is_death == 0) win_player = 1;
            if (PlayerHealth2.is_death == 0) win_player = 2;
            if (PlayerHealth3.is_death == 0) win_player = 3;
            NextLevel(); // �������� ���������� �������
        }

        // ���������� �������� �������� �������
        textPlayer1Health.text = PlayerHealth1.health + "/" + player_health.MAX_HEALTH;
        textPlayer2Health.text = PlayerHealth2.health + "/" + player_health.MAX_HEALTH;
        textPlayer3Health.text = PlayerHealth3.health + "/" + player_health.MAX_HEALTH;

        // ������������ ������� �������� ��� ������ ������
        if (PlayerHealth1.health <= 0) textPlayer1Health.gameObject.SetActive(false);
        if (PlayerHealth2.health <= 0) textPlayer2Health.gameObject.SetActive(false);
        if (PlayerHealth3.health <= 0) textPlayer3Health.gameObject.SetActive(false);

        // ������������ ����� ��������� ����
        if (bullet_punch) playClip(ref bullet_punch, punch);
        // ������������ ����� �������� ����
        if (bullet_ricoshet) playClip(ref bullet_ricoshet, rikoshet);
        // ������������ ����� ��������� ����
        if (bullet_over) playClip(ref bullet_over, bullet_over_audio);
        // ������������ ����� ��������
        if (bullet_fire) playClip(ref bullet_fire, fire);
        // ������������ ����� ��������� ������
        if (bonusCreated) playClip(ref bonusCreated, CreateingBonus);
        // ������������ ����� ��������� ������
        if (bonusGeting) playClip(ref bonusGeting, GetBonus);

        // ��������� ������ � ������������ ��� ������� ������
        if (PlayerHealth1.timeWithoutDamage > 0 && PlayerHealth1.is_death == 0) iconDefencePlayer1.SetActive(true);
        else iconDefencePlayer1.SetActive(false);
        if (PlayerHealth2.timeWithoutDamage > 0 && PlayerHealth2.is_death == 0) iconDefencePlayer2.SetActive(true);
        else iconDefencePlayer2.SetActive(false);
        if (PlayerHealth3.timeWithoutDamage > 0 && PlayerHealth3.is_death == 0) iconDefencePlayer3.SetActive(true);
        else iconDefencePlayer3.SetActive(false);

        if (PlayerFire1.timeUpgrade > 0 && PlayerHealth1.is_death == 0) iconUpgradePlayer1.SetActive(true);
        else iconUpgradePlayer1.SetActive(false);
        if (PlayerFire2.timeUpgrade > 0 && PlayerHealth2.is_death == 0) iconUpgradePlayer2.SetActive(true);
        else iconUpgradePlayer2.SetActive(false);
        if (PlayerFire3.timeUpgrade > 0 && PlayerHealth3.is_death == 0) iconUpgradePlayer3.SetActive(true);
        else iconUpgradePlayer3.SetActive(false);
    }

    private void Awake()
    {
        // ������ ��������� ���������� ������� ������ ��� ���� � 2 ������
        if (TwoPlayers)
        {
            textPlayer2Health.gameObject.transform.Translate(new Vector2(850, 0));
            textPlayer2Score.gameObject.transform.Translate(new Vector2(850, 0));
            iconUpgradePlayer2.gameObject.transform.Translate(new Vector2(850, 0));
            iconDefencePlayer2.gameObject.transform.Translate(new Vector2(850, 0));
        }

        // ��������� ���� ������ �-� ������� ������� �����, ���� ���-�� ��� �������
        if (win_player == 1) scorePlayer1++;
        if (win_player == 2) scorePlayer2++;
        if (win_player == 3) scorePlayer3++;
        win_player = 0;

        // ���������, ������ �� ���-�� �� ������� ����� ���������� �����, ���� ��, �� ��������� ����
        if (scorePlayer1 >= numberScoreForFinish || scorePlayer2 >= numberScoreForFinish ||
            scorePlayer3 >= numberScoreForFinish)
        {
            SceneManager.LoadScene("finish"); // ����������� ����� � ����
        }

        PlayerHealth1 = Player1.GetComponent<player_health>();
        PlayerHealth2 = Player2.GetComponent<player_health>();
        PlayerHealth3 = Player3.GetComponent<player_health>();
        PlayerFire1 = Player1.GetComponent<player_fire>();
        PlayerFire2 = Player2.GetComponent<player_fire>();
        PlayerFire3 = Player3.GetComponent<player_fire>();

        // ������� ����� �������
        textPlayer1Score.text = scorePlayer1.ToString();
        textPlayer2Score.text = scorePlayer2.ToString();
        textPlayer3Score.text = scorePlayer3.ToString();


        // ��������� �������� ������ ���� ����� ���� �� �����
        if (TwoPlayers)
        {
            Player3.SetActive(false);
            textPlayer3Health.gameObject.SetActive(false);
            textPlayer3Score.gameObject.SetActive(false);
            PlayerHealth3.is_death = 1;
        }
    }

    public static void NextLevel()
    {
        // �������� ���������� ����� ���������� ������
        int number_next_level = UnityEngine.Random.Range(0, Levels.Length);
        // ��������� ���� �������
        SceneManager.LoadScene(Levels[number_next_level]);
    }

    // ������������ ����� ������-���� �������
    private void playClip(ref bool Event, AudioClip Clip) {
        Event = false;
        GetComponent<AudioSource>().PlayOneShot(Clip);
    }
}