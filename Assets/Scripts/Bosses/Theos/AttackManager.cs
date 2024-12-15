using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager primaryInstace; // Prevents need for assigned memory addresses. Assigned on awake.
    public float minBuffer = .25f; // Time between an attack becoming inactive and adding a new attack
    public float marginalBuffer = 1; // The added buffer for number of active attacks.
    public int attackCount = 1; // Number of active attacks allowed
    List<GameObject> active = new();
    Queue<GameObject> inactive = new();
    private int attacksThisPhase = 0; // Number of attacks completed this phase
    public int phase = 0; // 4 phases, 0 indexed
    public int[] attackRequirements = {10, 15, 20, 30};
    public GameObject[] portals;
    public GameObject[] rings;
    public CrossFader crossFader;

    void Awake()
    {
        primaryInstace = this;
    }

    void OnEnable()
    {
        // Resets active and continues inactive queue
        foreach(GameObject atk in active)
        {
            atk.SetActive(false);
            inactive.Enqueue(atk);
        }
        for(int i = 0; i < transform.childCount; i++)
        {
            inactive.Enqueue(transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < attackCount; i++)
        {
            Activate(inactive.Dequeue());
        }
    }

    void OnDisable()
    {
        foreach(GameObject atk in active)
        {
            atk.SetActive(false);
            inactive.Enqueue(atk);
        }
    }

    void Activate(GameObject attack)
    {
        StartCoroutine(Buffer());
        IEnumerator Buffer()
        {
            yield return new WaitForSeconds(minBuffer + marginalBuffer * active.Count);
            attack.SetActive(true);
            active.Add(attack);
        }
    }

    public void NextPhase()
    {
        rings[phase].SetActive(false); // Lose a ring
        phase++;
        attackCount++;
        attacksThisPhase = 0;
        crossFader.NextTrack();
    }


    // Update is called once per frame
    void Update()
    {
        foreach(GameObject attack in active)
        {
            if(!attack.activeInHierarchy)
            {
                active.Remove(attack);
                attacksThisPhase++;
                if(attacksThisPhase >= attackRequirements[phase])
                {
                    portals[phase].SetActive(true);
                }
                inactive.Enqueue(attack);
                Activate(inactive.Dequeue());
                break; // Prevents errors from collection change
            }
        }
    }
}
