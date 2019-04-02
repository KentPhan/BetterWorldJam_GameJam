using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets
{
    public enum GameStates
    {
        START,
        PLAY,
        GAMEOVER
    }

    public class GameManager : MonoBehaviour
    {
        // Energy
        [SerializeField] private float m_EnergyDecreaseRate = 10.0f;
        [SerializeField] private float m_InitialEnergy = 100.0f;
        [SerializeField] private float m_MaxEnergy = 100.0f;
        [SerializeField] private TextMeshProUGUI m_EnergyText;

        // Clouds
        [SerializeField] private GameObject m_CloudPrefab;
        [SerializeField] private GameObject m_CloudSpawnLeft;
        [SerializeField] private GameObject m_CloudSpawnRight;

        [SerializeField] private float m_CloudSpawnRate = 10.0f;

        [SerializeField] private float m_CloudSpawnRateModifier = 0.9f;
        [SerializeField] private float m_CloudSpawnRateIncreaseRate = 10.0f;
        [SerializeField] private float m_CloudSpawnRateMin = 3.0f;

        private float m_CurrentCloudSpawnTimer = 3.0f;
        private float m_CurrentCloudSpawnRateIncreaseTimer = 10.0f;


        // UI
        [SerializeField] private RectTransform m_Start;
        [SerializeField] private RectTransform m_Play;
        [SerializeField] private RectTransform m_GameOver;









        private float m_CurrentEnergy;

        private GameStates m_CurrentState;

        public static GameManager Instance;

        public void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_CurrentEnergy = m_InitialEnergy;
            m_CurrentState = GameStates.START;

            m_Start.gameObject.SetActive(true);
            m_Play.gameObject.SetActive(false);
            m_GameOver.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }



            switch (m_CurrentState)
            {
                case GameStates.START:
                    if (Input.GetKey(KeyCode.Return))
                    {
                        m_CurrentState = GameStates.PLAY;

                        m_Start.gameObject.SetActive(false);
                        m_Play.gameObject.SetActive(true);
                        m_GameOver.gameObject.SetActive(false);
                    }
                    break;
                case GameStates.PLAY:
                    m_CurrentEnergy -= Time.deltaTime * m_EnergyDecreaseRate;
                    m_EnergyText.text = ((int)m_CurrentEnergy).ToString();
                    if (m_CurrentEnergy <= 0)
                    {
                        m_CurrentState = GameStates.GAMEOVER;

                        m_Start.gameObject.SetActive(false);
                        m_Play.gameObject.SetActive(false);
                        m_GameOver.gameObject.SetActive(true);
                    }



                    if (m_CurrentCloudSpawnTimer <= 0.0f)
                    {
                        // Cloud Shit
                        float l_Rand = Random.Range(-1.0f, 1.0f);
                        if (l_Rand <= 0)
                        {
                            Instantiate(m_CloudPrefab, m_CloudSpawnLeft.transform).GetComponent<CloudComponent>();
                        }
                        else
                        {
                            CloudComponent l_Cloud = Instantiate(m_CloudPrefab, m_CloudSpawnRight.transform).GetComponent<CloudComponent>();
                            l_Cloud.SetDirection(false);
                        }

                        m_CurrentCloudSpawnTimer = m_CloudSpawnRate;
                    }


                    if (m_CurrentCloudSpawnRateIncreaseTimer <= 0.0f)
                    {
                        m_CloudSpawnRate = Mathf.Clamp(m_CloudSpawnRate * m_CloudSpawnRateModifier, m_CloudSpawnRateMin, 100.0f);
                        m_CurrentCloudSpawnRateIncreaseTimer = m_CloudSpawnRateIncreaseRate;
                    }


                    m_CurrentCloudSpawnTimer -= Time.deltaTime;
                    m_CurrentCloudSpawnRateIncreaseTimer -= Time.deltaTime;

                    break;
                case GameStates.GAMEOVER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public void IncreaseEnergy(float i_EnergyObtained)
        {
            float l_NewEnergyNumber = Mathf.Clamp(m_CurrentEnergy + (i_EnergyObtained * Time.deltaTime), 0.0f, m_MaxEnergy);

            m_CurrentEnergy = l_NewEnergyNumber;
            m_EnergyText.text = ((int)m_CurrentEnergy).ToString();
        }
    }




}


