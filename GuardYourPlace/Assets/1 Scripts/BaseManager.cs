using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour,ILoseObserver
{
    [SerializeField] GameObject[] basePrefabs;
    public static BaseManager Instance;
    GameObject currentBase;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GameManager.Instance.Add_LoseObserver(this);
        StartCoroutine(startDelay());
    }
    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(0.5f);
        baseCreater();
    }
    public void LoseScenario()
    {
        StartCoroutine(loseDelay());
    }
    IEnumerator loseDelay()
    {
        for (int i = 0; i < currentBase.transform.childCount; i++)
        {
            StartCoroutine(downScaling(currentBase.transform.GetChild(i)));
            yield return new WaitForSeconds(0.05f);
        }
        //yield return new WaitForSeconds(0.35f);
        //Destroy(currentBase);
    }
    public void oldBaseDest()
    {
        StartCoroutine(scaleDest());
        
    }
    IEnumerator scaleDest()
    {
        for (int i = 0; i < currentBase.transform.childCount; i++)
        {
            StartCoroutine(downScaling(currentBase.transform.GetChild(i)));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.35f);
        Destroy(currentBase);
        baseCreater();
        //StartCoroutine(upScaling());

    }
    IEnumerator downScaling(Transform hmn)
{
    //////
 
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
        counter = firstSize;

        while (counter > 0)
        {
            counter -= 15 * Time.deltaTime;
            hmn.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        hmn.localScale = new Vector3(0, 0, 0);

    }


    IEnumerator scaleUp()
    {
        for (int i = 0; i < currentBase.transform.childCount; i++)
        {
            StartCoroutine(upScaling(currentBase.transform.GetChild(i)));
            yield return new WaitForSeconds(0.05f);
        }


    }
    IEnumerator upScaling(Transform hmn)
    {
        //////

        float counter = 0f;
        float firstSize = 1f;
        float sizeDelta;

        while (counter < 1f)
        {
            counter += 15 * Time.deltaTime;
            hmn.localScale = new Vector3(counter, counter, counter);
            yield return null;
        }
        hmn.localScale = new Vector3(1, 1, 1);
        counter = 0;
        while (counter < Mathf.PI)
        {
            counter += 15 * Time.deltaTime;
            sizeDelta = 1f - Mathf.Abs(Mathf.Cos(counter));
            sizeDelta /= 3f;
            hmn.localScale = new Vector3(firstSize + sizeDelta, firstSize + sizeDelta, firstSize + sizeDelta);

            yield return null;
        }
        hmn.localScale = new Vector3(firstSize, firstSize, firstSize);
        hmn.localScale = new Vector3(1, 1, 1);

    }
    // Update is called once per frame
    public void baseCreater()
    {
        GameObject basee = Instantiate(basePrefabs[Globals.evoLevel - 1], transform.position, transform.rotation, this.transform);
        currentBase = basee;
        for (int i = 0; i < currentBase.transform.childCount; i++)
        {
            currentBase.transform.GetChild(i).localScale = new Vector3(0, 0, 0);
        }
        StartCoroutine(scaleUp());
    }
}
