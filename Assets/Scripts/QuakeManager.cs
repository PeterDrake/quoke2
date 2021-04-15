﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Random = UnityEngine.Random;

public class QuakeManager : MonoBehaviour
{
    public static QuakeManager Instance;

    [Header("Admin Tools")]
    [SerializeField] private bool adminMode = true;
    [SerializeField] private bool showCountdown = true;


    [Header("Camera Shake Options")]
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public float ShakeDuration = 1f;
    public float ShakeAmplitude = 5f;
    public float ShakeFrequency = 5f;

    private float ShakeElapsedTime = 0f;


    [Header("Quake Options")]
    [Tooltip("How many moves can the player make before the earthquake goes off?")]
    public int turnsTillQuakeStart = 5;
    private int initialTurn;
    private int quakeStartTurn;
    private bool isQuakeTime = false;
    private bool firstQuakeCompleted = false;

    [Tooltip("How many moves can the player make during the earthquake before they die?")]
    public int turnsTillDeath = 5;
    private int underCoverTurn;
    [Tooltip("How many seconds should the earthquake last after the player has reached cover?")]
    public float secondsUnderCover = 5f;
    private bool isUnderCover = false;

    [Tooltip("How many moves can the player make after the earthquake before they die?")]
    public int turnsTillAftershock = 5;
    private int exitHouseTurn;
    private bool exitedHouse = false;
    private bool isAftershockTime = false;


    // door shaking objects
    private GameObject[] doors;
    private Rigidbody[] bodies;
    private Clobberer[] clobberers;
    
    private bool quaking;
    private int currentTurn = 0;

    //private InformationCanvas _informationCanvas;
    //[TextArea] [SerializeField] private string textOnQuake;
    //[TextArea] [SerializeField] private string textAfterQuake;


    // scripts can 'subscribe' to this Event to have their functions called when the earthquake begins
    // so, when implemented, we will subscribe sound/other effects to this event
    public UnityEvent OnQuake;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(nameof(TurnCounter), 0);

        doors = GameObject.FindGameObjectsWithTag("Door");
        bodies = Array.ConvertAll(doors, d => d.GetComponent(typeof(Rigidbody)) as Rigidbody);
        clobberers = Array.ConvertAll(doors, d => d.transform.parent.gameObject.GetComponent(typeof(Clobberer)) as Clobberer);

        //_informationCanvas = GameObject.Find("MiniGameClose").transform.Find("GUI").GetComponent<GuiDisplayer>().GetBanner();
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        initialTurn = currentTurn;
    }

    void Update()
    {
        // if we're not already ready for a quake or the first quake has already been completed
        if (!isQuakeTime && !firstQuakeCompleted)
        {
            CheckForQuakeStart();
        } 
        // if we're in the middle of the first quake
        else if (quaking)
        {
            if (!isUnderCover)
            {
                CheckForQuakeDeath();
                CheckForUnderCover();
            }
        } 
        // if we're not in the middle of the quake, the player hasn't exited the house, and there hasn't already been an aftershock
        else if (!quaking && !exitedHouse && !isAftershockTime)
        {
            CheckForAftershockStart();
        }


        if ((isQuakeTime && !quaking) || (adminMode && Input.GetKeyDown("p"))|| (isAftershockTime && !quaking))
        {
            TriggerQuake();
        }
    }

    // purely for test purposes until turn counting in player movement is implemented
    private IEnumerator TurnCounter(int startingTurn)
    {
        currentTurn = startingTurn;
        while (currentTurn >= 0)
        {
            yield return new WaitForSeconds(1f);
            currentTurn++;
            if (showCountdown) Debug.Log("Turn: " + currentTurn);
        }
    }

    private IEnumerator UnderCoverCountdown(float CountdownTime)
    {
        float timeLeftInCover = CountdownTime;
        while (timeLeftInCover > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeftInCover--;
            if (showCountdown) Debug.Log("Time Left In Cover: " + timeLeftInCover);
        }

        StopQuake();
    }

    // flaps each of the doors for the given duration
    public IEnumerator FlapDoors(float duration)
    {
        while (duration > 0)
        {
            Vector3 kick = Random.onUnitSphere * 1;
            foreach (Rigidbody b in bodies)
            {
                b.AddRelativeForce(kick, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(0.25f);
            duration -= 0.25f;

        }
    }

    // everything that happens during the earthquake
    private IEnumerator ShakeIt()
    {
        foreach (Clobberer c in clobberers)
        {
            c.enabled = true;
        }

        int shakes = 1;
        while (quaking)
        {
            ShakeCamera(ShakeDuration, ShakeAmplitude, ShakeFrequency);
            StartCoroutine(FlapDoors(ShakeDuration));
            yield return new WaitForSeconds(ShakeDuration);

            shakes++;
        }
    }

    public void CheckForQuakeStart()
    {
        if (currentTurn >= initialTurn + turnsTillQuakeStart)
        {
            isQuakeTime = true;
            Debug.Log("Earthquake!");
        }
    }

    public void CheckForUnderCover()
    {
        //TODO add cover implementation
        isUnderCover = true;
        // we start the coroutine here instead of in the update function so we don't start it multiple times
        StartCoroutine(nameof(UnderCoverCountdown), secondsUnderCover);
    }

    public void CheckForQuakeDeath()
    {
        if (currentTurn >= quakeStartTurn + turnsTillDeath)
        {
            //TODO kill player
        }
    }

    public void CheckForAftershockStart()
    {
        if (currentTurn >= underCoverTurn + turnsTillAftershock)
        {
            isAftershockTime = true;
            Debug.Log("Aftershock!");
        }
    }

    public void TriggerQuake()
    {
        if (quaking) return;

        quaking = true;

        quakeStartTurn = currentTurn;
        firstQuakeCompleted = true;

        OnQuake.Invoke(); // every function subscribed to OnQuake is called here

        //_informationCanvas.ChangeText(textOnQuake);

        StartCoroutine(ShakeIt());
    }

    public void StopQuake()
    {
        if (!quaking) return;

        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;

        isQuakeTime = false;
        quaking = false;

        StopCoroutine(nameof(ShakeIt));
        underCoverTurn = currentTurn;

        foreach (Clobberer c in clobberers)
        {
            c.enabled = false;
        }

        //_informationCanvas.ChangeText(textAfterQuake);
        //Systems.Objectives.Satisfy("SURVIVEQUAKE");
        //Systems.Objectives.Register(QuakeManager.Instance.leaveHouse, () => _leaveSatisfied = true);

        //enableDoors.SetActive(false); // allow player to exit house
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        ShakeElapsedTime = duration;
        while (ShakeElapsedTime > 0)
        {

            // Set Cinemachine Camera Noise parameter
            virtualCameraNoise.m_AmplitudeGain = amplitude;
            virtualCameraNoise.m_FrequencyGain = frequency;

            // Update Shake Timer
            ShakeElapsedTime -= Time.deltaTime;
        }
    }
}



