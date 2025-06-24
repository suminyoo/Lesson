using UnityEngine;

public class CoinAction : MonoBehaviour
{
    public void CoinMiddle()
    {
        Debug.Log("In coin middle");
        SoundManager.Instance.PlaySFX(ESfx.Item);

    }
    public void CoinEnd()
    {
        Debug.Log("In coin end");

    }
}
