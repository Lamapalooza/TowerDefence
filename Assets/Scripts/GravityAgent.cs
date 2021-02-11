using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAgent : MonoBehaviour
{
    [SerializeField] private float m_Mass;
    [SerializeField] private float m_GravityConst;
    [SerializeField] private Vector3 m_Position;
    [SerializeField] private Vector3 m_Speed;
    [SerializeField] private Vector3 m_MassCentre;
    private const float TOLERANCE = 0.1f;
    void Start()
    {
        transform.position = m_Position;
    }

    void FixedUpdate()
    {
        float distance = (m_MassCentre - transform.position).magnitude;
        if (distance < TOLERANCE)
        {
            return;
        }
        Vector3 dir = (m_MassCentre - transform.position).normalized;
        Vector3 delta = m_Speed * Time.fixedDeltaTime;
        transform.Translate(delta);
        float m_Acceleration = m_Mass * m_GravityConst / (float) Math.Pow(distance, 2);
        m_Position = transform.position + delta + dir * (m_Acceleration * (float) Math.Pow(Time.fixedDeltaTime, 2) / 2);
        m_Speed = m_Speed + dir * (m_Acceleration * Time.fixedDeltaTime);
    }
}
