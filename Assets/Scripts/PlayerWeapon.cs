using System;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float _delay = 0.25f;
    [SerializeField] BlasterShot _blasterShotPrefab;
    [SerializeField] LayerMask _aimLayerMask;
    [SerializeField] Transform _firePoint;

    float _nextFireTime;
    

    void Update()
    {
        AimTowardMouse();

        if (!ReadyToFire())
            return;
        Fire();

    }

     void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _aimLayerMask ))
        {
            var destination = hitInfo.point;
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.Normalize();
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    bool ReadyToFire() => Time.time >= _nextFireTime;
    

     void Fire()
    {
        _nextFireTime = Time.time + _delay;
        var shot = Instantiate(_blasterShotPrefab, _firePoint.position, transform.rotation);
        shot.Launch(transform.forward);
    }
}
