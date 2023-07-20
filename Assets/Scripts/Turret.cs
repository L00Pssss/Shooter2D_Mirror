using Mirror;
using UnityEngine;

public class Turret : NetworkBehaviour
{
    [SerializeField] 
    private GameObject m_Projectile;

    [SerializeField] 
    private float m_FireRate;

    private float currentTime;

    private void Update()
    {
        if (isServer)
        {
            currentTime += Time.deltaTime;
        }
    }

    [Command]
    public void CmdFire()
    {
        SvFire();
    }

    [Server]
    private void SvFire()
    {
        if (currentTime < m_FireRate) return;

        GameObject projectile = Instantiate(m_Projectile, transform.position, transform.rotation);
        projectile.GetComponent<Projectile>().SetParent(transform);

        currentTime = 0;

        RpsFire();
    }

    [ClientRpc]
    private void RpsFire()
    {
        GameObject projectile = Instantiate(m_Projectile, transform.position, transform.rotation);
        projectile.GetComponent<Projectile>().SetParent(transform);
    }

}
