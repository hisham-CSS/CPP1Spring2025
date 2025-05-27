using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;

    private Transform playerRef;

    void Awake()
    {
        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;
    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        playerRef = playerInstance.transform;
        return playerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerRef) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(playerRef.position.x, minXPos, maxXPos);
        transform.position = pos;
    }
}
