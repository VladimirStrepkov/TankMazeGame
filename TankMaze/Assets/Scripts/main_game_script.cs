using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class main_game_script : MonoBehaviour
{
    // Названия уровней для раундов
    public static int countLevels = 35; // Сколько уровней в игре
    public static string[] Levels = new string[countLevels];

    // игровые объекты игроков (перетаскиваем вручную)
    public GameObject Player1, Player2, Player3;

    // получаем доступ к скриптам здоровья игроков и скриптам их стрельбы
    private player_health PlayerHealth1, PlayerHealth2, PlayerHealth3;
    private player_fire PlayerFire1, PlayerFire2, PlayerFire3;

    // Префабы бонусов
    public GameObject Health_up, Without_damage, Upgrade;

    // Сколько очков нужно набрать чтобы игра закончилась
    // начальное значение >0 т.к. иначе сцены отдельно не посмотреть (запускается финишное меню)
    public static int numberScoreForFinish = 10;

    // очки игроков
    public static int scorePlayer1 = 0, scorePlayer2 = 0, scorePlayer3 = 0;

    // Играют два игрока или три
    public static bool TwoPlayers = false;

    private bool finishRound = false;        // заканчивается ли раунд
    private int timeBeforeFinishRound = 150; // Время до следующего раунда когда этот закончился

    private const int TIME_NEXT_BONUS = 400;   // Сколько времени нужно на появление каждого бонуса
    public int timeNextBonus = TIME_NEXT_BONUS; // Сколько времени до следующего бонуса

    private const int MAX_NUMBER_BONUSES = 5; // максимальное число бонусов на карте
    public int numberBonuses = 0;             // Сколько сейчас бонусов на карте

    public GameObject marker1; // маркеты для определения где будут спавниться бонусы
    public GameObject marker2; // первый слева снизу (минимум), второй справа сверху (максимум)

    public TMP_Text textPlayer1Health; // Надписи здоровья игроков
    public TMP_Text textPlayer2Health;
    public TMP_Text textPlayer3Health;

    public TMP_Text textPlayer1Score;  // Надписи очков игроков
    public TMP_Text textPlayer2Score;
    public TMP_Text textPlayer3Score;

    public GameObject iconUpgradePlayer1;  // Иконки бонуса апгрейда
    public GameObject iconUpgradePlayer2;
    public GameObject iconUpgradePlayer3;

    public GameObject iconDefencePlayer1;  // Иконки бонуса неуязвимости
    public GameObject iconDefencePlayer2;
    public GameObject iconDefencePlayer3;

    public static int win_player = 0; // Номер победившего в прошлом раунде игрока (0 если никто не победил)

    public AudioClip punch;                  // звук попадания пули во что-то
    public static bool bullet_punch = false; // Попала ли пуля во что-то (нужно для проигрывания звука)

    public AudioClip rikoshet;                  // звук рикошета пули
    public static bool bullet_ricoshet = false; // Отрекошетила ли пуля

    public AudioClip bullet_over_audio;        // Звук исчезания пуль
    public static bool bullet_over = false;    // исчезла ли пуля

    public AudioClip fire;                  // звук выстрела
    public static bool bullet_fire = false; // Произошёл ли выстрел

    public AudioClip CreateingBonus;         // Звук появления бонуса
    public static bool bonusCreated = false; // появился ли новый бонус

    public AudioClip GetBonus;              // звук получения бонуса
    public static bool bonusGeting = false; // Получен ли кем-то бонус
    private void FixedUpdate()
    {
        if (timeNextBonus > 0) timeNextBonus--;
        else
        {
            timeNextBonus = TIME_NEXT_BONUS;
            if (numberBonuses < MAX_NUMBER_BONUSES)
            {
                numberBonuses++;
                int numBon = UnityEngine.Random.Range(1, 4); // тип бонуса
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

        // Если умерло 1+ игрок из 2 или 2+ из 3
        if ((PlayerHealth1.is_death + PlayerHealth2.is_death + PlayerHealth3.is_death >= 2 && !TwoPlayers) ||
            (PlayerHealth1.is_death + PlayerHealth2.is_death >= 1 && TwoPlayers))
            finishRound = true;

        // Конец раунда
        if (finishRound && timeBeforeFinishRound > 0) timeBeforeFinishRound--;
        else if (finishRound && timeBeforeFinishRound == 0)
        {
            if (PlayerHealth1.is_death == 0) win_player = 1;
            if (PlayerHealth2.is_death == 0) win_player = 2;
            if (PlayerHealth3.is_death == 0) win_player = 3;
            NextLevel(); // случайно генерируем уровень
        }

        // Обновление надписей здоровья игроков
        textPlayer1Health.text = PlayerHealth1.health + "/" + player_health.MAX_HEALTH;
        textPlayer2Health.text = PlayerHealth2.health + "/" + player_health.MAX_HEALTH;
        textPlayer3Health.text = PlayerHealth3.health + "/" + player_health.MAX_HEALTH;

        // Исчезновение надписи здоровья при смерти игрока
        if (PlayerHealth1.health <= 0) textPlayer1Health.gameObject.SetActive(false);
        if (PlayerHealth2.health <= 0) textPlayer2Health.gameObject.SetActive(false);
        if (PlayerHealth3.health <= 0) textPlayer3Health.gameObject.SetActive(false);

        // Проигрывание звука попадания пули
        if (bullet_punch) playClip(ref bullet_punch, punch);
        // Проигрывание звука рикошета пули
        if (bullet_ricoshet) playClip(ref bullet_ricoshet, rikoshet);
        // Проигрывание звука исчезания пули
        if (bullet_over) playClip(ref bullet_over, bullet_over_audio);
        // Проигрывание звука выстрела
        if (bullet_fire) playClip(ref bullet_fire, fire);
        // Проигрывание звука появления бонуса
        if (bonusCreated) playClip(ref bonusCreated, CreateingBonus);
        // Проигрывание звука получения бонуса
        if (bonusGeting) playClip(ref bonusGeting, GetBonus);

        // Появление иконок и исчезновение при подборе бонуса
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
        // Меняем положение интерфейса второго игрока при игре в 2 игрока
        if (TwoPlayers)
        {
            textPlayer2Health.gameObject.transform.Translate(new Vector2(850, 0));
            textPlayer2Score.gameObject.transform.Translate(new Vector2(850, 0));
            iconUpgradePlayer2.gameObject.transform.Translate(new Vector2(850, 0));
            iconDefencePlayer2.gameObject.transform.Translate(new Vector2(850, 0));
        }

        // Добавляем очко игроку к-й выйграл прошлый раунд, если кто-то его выйграл
        if (win_player == 1) scorePlayer1++;
        if (win_player == 2) scorePlayer2++;
        if (win_player == 3) scorePlayer3++;
        win_player = 0;

        // Проверяем, набрал ли кто-то из игроков нужно количество очков, если да, то завершаем игру
        if (scorePlayer1 >= numberScoreForFinish || scorePlayer2 >= numberScoreForFinish ||
            scorePlayer3 >= numberScoreForFinish)
        {
            SceneManager.LoadScene("finish"); // завершающая сцена в игре
        }

        PlayerHealth1 = Player1.GetComponent<player_health>();
        PlayerHealth2 = Player2.GetComponent<player_health>();
        PlayerHealth3 = Player3.GetComponent<player_health>();
        PlayerFire1 = Player1.GetComponent<player_fire>();
        PlayerFire2 = Player2.GetComponent<player_fire>();
        PlayerFire3 = Player3.GetComponent<player_fire>();

        // надписи очков игроков
        textPlayer1Score.text = scorePlayer1.ToString();
        textPlayer2Score.text = scorePlayer2.ToString();
        textPlayer3Score.text = scorePlayer3.ToString();


        // Отключаем третьего игрока если режим игры на двоих
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
        // Случайно генерируем номер следующего уровня
        int number_next_level = UnityEngine.Random.Range(0, Levels.Length);
        // загружаем этот уровень
        SceneManager.LoadScene(Levels[number_next_level]);
    }

    // Проигрывание звука какого-либо события
    private void playClip(ref bool Event, AudioClip Clip) {
        Event = false;
        GetComponent<AudioSource>().PlayOneShot(Clip);
    }
}