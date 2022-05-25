using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    LoadAd ads;
    [System.Serializable]
    public class GameState
    {
        public int playerHealth;
        public float playerFireRate;
        public float playerBulletSize;
        public float playerBulletSpeed;
        public int[] enemyHealth;
        public float[] enemySpeeds;
        public int level;
        public int wave;
        public int wavesPerLevel;
        public int currentScore;
        public int combo;
        public int highScore;
        public int skinNumber;
    }
    [SerializeField]
    AssignPoolToObjects pools;
    public GameState state;
    [System.Serializable]
    public class Wave
    {
        public string note;
        public PoolObject enemyPool;
        public Transform enemyPos;
        public int amount;
        public float spawnRate;


    }
    [SerializeField]
    Text levelWaveText;
    [SerializeField]
    GameObject player;
    bool paused = false;
    bool inGame = false;
    public Wave[] waves;
    [SerializeField]
    float timeBetweenWaves=5f;
    float waveCountdownTimer,waitCountdownTimer;
    [SerializeField]
    AudioSource source;
    [SerializeField]
    GameObject menuButton;
    [SerializeField]
    GameObject highScoreSplash;
    [SerializeField]
    GameObject continueButton;
    [SerializeField]
    Text splashHighScoreText;
    enum WaveState
    {
        spawning, waiting, counting
    }
    WaveState waveState = WaveState.waiting;
    public Text scoreBoard;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this);
        }
        state.wave = 0;
        LoadLevel();
        StartWave();
        
    }
    private void Update()
    {
        if(inGame == false)
        {
            return;
        }

        if (waveState == WaveState.spawning)
        {
            if (waveCountdownTimer <= 0)
            {
                
                StartCoroutine(SpawnWave(waves[state.wave]));
                waveState = WaveState.waiting;
            }
            else
            {
                waveCountdownTimer -= Time.deltaTime;
            }
        }
        else if (waveState == WaveState.waiting)
        {
            if (waitCountdownTimer <= 0)
            {
                if (AmountOfEnemiesAlive() == 0)
                {
                    if (state.wave > state.wavesPerLevel-1)
                    {
                        state.level++; // NEW LEVEL
                        UpgradeEnemy();
                        UpgradePlayer();
                        
                        SaveLevel();
                        state.wave = 0;
                        SetLevelText(true);
                    }
                    else
                    {
                        if (state.wave == 0) //so it does it for new levels
                        {
                            SetLevelText(true);
                           // Debug.Log("THIS");
                        }
                        else
                        {
                            SetLevelText(false);
                            Debug.Log("current wave " + state.wave);

                        }
                    }
                    StartWave();
                    
                    waveCountdownTimer = timeBetweenWaves;
                    waveState = WaveState.spawning;
                }
                waitCountdownTimer = timeBetweenWaves;
            }
            else
            {
                waitCountdownTimer -= Time.deltaTime;
            }
        }

        if (state.currentScore > state.highScore)
        {
            state.highScore = state.currentScore;
            SetHighScoreText();
        }
    }
    public void Pause()
    {
        paused = !paused;
        if (paused == true) //just paused the game
        {
            menuButton.SetActive(true);
            inGame = false;
            Time.timeScale = 0;

        }
        else                // just un paused the game

        {
            menuButton.SetActive(false);
            inGame = true;
            Time.timeScale = 1;
        }
    }
    public void QuitGame()
    {
        menuButton.SetActive(false);
        SaveLevel();
        inGame = false;
        player.SetActive(false);
        Time.timeScale = 1;

    }

    public void newGame()
    {
        inGame = true;
        if(paused == true)
        {
            Pause();
        }
        player.SetActive(true);
        StartWave();
    }
    public void PlaySound()
    {
        source.Play();
    }

    void SaveLevel()
    {
        PlayerPrefs.SetInt("skin", state.skinNumber);
        PlayerPrefs.SetInt("combo", state.combo);
        PlayerPrefs.SetInt("score", state.currentScore);
        PlayerPrefs.SetInt("highScore", state.highScore);
        PlayerPrefs.SetInt("l", state.level);
        PlayerPrefs.SetInt("pHealth", state.playerHealth);
        PlayerPrefs.SetInt("eHealth1", state.enemyHealth[0]);
        PlayerPrefs.SetInt("eHealth2", state.enemyHealth[1]);
        PlayerPrefs.SetInt("eHealth3", state.enemyHealth[2]);
        PlayerPrefs.SetFloat("eSpeed1", state.enemySpeeds[0]);
        PlayerPrefs.SetFloat("eSpeed2", state.enemySpeeds[1]);
        PlayerPrefs.SetFloat("eSpeed3", state.enemySpeeds[2]);
        PlayerPrefs.SetInt("WPL", state.wavesPerLevel);
        PlayerPrefs.SetFloat("PFR", state.playerFireRate);
        PlayerPrefs.SetFloat("PBS", state.playerBulletSize);
        PlayerPrefs.SetFloat("PBSp", state.playerBulletSpeed);
    }
    void LoadLevel()
    {
        state.skinNumber = PlayerPrefs.GetInt("skinNumber", 0);
        state.combo = PlayerPrefs.GetInt("combo", 1);
        state.currentScore = PlayerPrefs.GetInt("score", 0);
        state.highScore = PlayerPrefs.GetInt("highScore", 0);
        SetHighScoreText();
        scoreBoard.text = "Score: " + state.currentScore.ToString();
        state.level = PlayerPrefs.GetInt("l", 1);
        state.wavesPerLevel = PlayerPrefs.GetInt("WPL", 3);
        state.enemyHealth = new int[3];
        state.enemyHealth[0] = PlayerPrefs.GetInt("eHealth1", 1);
        state.enemyHealth[1] = PlayerPrefs.GetInt("eHealth2", 5);
        state.enemyHealth[2] = PlayerPrefs.GetInt("eHealth3", 20);
        state.enemySpeeds = new float[3];
        state.enemySpeeds[0] = PlayerPrefs.GetFloat("eSpeed1", 3.5f);
        state.enemySpeeds[1] = PlayerPrefs.GetFloat("eSpeed2", 2.0f);
        state.enemySpeeds[2] = PlayerPrefs.GetFloat("eSpeed3", 1.0f);
        state.playerHealth = PlayerPrefs.GetInt("pHealth", 3);
        state.playerBulletSize = PlayerPrefs.GetFloat("PBS", 0.25f);
        state.playerBulletSpeed= PlayerPrefs.GetFloat("PBSp", 40);
        state.playerFireRate = PlayerPrefs.GetFloat("PFR", 0.4f);
    }
    public void ResetLevel()
    {
        PlayerPrefs.SetInt("skinNumber", 0);
        PlayerPrefs.SetInt("combo", 1);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("l", 1);
        PlayerPrefs.SetInt("pHealth", 3);
        PlayerPrefs.SetInt("eHealth1", 3);
        PlayerPrefs.SetInt("eHealth2", 10);
        PlayerPrefs.SetInt("eHealth3", 20);
        PlayerPrefs.SetFloat("eSpeed1", 3.5f);
        PlayerPrefs.SetFloat("eSpeed2", 2.0f);
        PlayerPrefs.SetFloat("eSpeed3", 1.0f);
        PlayerPrefs.SetInt("WPL", 3);
        PlayerPrefs.SetFloat("PFR", 0.4f);
        PlayerPrefs.SetFloat("PBS", 0.25f);
        PlayerPrefs.SetFloat("PBSp", 40.0f);
        LoadLevel();
    }
    void SetLevelText(bool newLevel){
        if (newLevel == true)
        {
            if (state.level > 1)
            {
                ads.ShowAd();
                UpgradeEnemy();
            }
            levelWaveText.text = "Level " + state.level.ToString() + "\n" + "Wave " + (state.wave + 1).ToString();
        }
        else { levelWaveText.text = "\n" + "Wave " + (state.wave + 1).ToString(); }

        levelWaveText.gameObject.SetActive(true);
    }

    void StartWave()
    {
        waveCountdownTimer = timeBetweenWaves;

    }
    IEnumerator SpawnWave(Wave wave)
    {
        waveState = WaveState.spawning;
        for(int i = 0; i < wave.amount; i++)
        {
            SpawnEnemy(wave.enemyPool,wave.enemyPos.position);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }
        //spawn everything

        waveState = WaveState.waiting;
        Debug.Log("waiting");
        if(player.activeInHierarchy)
            state.wave++;
        yield break;
    }

    void SpawnEnemy(PoolObject pool, Vector3 position)
    {
        GameObject enemy = pool.GetGameObject();
        if (enemy != null)
        {
            enemy.transform.position = position;
            enemy.SetActive(true);
        }
    }
    int AmountOfEnemiesAlive()
    {        
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    void UpgradeEnemy()
    {
        int rand = (int)Random.Range(0, 10);
        switch (rand)
        {
            case 0:
                state.enemyHealth[0]++;
                foreach (GameObject obj in pools.pools[4].pooledObjects)
                {
                    obj.GetComponent<EnemyCollisions>().SetHealth(state.enemyHealth[0]);
                }
                break;

            case 1:
                state.enemyHealth[1]++;
                foreach (GameObject obj in pools.pools[5].pooledObjects)
                {
                    obj.GetComponent<EnemyCollisions>().SetHealth(state.enemyHealth[1]);
                }
                break;
            case 2:
                state.enemySpeeds[0]+=(state.enemySpeeds[0]*0.1f);
                foreach (GameObject obj in pools.pools[4].pooledObjects)
                {
                    obj.GetComponent<EnemyAI>().SetSpeed(state.enemySpeeds[0]);
                }
                break;
            case 3:
                state.enemySpeeds[1] += (state.enemySpeeds[1] * 0.1f);
                foreach (GameObject obj in pools.pools[5].pooledObjects)
                {
                    obj.GetComponent<EnemyAI>().SetSpeed(state.enemySpeeds[0]);
                }
                break;

            case 4:
                if (state.wavesPerLevel < waves.Length - 2)
                    state.wavesPerLevel++;
                break;
            case 5:
                int rand2 = (int)Random.Range(0, state.wavesPerLevel);
                waves[rand2].amount++;

                break;
            case 6:
                if (state.wavesPerLevel < waves.Length - 2)
                    state.wavesPerLevel++;
                break;

            case 7:
                if (state.wavesPerLevel < waves.Length - 2)
                    state.wavesPerLevel++;
                break;
            case 8:

                break;
            case 9:

                break;

            case 10:

                break;

        }
    }
    public void UpgradePlayer()
    {
        int rand = Random.Range(0, 6);
        switch (rand)
        {
            case 0:
                state.playerBulletSize += (state.playerBulletSize * 0.1f);
                foreach (GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.transform.localScale = new Vector3(state.playerBulletSize, state.playerBulletSize, 1);
                }
                break;

            case 1:
                state.playerFireRate -= (state.playerFireRate * 0.1f);
                break;
            case 2:
                state.playerBulletSize += (state.playerBulletSize * 0.1f);
                foreach(GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.transform.localScale = new Vector3(state.playerBulletSize, state.playerBulletSize, 1);
                }

                break;

            case 3:
                state.playerBulletSize += (state.playerBulletSize * 0.1f);
                foreach (GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.transform.localScale = new Vector3(state.playerBulletSize, state.playerBulletSize, 1);
                }
                break;
            case 4:
                state.playerBulletSpeed += (state.playerBulletSpeed * 0.1f);
                foreach (GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.GetComponent<MoveBullet>().SetSpeed(state.playerBulletSpeed);
                }
                break;

            case 5:
                state.playerBulletSpeed += (state.playerBulletSpeed * 0.1f);
                foreach (GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.GetComponent<MoveBullet>().SetSpeed(state.playerBulletSpeed);
                }
                break;
            case 6:
                state.playerFireRate -= (state.playerFireRate * 0.1f);
                break;

            case 7:
                state.playerBulletSize += (state.playerBulletSize * 0.1f);
                foreach (GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.transform.localScale = new Vector3(state.playerBulletSize, state.playerBulletSize, 1);
                }
                break;
            case 8:
                state.playerBulletSpeed += (state.playerBulletSpeed * 0.1f);
                foreach (GameObject obj in pools.pools[2].pooledObjects)
                {
                    obj.GetComponent<MoveBullet>().SetSpeed(state.playerBulletSpeed);
                }
                break;

            case 9:
                if (state.playerHealth > 3)
                {
               
                    if (PlayerCollisions.health < 3)
                        PlayerCollisions.health = 3;
                    state.playerHealth = PlayerCollisions.health;
                    PlayerCollisions.healthBarHandler.setImages(PlayerCollisions.health);

                }
                break;
            case 10:
                break;
        }
    }

    public void AddScore(int value)
    {
        state.currentScore += value * state.combo;
        state.combo++;
        scoreBoard.text = "Score: "+state.currentScore.ToString();
    }

    void SetHighScoreText()
    {
        if (state.highScore != 0)
        {
            highScoreSplash.SetActive(true);
            splashHighScoreText.text = "High Score \n" + state.highScore.ToString();
            continueButton.SetActive(true);
        }
    }
}
