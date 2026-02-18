using System;
using UnityEngine;

public class DoorScaler : MonoBehaviour
{
    [SerializeField] DoorDir dir;
    [SerializeField] float scaleConst = 40f;
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        Vector3 playerPosition = PlayerManager.Instance.PlayerPosition;
        switch (dir)
        {
            // case DoorDir.Up:
            //     transform.localScale = new Vector3(originalScale.x, originalScale.y * (playerPosition.y - transform.position.y) * scaleConst * 0.01f, 1f);
            //     break;
            // case DoorDir.Down:
            //     transform.localScale = new Vector3(-originalScale.x, originalScale.y * (playerPosition.y - transform.position.y) * scaleConst * 0.01f, 1f);
            //     break;
            case DoorDir.Up:
                transform.localScale = new Vector3(-originalScale.x * (playerPosition.y - transform.position.y) * scaleConst * 0.01f, originalScale.y, 1f);
                break;
            case DoorDir.Down:
                transform.localScale = new Vector3(originalScale.x * (playerPosition.y - transform.position.y) * scaleConst * 0.01f, originalScale.y, 1f);
                break;
            case DoorDir.Left:
                transform.localScale = new Vector3(originalScale.x * (playerPosition.x - transform.position.x) * scaleConst * 0.01f, originalScale.y, 1f);
                break;
            case DoorDir.Right:
                transform.localScale = new Vector3(-originalScale.x * (playerPosition.x - transform.position.x) * scaleConst * 0.01f, originalScale.y, 1f);
                break;
        }
    }
}

[Serializable]
public enum DoorDir
{
    Up,
    Down,
    Left,
    Right
}
