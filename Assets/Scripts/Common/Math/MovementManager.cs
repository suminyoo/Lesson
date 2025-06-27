using UnityEngine;
public interface IMovement
{
    void Move();
    void Stop();
}
public class MovementManager : MonoBehaviour
{
    private IMovement currentMovement;
    public MonoBehaviour[] movementScripts;

    private bool isStopped = false;

    private void Start()
    {
        SetMovement(0); // 기본
    }

    private void Update()
    {
        // 전환 키
        if (Input.GetKeyDown(KeyCode.P)) SetMovement(0); // Cosine
        if (Input.GetKeyDown(KeyCode.O)) SetMovement(1); // Sine
        if (Input.GetKeyDown(KeyCode.I)) SetMovement(2); // Tangent

        // 정지/시작 키
        if (Input.GetKeyDown(KeyCode.L)) isStopped = true;  // 정지
        if (Input.GetKeyDown(KeyCode.M)) isStopped = false; // 시작

        if (!isStopped)
            currentMovement?.Move();
    }

    private void SetMovement(int index)
    {
        if (index < 0 || index >= movementScripts.Length)
        {
            Debug.LogWarning("잘못된 인덱스");
            return;
        }

        for (int i = 0; i < movementScripts.Length; i++)
        {
            movementScripts[i].enabled = (i == index);
        }

        currentMovement = movementScripts[index] as IMovement;

        if (currentMovement == null)
        {
            Debug.LogWarning($"movementScripts[{index}]는 IMovement를 구현하지 않았습니다.");
        }
    }
}
