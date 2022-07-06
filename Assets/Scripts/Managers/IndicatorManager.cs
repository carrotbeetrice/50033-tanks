using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public GameObject[] m_ChaseIndicators;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_ChaseIndicators.Length; i++)
        {
            m_ChaseIndicators[i].SetActive(false);
        }
    }

    public void EnableIndicator(int spotter)
    {
        m_ChaseIndicators[spotter - 2].SetActive(true);
    }

    public void DisableIndicator(int spotter)
    {
        m_ChaseIndicators[spotter - 2].SetActive(false);
    }
}
