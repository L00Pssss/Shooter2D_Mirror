using Mirror;
using UnityEngine;
public enum WeaponType
{
    Simple,
    Rocket
}
public class Turret : NetworkBehaviour
{
    [SerializeField] 
    private GameObject[] m_Projectile;

    public GameObject[] Projectile => m_Projectile;

    [SerializeField] 
    private float m_FireRate;

    private float currentTime;

    [SerializeField]
    private int m_ammo;

    public int Ammo
    {
        get { return m_ammo; }
        set { m_ammo = value; }
    }
    public bool CanFire => m_ammo > 0;

    private void Update()
    {
        if (isServer)
        {
            currentTime += Time.deltaTime;
        }
    }

    [Command]
    public void CmdRotateTurretTowardsMouse(float angle)
    {
        SVRotateTurretTowardsMouse(angle);
    }

    [SerializeField] private Transform turretVisualMode;

    public Transform TurretVisualMode => turretVisualMode;

    [Server]
    public void SVRotateTurretTowardsMouse(float angle)
    {
        if (turretVisualMode != null)
        {
            turretVisualMode.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            RpcRotateTurretTowardsMouse(angle);
        }
    }

    [ClientRpc]
    public void RpcRotateTurretTowardsMouse(float angle)
    {
        if (isClient)
        {
            turretVisualMode.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    [Command]
    public void CmdFire(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Simple || (weaponType == WeaponType.Rocket && CanFire))
        {
            SvFire(weaponType);
        }
    }



    [Server]
    private void SvFire(WeaponType weaponType)
    {
        if (currentTime < m_FireRate) return;


        if (weaponType == WeaponType.Simple || (weaponType == WeaponType.Rocket && CanFire))
        {
            if (weaponType == WeaponType.Simple)
            {
                FireProjectile(m_Projectile[0]);
                RpsFire(weaponType);
            }
            else if (weaponType == WeaponType.Rocket)
            {
                FireProjectile(m_Projectile[1]);
                RpsFire(weaponType);
            }
           
            currentTime = 0;
     
        }
    }

    private void FireProjectile(GameObject prefab)
    {
        GameObject projectile = Instantiate(prefab, transform.position, transform.rotation);
        projectile.GetComponent<Projectile>().SetParent(transform);
    }

    [ClientRpc]
    private void RpsFire(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Simple)
        {
            GameObject projectile = Instantiate(m_Projectile[0], transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().SetParent(transform);
        }

        if (weaponType == WeaponType.Rocket)
        {
            GameObject projectile = Instantiate(m_Projectile[1], transform.position, transform.rotation);
            projectile.GetComponent<Projectile>().SetParent(transform);
        }
    }

}
