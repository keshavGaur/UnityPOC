﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This target can receive damage.
/// </summary>
public class DamageTaker : MonoBehaviour
{
    // Start hitpoints
    public int hitpoints;
    // Remaining hitpoints
    [HideInInspector]
    public int currentHitpoints;
    // Damage visual effect duration
    public float damageDisplayTime = 0.2f;
    // Helth bar object
    public Transform healthBar;
    // SendMessage will trigger on damage taken
    public bool isTrigger;

    // Image of this object
    private SpriteRenderer sprite;
    // Visualisation of hit or heal is in progress
    private bool coroutineInProgress;
    // Original width of health bar (full hp)
    private float originHealthBarWidth;
    static int x = 0;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        currentHitpoints = hitpoints;
        sprite = GetComponentInChildren<SpriteRenderer>();
        Debug.Assert(sprite && healthBar, "Wrong initial parameters");
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        originHealthBarWidth = healthBar.localScale.x;
        currentHitpoints = hitpoints;
    }

    /// <summary>
    /// Take damage.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void TakeDamage(int damage)
    {
       // Debug.Log("WENT to damage taker hitpoints are" + currentHitpoints + "   &   damge"+ damage);
        if (damage > 0)
        {
            if (this.enabled == true)
            {
              //  Debug.Log("this IS ENABLED IN DAMAGE tAKER");
                if (currentHitpoints > damage)
                {
                  //  Debug.Log("STILL ALIVE");
                    // Still alive
                    currentHitpoints -= damage;
                    UpdateHealthBar();
                    // If no coroutine now
                    if (coroutineInProgress == false)
                    {
                       // Debug.Log("Nocoroutine in progress");
                        // Damage visualisation
                        StartCoroutine(DisplayDamage());
                    }
                    if (isTrigger == true)
                    {
                        // Notify other components of this game object
                     //   Debug.Log("isTrigger IS TRUE");
                        SendMessage("OnDamage");
                    }
                }
                else
                {
                    // Die
                    currentHitpoints = 0;
                    UpdateHealthBar();
                    Die();
                }
            }
        }
        else // damage < 0
        {
            // Healed
            currentHitpoints = Mathf.Min(currentHitpoints - damage, hitpoints);
            UpdateHealthBar();
        }
    }

    /// <summary>
    /// Updates the health bar width.
    /// </summary>
    private void UpdateHealthBar()
    {
     //   Debug.Log("IN UpdateHealthBar");
        float healthBarWidth = originHealthBarWidth * currentHitpoints / hitpoints;
       // Debug.Log("healthBarWidth IS:  " + healthBarWidth);
        healthBar.localScale = new Vector2(healthBarWidth, healthBar.localScale.y);
    }

    /// <summary>
    /// Die this instance.
    /// </summary>
    public void Die()
    {
        EventManager.TriggerEvent("UnitKilled", gameObject, null);
        Destroy(gameObject);
    }

    /// <summary>
    /// Damage visualisation.
    /// </summary>
    /// <returns>The damage.</returns>
    IEnumerator DisplayDamage()
    {
        coroutineInProgress = true;
        Color originColor = sprite.color;
        float counter;
        // Set color to black and return to origin color over time
        for (counter = 0f; counter < damageDisplayTime; counter += Time.fixedDeltaTime)
        {
            sprite.color = Color.Lerp(originColor, Color.black, Mathf.PingPong(counter, damageDisplayTime));
            yield return new WaitForFixedUpdate();
        }
        sprite.color = originColor;
        coroutineInProgress = false;
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
        EventManager.TriggerEvent("UnitDie", gameObject, null);
        StopAllCoroutines();
    }
}