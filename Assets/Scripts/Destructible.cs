using Mirror;
using UnityEngine;

public class Destructible : NetworkBehaviour
{
    [SerializeField] private int m_MaxhitPoint;

    [SerializeField] private GameObject m_DestroySfx;

    public int HitPoint => currentHitPoint;
    private int currentHitPoint;

    [SyncVar]
    private int syncCurrentHitPoint;

    public override void OnStartServer()
    {
        base.OnStartServer();

        syncCurrentHitPoint = m_MaxhitPoint;
        currentHitPoint = m_MaxhitPoint;
    }
    [Server]
    public void SvApplyDamage(int damage)
    {
        syncCurrentHitPoint -= damage;

        if (syncCurrentHitPoint <= 0)
        {
            if (m_DestroySfx != null)
            {
               GameObject sfx =  Instantiate(m_DestroySfx, transform.position, Quaternion.identity);

               NetworkServer.Spawn(sfx);
            }

            Destroy(gameObject);
        }
    }
    private void ChangeHitPoint(int oldValue, int newValue)
    {
        currentHitPoint = newValue;
    }
}
