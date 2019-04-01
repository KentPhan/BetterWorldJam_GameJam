using UnityEngine;

namespace Assets
{
    public class PlayerComponent : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private float m_RotationSpeed;


        private Rigidbody2D m_RB;

        private void Awake()
        {
            m_RB = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float l_DeltaTime = Time.deltaTime;
            Vector2 l_NewPosition = new Vector2(Input.GetAxis("Horizontal") * m_MoveSpeed * l_DeltaTime, 0.0f);
            Quaternion l_NewRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 90.0f * Input.GetAxis("Vertical")),
                m_RotationSpeed * l_DeltaTime);

            Debug.Log(transform.rotation + " " + l_NewRotation);

            m_RB.MovePosition((Vector2)transform.position + l_NewPosition);
            transform.rotation = l_NewRotation;



        }
    }
}
