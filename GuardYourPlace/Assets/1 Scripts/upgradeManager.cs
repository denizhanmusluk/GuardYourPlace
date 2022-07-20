using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class upgradeManager : MonoBehaviour
{
    public static upgradeManager Instance;
    [SerializeField] int[] distanceCenter;
    //count Up
    [SerializeField] int[] countAddCost;

    //Evolve Up
    [SerializeField] int[] evolveAddCost;

    [SerializeField] TextMeshProUGUI evolveAddCostText;
    [SerializeField] TextMeshProUGUI evolveLevelText;

    [SerializeField] TextMeshProUGUI countAddCostText;
    [SerializeField] TextMeshProUGUI countLevelText;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject panelBG;
    [SerializeField] RectTransform fullPanel;
    public GameObject playerRoot;
    PlayerControl _playerControl;
    PlayerParent _playerParent;

    float firstImageScale = 0;
    float lastImageScale = 1f;
    [SerializeField] Button button1, button2, button3;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        _playerControl = playerRoot.GetComponent<PlayerControl>();
        _playerParent = playerRoot.GetComponent<PlayerParent>();
        Globals.playerCount = 1;
        panel.SetActive(false);
        panelBG.SetActive(false);
        //_playerControl = transform.parent.GetComponent<PlayerControl>();

        countAddCostText.text = countAddCost[Globals.playerCount - 1].ToString();
        countLevelText.text = (Globals.playerCount).ToString();

        evolveAddCostText.text = evolveAddCost[Globals.evoLevel - 1].ToString();
        evolveLevelText.text = (Globals.evoLevel).ToString();
        setPosPlayers();
    }
    public void levelUp()
    {
        if (Globals.moneyAmount >= evolveAddCost[Globals.evoLevel - 1])
        {
            GameManager.Instance.MoneyUpdate(-evolveAddCost[Globals.evoLevel - 1]);

            Globals.evoLevel++;
            _playerParent.evolutionSet();
            evolveAddCostText.text = evolveAddCost[Globals.evoLevel - 1].ToString();
            evolveLevelText.text = (Globals.evoLevel).ToString();
            BaseManager.Instance.oldBaseDest();
            //setPosPlayers();
        }
        buttonInteractableSet();
    }
    public void levelDown()
    {
     
            Globals.evoLevel--;
            _playerParent.evolutionSet();
            evolveAddCostText.text = evolveAddCost[Globals.evoLevel - 1].ToString();
            evolveLevelText.text = (Globals.evoLevel).ToString();
            BaseManager.Instance.oldBaseDest();
            //setPosPlayers();
        
    }
    public void duplicate()
    {
        if (Globals.moneyAmount >= countAddCost[Globals.playerCount - 1])
        {
            GameManager.Instance.MoneyUpdate(-countAddCost[Globals.playerCount - 1]);

            Globals.playerCount++;
            StartCoroutine(scaleCalling(1));
            _playerControl.cameraSetUp(Globals.playerCount);
            countAddCostText.text = countAddCost[Globals.playerCount - 1].ToString();
            countLevelText.text = (Globals.playerCount).ToString();
        }
        buttonInteractableSet();
    }
    IEnumerator scaleCalling(int cloneCount)
    {
     
        //GameObject mainPlayer = _playerControl.players[0];
        for (int i = 0; i < cloneCount; i++)
        {
            GameObject clonePlayer = Instantiate(_playerControl.players[0], _playerControl.transform.position, Quaternion.identity, _playerControl.transform);
            //clonePlayer.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
            _playerControl.players.Add(clonePlayer);
            StartCoroutine(throughlyScaling(clonePlayer.transform));

            yield return new WaitForSeconds(0.1f);
        }
        setPosPlayers();

        //_playerControl.players.Remove(mainPlayer);
        //Destroy(mainPlayer);
    }    
   public void setPosPlayers()
    {
        for (int i = 0; i < Globals.playerCount; i++)
        {
            _playerControl.players[i].GetComponent<PlayerEvolution>().playerLocalPos = new Vector3(distanceCenter[Globals.evoLevel - 1] * Mathf.Sin(Mathf.PI * 2 * i / Globals.playerCount), 0, distanceCenter[Globals.evoLevel - 1] * Mathf.Cos(Mathf.PI * 2 * i / Globals.playerCount));
            _playerControl.players[i].GetComponent<PlayerEvolution>().playerPositionMove(360 * i / Globals.playerCount);
        }
    }
    //IEnumerator scaleCalling(int cloneCount)
    //{
    //    for (int i = 1; i < _playerControl.players.Count; i++)
    //    {
    //        GameObject plyr = _playerControl.players[i];
    //        _playerControl.players.Remove(_playerControl.players[i]);
    //        Destroy(plyr);

    //    }
    //    //GameObject mainPlayer = _playerControl.players[0];
    //    for (int i = 1; i < cloneCount; i++)
    //    {
    //        GameObject clonePlayer = Instantiate(_playerControl.players[0], _playerControl.transform.position, Quaternion.identity, _playerControl.transform);
    //        clonePlayer.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
    //        _playerControl.players.Add(clonePlayer);
    //        StartCoroutine(throughlyScaling(clonePlayer.transform));

    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    //_playerControl.players.Remove(mainPlayer);
    //    //Destroy(mainPlayer);
    //}

    IEnumerator throughlyScaling(Transform hmn)
    {

        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;

        while (counter < firstSize)
        {
            counter += 15 * Time.deltaTime;
            hmn.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }

        counter = 0f;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 4f;
            hmn.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        hmn.localScale = new Vector3(firstSize, firstSize, firstSize);
        //for (int t = 0; t < _playerControl.players.Count; t++)
        //{
        //    _playerControl.players[t].GetComponent<PlayerEvolution>().firstMove();
        //}
    }



    public void upgradeClose()
    {
        panelBG.SetActive(false);
        StartCoroutine(panelScaleDown(fullPanel));
        ZombiSpawner.Instance.spawnActive = true;
        ZombiSpawner.Instance.StartGame();
    }
    IEnumerator panelScaleDown(RectTransform image)
    {
        float counter = lastImageScale;

        while (counter > firstImageScale)
        {
            counter -= 5 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(firstImageScale, firstImageScale, firstImageScale);
        panel.SetActive(false);

    }

    public void upgradeOpen()
    {
        panel.SetActive(true);
        panelBG.SetActive(true);

        buttonInteractableSet();

        StartCoroutine(panelScaleSet(fullPanel));
    }

    IEnumerator panelScaleSet(RectTransform image)
    {
        float counter = firstImageScale;
        while (counter < lastImageScale)
        {
            counter += 5 * Time.deltaTime;
            image.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);
        counter = 0f;
        float scale = 0;
        while (counter < Mathf.PI)
        {
            counter += 10 * Time.deltaTime;
            scale = Mathf.Sin(counter);
            scale *= 0.2f;
            image.localScale = new Vector3(lastImageScale + scale, lastImageScale + scale, lastImageScale + scale);
            yield return null;
        }
        image.localScale = new Vector3(lastImageScale, lastImageScale, lastImageScale);

    }

    void buttonInteractableSet()
    {

        //////////////////////////////////////////////////
        if (Globals.moneyAmount >= countAddCost[Globals.playerCount - 1])
        {
            button1.interactable = true;
            button1.GetComponent<Image>().fillAmount = 1;
        }
        else
        {
            button1.interactable = false;
            button1.GetComponent<Image>().fillAmount = (float)Globals.moneyAmount / (float)countAddCost[Globals.playerCount - 1];
        }
        //////////////////////////////////////////////////

        if (Globals.moneyAmount >= evolveAddCost[Globals.evoLevel - 1])
        {
            button2.interactable = true;
            button2.GetComponent<Image>().fillAmount = 1;
        }
        else
        {
            button2.interactable = false;
            button2.GetComponent<Image>().fillAmount = (float)Globals.moneyAmount / (float)evolveAddCost[Globals.evoLevel - 1];
        }
        //////////////////////////////////////////////////
        /*
        if (Globals.moneyAmount >= helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed])
        {
            button3.interactable = true;
            button3.GetComponent<Image>().fillAmount = 1;
        }
        else
        {
            button3.interactable = false;
            button3.GetComponent<Image>().fillAmount = (float)Globals.moneyAmount / (float)helperCenter.costTroubleSolutionSpeedUpgrade[helperCenter.levelTroubleSolutionSpeed];
        }
        */
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgradeOpen();
        }
    }
}
