using UnityEngine;

namespace Assets
{
    public class CloudComponent : MonoBehaviour
    {
        public float CloudSpeed = 10.0f;
        public float MaxDistance = 300.0f;
        private float m_CurrentDistance = 0.0f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float Movement = CloudSpeed * Time.deltaTime;
            transform.position += (Vector3)new Vector2(Movement, 0.0f);
            m_CurrentDistance += Movement;




            if (m_CurrentDistance >= MaxDistance)
                Destroy(this.gameObject);
        }


        public void SetDirection(bool i_Right)
        {
            if (i_Right)
            {

            }
            else
            {
                CloudSpeed *= -1.0f;
            }
        }
    }
}
