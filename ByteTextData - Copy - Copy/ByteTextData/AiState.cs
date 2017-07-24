using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Basic class for AI state.
/// </summary>
public class AiState : MonoBehaviour
{
    // Allowed triiger types for AI state transactions
    public enum Trigger
    {
        TriggerEnter,   // On collider enter
        TriggerStay,    // On collider stay
        TriggerExit,    // On collider exit
        Damage,         // On damage taken
        Cooldown        // On some cooldown expired
    }

    [Serializable]
    // Allows to specify AI state change on any trigger
    public class AiTransaction
    {
        public Trigger trigger;
        public AiState newState;
    }
    // List with specified transactions for this AI state
    public AiTransaction[] specificTransactions;

    // Animation controller for this AI
    protected Animator anim;
    // AI behavior of this object
    protected AiBehavior aiBehavior;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    public virtual void Awake()
    {
        StartCoroutine("waitfordownload");
        aiBehavior = GetComponent<AiBehavior>();
        //if (aiBehavior == null) Debug.Log("aiBehavior is null");
        //else Debug.Log("aiBehavior is not null");
        anim = GetComponentInParent<Animator>();
        //if (anim == null) Debug.Log("anim is null");
       // else Debug.Log("anim is not null");
        Debug.Assert(aiBehavior, "Wrong initial parameters");
    }

    IEnumerator waitfordownload()
    {
        yield return new WaitUntil(() => AssetBundleScene.myHashtable.Count != 0);
    }
    /// <summary>
    /// Raises the state enter event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
    public virtual void OnStateEnter(AiState previousState, AiState newState)
    {

    }

    /// <summary>
    /// Raises the state exit event.
    /// </summary>
    /// <param name="previousState">Previous state.</param>
    /// <param name="newState">New state.</param>
    public virtual void OnStateExit(AiState previousState, AiState newState)
    {

    }

    /// <summary>
    /// Raises the trigger event.
    /// </summary>
    /// <param name="trigger">Trigger.</param>
    /// <param name="my">My.</param>
    /// <param name="other">Other.</param>
    public virtual bool OnTrigger(Trigger trigger, Collider2D my, Collider2D other)
    {
       // Debug.Log("IN ONTRIGGER OF AISTATE");
       // Debug.Log(specificTransactions + ": specificTransactions IMPPORTANT");

        bool res = false;
        specificTransactions = GetComponentInParent<AiStateIdle>().specificTransactions;
        foreach (AiTransaction transaction in specificTransactions)
        {
            // Debug.Log(specificTransactions);
            // Debug.Log(transaction);
           // Debug.Log(transaction.trigger + ":transaction.trigger");
           // Debug.Log(trigger+ ":trigger ");
            if (trigger == transaction.trigger)
            {
                aiBehavior.ChangeState(transaction.newState);
                res = true;
                break;
            }
        }
        return false;
    }
}
