using UnityEngine;

public class LifeAction : MonoBehaviour
{
    public TG_PlayerData playerData;
    [SerializeField] private EffectEventSO effectEvent;
    public float lifeTime = 10f;
    private bool collected = false;

    void Start()
    {
        Invoke(nameof(CheckAndDestroy), lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            SoundManager.Instance.PlaySFX(ESfx.Item);
            effectEvent.Raise(EEffect.PowerUp, other.gameObject.GetComponent<Player>().transform.position);
            playerData.AddLife(1);

            Destroy(gameObject);
        }   
    }

    void CheckAndDestroy()
    {
        if (!collected)
        {
            Debug.Log("life destroy");
            Destroy(gameObject);
        }
    }

}
