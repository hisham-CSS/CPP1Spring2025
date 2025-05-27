using UnityEngine;

public class LevelStart : MonoBehaviour
{
    public Transform startPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => GameManager.Instance.InstantatePlayer(startPoint.position);
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
