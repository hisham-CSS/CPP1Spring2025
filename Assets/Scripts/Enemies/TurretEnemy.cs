using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class TurretEnemy : Enemy
{
    [SerializeField] private float projectileFireRate = 2.0f;
    private float timeSinceLastFire = 0;
    private PlayerController playerRef;

    public float distThreshold = 2.0f;

    private void Awake() => GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;
    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        playerRef = playerInstance;
        return playerInstance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (projectileFireRate <= 0)
            projectileFireRate = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0) return;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        sr.flipX = (playerRef.transform.position.x < transform.position.x);
        float distance = Mathf.Abs(playerRef.transform.position.x - transform.position.x); //check distance just on the x

        if (distance < distThreshold)
        {
            sr.color = Color.red; // Change color to indicate distance threshold is met
            CheckFire(stateInfo);
        }
        else
        {
            sr.color = Color.white; // Reset color if not in distance threshold
        }
    }

    void CheckFire(AnimatorStateInfo info)
    {
        if (!info.IsName("Idle")) return;

        if (Time.time >= timeSinceLastFire + projectileFireRate)
        {
            anim.SetTrigger("Fire");
            timeSinceLastFire = Time.time;
        }
    }
}
