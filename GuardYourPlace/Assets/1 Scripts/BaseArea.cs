using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseArea : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] int maxHealth;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = 1;
    }
    public void healthSet(int miktar)
    {
        Debug.Log("slider");
        int healthOld = currentHealth;
        currentHealth = currentHealth + miktar;
            LeanTween.value(healthOld, currentHealth, 0.2f).setOnUpdate((float val) =>
            {
                healthBar.value = val / (float)maxHealth;
                if (currentHealth<= 0)
                {
                    currentHealth = 0;
                    healthBar.value = (float)currentHealth / (float)maxHealth;
                }
             
            });
        if(currentHealth <= 0)
        {
            if (Globals.evoLevel > 1)
            {
                upgradeManager.Instance.levelDown();
            }
            else
            {
                GameManager.Instance.Notify_LoseObservers();
            }
        }
    }
    void Update()
    {
        
    }
}
