using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChefNavigation : MonoBehaviour {
    NavMeshAgent agent;
    ChefBase ThisChef;
    KitchenManager kitchenManager;

    public Transform CurrentTarget;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        ThisChef = GetComponent<ChefBase>();
        kitchenManager = GameObject.FindObjectOfType<KitchenManager>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            GoToWorkstation();
        }
    }

    public void GoToWorkstation() {
        GoToTarget(ThisChef.CurrentWorkstation.StandingPoint);
    }

    public void GoToTarget(Transform target) {
        CurrentTarget = target;
        agent.SetDestination(CurrentTarget.position);
    }


}
