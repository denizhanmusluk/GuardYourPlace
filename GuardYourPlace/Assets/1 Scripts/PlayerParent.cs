using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
public class PlayerParent : MonoBehaviour, ILoseObserver
{
    [SerializeField] TextMeshProUGUI levelText, levelYearText;
    [SerializeField] Slider levelBar;
    [SerializeField] public List<GameObject> humans;
    public int currentYear;
    [SerializeField] TextMeshProUGUI yearText;
    [SerializeField] public GameObject yearCanvas;
    //public NavMeshAgent agent;
    //public int level;
    public int evoLevel;
    PlayerControl playerControl;
    int currentPlayerCount;
    public int[] levelYears;

    [SerializeField] GameObject yearUpParticle;
    bool yearParticleActive = false;
    private void Start()
    {
        GameManager.Instance.Add_LoseObserver(this);

        playerControl = GetComponent<PlayerControl>();

        evoLevel = Globals.evoLevel;
        currentYear = 1;
        Globals.currentYear = 0;
        yearText.text = currentYear.ToString();
        //agent = GetComponent<NavMeshAgent>();
        //agent.enabled = false;
        levelText.text = (Globals.waveLevel + 1).ToString();
        levelYearText.text = currentYear.ToString("N0") + "/" +ZombiSpawner.Instance.zombieLevel[Globals.waveLevel].ToString();
        levelBar.value = 0f;
    }
    public void LoseScenario()
    {
        for (int i = 0; i < playerControl.players.Count; i++)
        {
            playerControl.players[i].GetComponent<PlayerEvolution>().damageDead();
        }
    }
    public void throughlyScale()
    {
    }
    IEnumerator scaleCalling()
    {
        int humanCount = humans.Count;
        for (int i = 0; i < humanCount - 1; i++)
        {
            StartCoroutine(throughlyScaling(humans[humanCount - 1 - i].transform));
            yield return new WaitForSeconds(0.05f);
        }
        yearParticleActive = false;
    }
    IEnumerator throughlyScaling(Transform hmn)
    {
        //////
        if (yearParticleActive)
        {
            GameObject yearUp = Instantiate(yearUpParticle, hmn.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
            yearUp.transform.localScale = new Vector3(7, 7, 7);
        }
        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 3f;
            hmn.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        hmn.localScale = new Vector3(firstSize, firstSize, firstSize);

    }
    //public void UItargetSelect()
    // {
    //     if (humans.Count > 1)
    //     {
    //         direction.selectTarget(humans[humans.Count - 1].GetComponent<Employee>().jobId, transform);
    //     }
    //     else
    //     {
    //         direction.selectTarget(0, transform);
    //         direction.arrowScaleSet();
    //     }
    // }

    public void playerYearSet(int year)
    {
        
        currentYear += year;

        if (currentYear < 0)
        {
            currentYear = 0;
            YearUpdate(-Globals.currentYear);
        }
        else
        {
            YearUpdate(year);
        }
        //yearText.text = currentYear.ToString();
        //Globals.currentYear = currentYear;
        //if (currentYear > 40)
        //{

        //}
        
        if (currentYear> ZombiSpawner.Instance.zombieLevel[Globals.waveLevel] && ZombiSpawner.Instance.zombieLevel[ZombiSpawner.Instance.zombieLevel.Length - 1] > currentYear)
        {
            upgradeManager.Instance.upgradeOpen();
            ZombiSpawner.Instance.spawnActive = false;
            yearParticleActive = true;
            Globals.waveLevel++;
            //evolutionSet();
        }
        if (Globals.waveLevel > 0)
        {
            if (currentYear < ZombiSpawner.Instance.zombieLevel[Globals.waveLevel - 1])
            {
                Globals.waveLevel--;
                //evolutionSet();
            }
        }
        
    }
    public void YearUpdate(int miktar)
    {
        levelText.text = (Globals.waveLevel + 1).ToString();
        int yearOld = Globals.currentYear;
        Globals.currentYear = Globals.currentYear + miktar;
        LeanTween.value(yearOld, Globals.currentYear, 0.2f).setOnUpdate((float val) =>
        {
            //yearText.text = val.ToString("N0");
            levelYearText.text = val.ToString("N0") + "/" + ZombiSpawner.Instance.zombieLevel[Globals.waveLevel].ToString();
            //levelBar.fillAmount = (val) / levelYears[level];
            if (Globals.waveLevel > 0)
            {
                //levelYearText.text = (val - levelYears[level - 1]).ToString("N0") + "/" + levelYears[level].ToString();
                levelBar.value = (val - ZombiSpawner.Instance.zombieLevel[Globals.waveLevel - 1]) / (ZombiSpawner.Instance.zombieLevel[Globals.waveLevel] - ZombiSpawner.Instance.zombieLevel[Globals.waveLevel - 1]);
            }
            else
            {
                //levelYearText.text = val.ToString("N0") + "/" + levelYears[level].ToString();
                levelBar.value = (val) / ZombiSpawner.Instance.zombieLevel[Globals.waveLevel];
            }
        });//.setOnComplete(() =>{});
        //PlayerPrefs.SetInt("money", Globals.moneyAmount);

    }
   public void evolutionSet()
    {
        yearParticleActive = true;
        //evoLevel++;
        currentPlayerCount = playerControl.players.Count;
        yearText.text = (Globals.evoLevel).ToString();

        StartCoroutine(evolutionSett());
    }
    IEnumerator evolutionSett()
    {
  
        for (int i = 0; i < playerControl.players.Count; i++)
        {
            GameObject player = Instantiate(humans[Globals.evoLevel - 1], playerControl.players[i].transform.position, playerControl.players[i].transform.rotation, this.transform);
            player.GetComponent<PlayerEvolution>().maxHelath = GetComponent<PlayerControl>().currentHealth;
            Destroy(playerControl.players[i]);
            playerControl.players[i] = player;
            StartCoroutine(throughlyScaling(player.transform));
            targetSelectManager.Instance.Notify_ChangeObservers();

            yield return new WaitForSeconds(0.1f);
            upgradeManager.Instance.setPosPlayers();
        }
    }
}