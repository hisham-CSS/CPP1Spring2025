using UnityEngine;

public class SpawnPrefabOnExit : StateMachineBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn when the state exits

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (prefabToSpawn != null)
        {
            // Get the Collider2D component from the animator's GameObject
            Collider2D collider = animator.gameObject.GetComponent<Collider2D>();
            Vector2 groundPos = new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y + 0.15f);
             // Instantiate the prefab at the specified position and rotation
            GameObject spawnedObject = Instantiate(prefabToSpawn, groundPos, Quaternion.identity);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
