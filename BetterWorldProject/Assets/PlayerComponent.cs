using UnityEngine;

namespace Assets
{
    public class PlayerComponent : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_RotationSpeed;
        [SerializeField] private GameObject m_SolarPanel;

        private Color m_DefaultColor;
        private SpriteRenderer m_Renderer;



        [SerializeField] private float m_BaseEnergyAdded = 10.0f;
        public GameObject Sun;

        private Rigidbody2D m_RB;
        private int Mask;

        private void Awake()
        {
            m_RB = GetComponent<Rigidbody2D>();
            m_Renderer = m_SolarPanel.GetComponent<SpriteRenderer>();
            m_DefaultColor = m_Renderer.color;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Mask = LayerMask.GetMask("Sun", "Player", "Bounds", "SolarPanel");
            Mask = LayerMask.GetMask("Cloud");
        }

        // Update is called once per frame
        void Update()
        {
            float l_DeltaTime = Time.deltaTime;
            Vector2 l_NewPosition = new Vector2(Input.GetAxis("Horizontal") * m_MoveSpeed * l_DeltaTime, 0.0f);
            Quaternion l_NewRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 45.0f * Input.GetAxis("Vertical")),
                m_RotationSpeed * l_DeltaTime);


            Vector2 l_DirectionSun = (Sun.transform.position - transform.position).normalized;
            float l_EnergyRatio = Vector2.Dot(l_DirectionSun, m_SolarPanel.transform.up);
            float l_EnergyGained = (l_EnergyRatio * l_EnergyRatio * l_EnergyRatio) * m_BaseEnergyAdded;// Cube to increase drop off

            // Raycast for if blocked
            RaycastHit2D l_Cast = Physics2D.Linecast(transform.position, Sun.transform.position, Mask);
            if (l_Cast.collider != null)
                l_EnergyGained = 0.0f;


            Color l_NewColor = Color.Lerp(m_DefaultColor, Color.red, l_EnergyGained / m_BaseEnergyAdded);
            m_Renderer.color = new Color(l_NewColor.r, l_NewColor.g, l_NewColor.b, 1.0f);


            GameManager.Instance.IncreaseEnergy(l_EnergyGained);

            Debug.Log(l_EnergyRatio + " " + l_EnergyGained);
            Debug.DrawRay(transform.position, m_SolarPanel.transform.up * 200.0f, Color.red);


            //Debug.Log(transform.rotation + " " + l_NewRotation);p


            transform.position = ((Vector2)transform.position + l_NewPosition);
            m_SolarPanel.transform.rotation = l_NewRotation;



        }
    }
}
