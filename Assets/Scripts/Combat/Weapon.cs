using System;
using Music.Instrument;
using UnityEngine;
using UnityEngine.Pool;
using Plum.VFX;

namespace Music.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Music.Player.PlayerMovement movement;
        [SerializeField] private Projectile m_projectilePrefab;
        private ObjectPool<Projectile> _projectilePool;
        [SerializeField] private bool m_projectilePoolCollectionCheck = false;
        [SerializeField] private int m_defaultProjectilePoolSize = 30;
        [SerializeField] private int m_maxProjectilePoolSize = 1000;
        [SerializeField] private ProjectileInitSettings settings;
        [SerializeField] private Transform scaleFX;
        private Vector3 initialScale;
        
        private bool _shot = false;
        
        private void Start()
        {
            initialScale = scaleFX.localScale;
            settings.ReleaseAction = ReleaseProjectile;
            
            _projectilePool = new ObjectPool<Projectile>(
                () =>
                {
                    Projectile projectile = Instantiate(m_projectilePrefab);
                    projectile.Initialize(settings);
                    return projectile;
                },
                projectile =>
                {
                    projectile.Initialize(settings);
                }, 
                projectile =>
                {
                    projectile.Release();
                }, 
                projectile =>
                {
                    Destroy(projectile.gameObject);
                }, 
                m_projectilePoolCollectionCheck, 
                m_defaultProjectilePoolSize, 
                m_maxProjectilePoolSize
            );
            
            Metronome.SubscribeOnMethod(InstrumentType.Kick, OnNote);
        }

        private void Update()
        {
            scaleFX.localScale = Vector3.Lerp(scaleFX.localScale, initialScale, 5 * Time.deltaTime);
            _shot = Input.GetKey(KeyCode.Mouse0);
            if(_shot)
            {
                //SpawnProjectile();
            }
        }

        private void OnNote(float input)
        {
            if(_shot)
            {
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            scaleFX.localScale = new Vector3(initialScale.x * 2, initialScale.y * .5f, initialScale.z * 2);
            Music.Player.MainCam.RequestShake(1.0f, 1.0f);
            Projectile projectile = _projectilePool.Get();
            movement.AddForce(-settings.SpawnTransform.forward);
        }

        private void ReleaseProjectile(Projectile projectile)
        {
            _projectilePool.Release(projectile);
        }
    }
}