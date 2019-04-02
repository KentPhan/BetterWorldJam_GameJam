using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private float m_EnergyDecreaseRate = 10.0f;
        [SerializeField] private float m_InitialEnergy = 100.0f;

        [SerializeField] private TextMeshProUGUI m_EnergyText;


        [SerializeField] private RectTransform m_Start;
        [SerializeField] private RectTransform m_Play;
        [SerializeField] private RectTransform m_GameOver;



        private float m_CurrentEnergy;

        private GameStates m_CurrentState;


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

                    break;
                case GameStates.GAMEOVER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

