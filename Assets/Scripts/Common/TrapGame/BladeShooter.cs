using UnityEngine;

public class BladeShooter : Trap
{
    public GameObject bladePrefab;
    public Transform firePoint;
    public float fireInterval = 5f;

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
        blade.transform.Rotate(0f, 0f, 90f);  // Z축 90도 회전 (필요한 축 맞춰서)
        blade.GetComponent<Blade>().Launch();
    }

}