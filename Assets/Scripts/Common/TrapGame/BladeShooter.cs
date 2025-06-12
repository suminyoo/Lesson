using UnityEngine;

public class BladeShooter : Trap
{
    public GameObject bladePrefab;
    public Transform firePoint;
    public float fireInterval = 1f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireInterval)
        {
            FireBlade();
            timer = 0f;
        }
    }

    void FireBlade()
    {
        GameObject blade = Instantiate(bladePrefab, firePoint.position, firePoint.rotation, transform);
        blade.GetComponent<Blade>().Launch();
    }

}