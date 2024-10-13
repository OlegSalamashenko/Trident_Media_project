using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheel : MonoBehaviour
{
    [SerializeField] private GameObject wheel;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationTimeMaxSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private float scaleTime;
    [SerializeField] private int numberOfSpins;
    [SerializeField] private List<GameObject> prizes;

    public event EventHandler OnUpdateMoneyUI;

    private bool isSpin = false;
    private float slowDownTime;

    private float maxAngle = 0f;
    private float minAngle = 0f;
    private int winSector;

    public void StartPlay()
    {
        if (!isSpin)
        {
            StartCoroutine(SpinWheel());
        }
    }

    private IEnumerator SpinWheel()
    {
        winSector = SetWin();
        isSpin = true;
        float elapsedTime = 0f;
        float rotSpeed;
        //ускорение
        while (elapsedTime < accelerationTime)
        {
            rotSpeed = Mathf.Lerp(0, rotationSpeed, elapsedTime / accelerationTime);
            wheel.transform.rotation *= Quaternion.Euler(0, 0, rotSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Продолжительное кружение
        elapsedTime = 0f;

        while (elapsedTime < rotationTimeMaxSpeed)
        {
            wheel.transform.rotation *= Quaternion.Euler(0,0, rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Замедление
        float distance = (numberOfSpins * 360f) + UnityEngine.Random.Range(minAngle + 8, maxAngle - 8) - wheel.transform.rotation.eulerAngles.z;
        slowDownTime = (2 * distance) / rotationSpeed;
        float slowDown = rotationSpeed / slowDownTime;
        rotSpeed = rotationSpeed;

        elapsedTime = 0f;

        while (elapsedTime < slowDownTime)
        {
            rotSpeed -= slowDown * Time.deltaTime;
            wheel.transform.rotation *= Quaternion.Euler(0,0,rotSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Увеличение
        elapsedTime = 0f;
        while (elapsedTime < scaleTime)
        {
            prizes[winSector].transform.localScale = (1 + 0.2f * Mathf.Sin(8 * Mathf.PI * elapsedTime / scaleTime)) * Vector3.one;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnUpdateMoneyUI?.Invoke(this,EventArgs.Empty);
        isSpin = false;
    }

    private int SetWin()
    {
        int randomSector = UnityEngine.Random.Range(0,prizes.Count);
        Debug.Log("WIN: " + prizes[randomSector] + " | index = " + randomSector);
        maxAngle = 360f / prizes.Count * (randomSector + 1);
        minAngle = 360f / prizes.Count * randomSector ;
        return randomSector;
    }

    public int GetWinningSector()
    {
        return winSector;
    }


}
