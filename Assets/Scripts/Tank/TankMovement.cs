﻿using UnityEngine;
using UnityEngine.Events;

public class TankMovement : MonoBehaviour
{
    
    public int m_PlayerNumber = 1;         
    public float m_Speed;           
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    private float m_InitialSpeed = 12f;
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         
    private BoxCollider m_Collider;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
        m_Speed = m_InitialSpeed;
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio ();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        bool isIdle = Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f;
        AudioClip currentClip = isIdle ? m_EngineIdling : m_EngineDriving;

        if (m_MovementAudio.clip != currentClip)
        {
            m_MovementAudio.clip = currentClip;
            m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
            m_MovementAudio.Play();
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Boundaries"))
        {
            Debug.Log("Collision with boundaries");
            m_Rigidbody.velocity = Vector3.zero;
            Vector3 direction = other.contacts[0].point - transform.position;
            direction = -direction.normalized;
            m_Rigidbody.AddForce(direction * 5);
        }
        else if (other.gameObject.CompareTag("Rocks"))
        {
            m_Rigidbody.AddExplosionForce(500f, transform.position, 5f);
            var health = m_Rigidbody.GetComponent<TankHealth>();
            if (health == null) return;
            health.TakeDamage(50f); // should be a constant somewhere
        }
    }

    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    public void ReduceSpeed()
    {
        Debug.Log("Player has been hit");
        m_Speed -= 2f;
    }

    public void ResetSpeed()
    {
        m_Speed = m_InitialSpeed;
    }
}