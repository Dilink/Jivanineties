using UnityEngine;
using DG.Tweening;

public class Tokendo : MonoBehaviour
{
    public float movementSpeed = 1.0f;

    public void MoveTo(Transform objective)
    {
        transform.DOMove(objective.position, movementSpeed).SetEase(Ease.InQuint).OnComplete(OnMovementFinished);
    }

    private void OnMovementFinished()
    {
        GameManager.Instance.AddTokendo(1);
    }
}
