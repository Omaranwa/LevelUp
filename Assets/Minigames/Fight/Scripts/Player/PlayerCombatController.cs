using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private float _projectileSpread = 0.15f;
    private float _shotTimer = 0;
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (GameManager.GameStateManager.IsDead)
        {
            return;
        }
        
        _shotTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && _shotTimer > GameManager.UpgradeManager.weaponSettings.FireRate)
        {
            _shotTimer = 0;
            Shoot();
        }
    }
    
    private void Shoot()
    {
        int projectileCount = GameManager.UpgradeManager.weaponSettings.ProjectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject projectileGO = Instantiate(projectilePrefab);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.SetOwner(Projectile.OwnerType.Player);
            projectile.SetDamage(GameManager.UpgradeManager.weaponSettings.Damage);
            
            
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
            float indexOffset = (float)i - i/2;
            Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * _projectileSpread;
            
            projectileGO.transform.position = transform.position.AsVector2() + offset;

            
            projectile.SetDirection(direction);
        }
        
    }
}
