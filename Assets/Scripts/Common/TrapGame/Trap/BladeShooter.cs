using UnityEngine;

public class BladeShooter : Trap
{
    public GameObject bladePrefab;
    public Transform firePoint;

    [Header("Blade Settings")]
    public float fireInterval = 5f;
    public float bladeSpeed = 7f;
    public float bladeLifetime = 10f;
    public int bladeDamage = 20;

    private float timer;

    void Awake()
    {
        fireInterval = Random.Range(2f, 6f);
        bladeSpeed = Random.Range(5f, 12f);
        bladeLifetime = Random.Range(5f, 10f); 
        bladeDamage = Random.Range(10, 30);
    }

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
        blade.transform.Rotate(0f, 0f, 90f);
        blade.GetComponent<Blade>().Initialize(bladeSpeed, bladeLifetime, bladeDamage);
    }
}