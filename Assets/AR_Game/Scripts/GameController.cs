using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AR_Game.Scripts
{
    public class GameController : MonoBehaviour
    {
        private static GameController Instance { get; set; }

        private int _score;
        
        [Header("Other")]
        public Enemy enemyPrefab;
        
        [Header("UI")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI countDownText;
        
        public Button startButton;
        public Button restartButton;
        public Button exitButton;
        public Button quitButton;
        public Button pauseMenuButton;

        public GameObject inGameMenu;
        public GameObject mainMenu;
        public GameObject pauseMenu;
        
        public Transform playerCameraTransform;

        public Material[] enemyMaterials;

        public Transform enemyContainer;
        
        
        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Application.targetFrameRate = 30;
                Instance = this;
                return;
            }
        
            Destroy(gameObject);
        }

        private void Start()
        {
            InitializeGame();
        }

        #endregion
        
        private void InitializeGame()
        {
            //Add events to to the ui buttons.
            startButton.onClick.AddListener(NewGame);
            
            restartButton.onClick.AddListener(NewGame);
            
            exitButton.onClick.AddListener(() =>
            {
                pauseMenu.SetActive(false);
                inGameMenu.SetActive(false);
                mainMenu.SetActive(true);
            });
            
            pauseMenuButton.onClick.AddListener(() =>
            {
                inGameMenu.SetActive(false);
                mainMenu.SetActive(false);
                pauseMenu.SetActive(true);
            });
            
            quitButton.onClick.AddListener(Application.Quit);
            
            //Enable the correct menus.
            inGameMenu.SetActive(false);
            pauseMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        private void NewGame()
        {
            mainMenu.SetActive(false);
            pauseMenu.SetActive(false);
            inGameMenu.SetActive(true);
            
            _score = 0;
            scoreText.text = "Score: " + _score;
            StartCoroutine(Countdown());
        }
        
        private void UpdateScore(Enemy enemy)
        {
            //Add score.
            _score += enemy.score;
            
            //Update score text.
            scoreText.text = "Score: " + _score;

            //Remove event from enemy.
            //ReSharper disable once DelegateSubtraction
            enemy.OnEnemyKilled -= UpdateScore;
            
            //Destroy enemy
            Destroy(enemy.gameObject);
            
            //Spawn new enemy.
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            var position = playerCameraTransform.position;
            var forward = playerCameraTransform.forward * Random.Range(1, 2);
            
            var enemy = Instantiate
            (
                enemyPrefab,
                new Vector3
                (
                    position.x + forward.x + Random.Range(-1,1),
                    position.y + forward.y + Random.Range(-.5f, 5f),
                    position.z + forward.z
                ), 
                Quaternion.identity,
                enemyContainer
            );
            
            enemy.OnEnemyKilled += UpdateScore;
            enemy.Initialize();
            enemy.meshRenderer.material = enemyMaterials[Random.Range(0, enemyMaterials.Length)];
        }

        private IEnumerator Countdown()
        {
            countDownText.enabled = true;
            countDownText.text = "Game starts in";
            
            yield return new WaitForSeconds(.5f);
            
            for (var i = 3; i >= 0; i--)
            {
                countDownText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            countDownText.enabled = false;
            
            //Spawn enemies.
            for (var i = 0; i < 10; i++)
            {
                SpawnEnemy();
            }
        }
    }
}