using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_LifeTime;
    [SerializeField] private GameObject m_DestroySfx;

    private Transform m_Parent;

    private void Start()
    {
        Destroy(gameObject, m_LifeTime);
    }

    private void Update()
    {
        float stepLength = Time.deltaTime * m_MovementSpeed;
        Vector2 step = transform.up * stepLength;

        transform.position += new Vector3(step.x, step.y, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Time.deltaTime * m_MovementSpeed);

        if (hit == true)
        {
            if (hit.collider.transform.root != m_Parent)
            {
                if (NetworkSessionManager.Instance.IsServer == true)
                {

                    Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

                    if (destructible != null)
                    {
                        destructible.SvApplyDamage(m_Damage);
                    }

                }
                if (NetworkSessionManager.Instance.IsClient == true)
                {
                    Instantiate(m_DestroySfx, transform.position, Quaternion.identity);                    
                }


                Destroy(gameObject);
            }
        }
    }

    public void SetParent(Transform parent)
    {
        m_Parent = parent;
    }
}
