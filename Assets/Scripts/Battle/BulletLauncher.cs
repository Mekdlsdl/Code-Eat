using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    private IObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet,
            OnGet,
            OnRelease,
            OnDestroyBullet,
            maxSize:5
        );
    }

    public void OnFire()
    {
        Bullet bullet = bulletPool.Get();
        var bulletPosX = new Vector2(transform.position.x + 0.8f, transform.position.y);
        bullet.transform.position = bulletPosX;
        bullet.Fire();
    }
    
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform);
        bullet.SetPool(bulletPool);
        return bullet;
    }

    private void OnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

}
