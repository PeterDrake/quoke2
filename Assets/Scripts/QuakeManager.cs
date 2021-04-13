using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the earthquake, falling object calls, effects, information, etc.
/// 
/// This script was copied from the first quake game.
/// </summary>
public class QuakeManager : MonoBehaviour
{
    public static QuakeManager Instance;

    [Header("Admin Tools")]
    [SerializeField] private bool adminMode = true;
    [SerializeField] private bool showCountdown = true;
    [Space]
    [Space]

    [Tooltip("How long after starting before the earthquake goes off?")]
    [SerializeField] private float TimeBeforeQuake = 15f;

    [Tooltip("How long after the first quake before aftershock?")]
    [SerializeField] private float AftershockTime = 10f;

    //----Camera Shake Options---
    [Header("Camera Shake Options")]
    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public float ShakeDuration;
    public float ShakeAmplitude;
    public float ShakeFrequency;

    private float ShakeElapsedTime = 0f;

    private bool _surviveSatisfied;

    public bool _leaveSatisfied;

    public string leaveHouse = "LEAVEHOUSE";
    //--------------------


    [TextArea] [SerializeField] private string textOnQuake;
    [TextArea] [SerializeField] private string textAfterQuake;

    // Game object which will be disabled after quake
    [SerializeField] private GameObject enableDoors;

    [SerializeField] private GameObject frontDoor;
    //added 49
    [SerializeField] private GameObject backDoor;
    //added 51
    [SerializeField] private GameObject bedroomDoor;

    private GameObject[] doors;
    private Rigidbody[] bodies;
    private Clobberer[] clobberers;

    [HideInInspector] public bool Quaking;

    public byte quakes; //times quaked 

    private bool _inQuakeZone; // is player in a zone where the quake can happen?
    public bool _inSafeZone; // is the player safe (under the table)?

    private bool _countdownFinished;
    private float entranceGracePeriod = 2f;
    private float _timeTillQuake;

    [SerializeField] private float _minimumShakes = 1;
    private bool quakeOverride;

    //private InformationCanvas _informationCanvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        StartCoroutine(nameof(QuakeCountdown), TimeBeforeQuake);

        doors = GameObject.FindGameObjectsWithTag("Door");
        bodies = Array.ConvertAll(doors, d => d.GetComponent(typeof(Rigidbody)) as Rigidbody);
        clobberers = Array.ConvertAll(doors, d => d.GetComponent(typeof(Clobberer)) as Clobberer);

        //_informationCanvas = GameObject.Find("MiniGameClose").transform.Find("GUI").GetComponent<GuiDisplayer>().GetBanner();
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if ((_countdownFinished && !Quaking && (quakeOverride || _inQuakeZone)) || (adminMode && Input.GetKeyDown("p")))
        {
            TriggerQuake();
        }
    }

    // Starts a countdown of 'time' seconds. When the countdown finishes, the earthquake will happen
    public void TriggerCountdown(float time)
    {
        _countdownFinished = false;
        StopCoroutine(nameof(QuakeCountdown));
        StartCoroutine(nameof(QuakeCountdown), time);
    }

    // the actual countdown
    private IEnumerator QuakeCountdown(float CountdownTime)
    {
        _timeTillQuake = CountdownTime;
        while (_timeTillQuake > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeTillQuake--;
            if (showCountdown) Debug.Log("Time Till Quake: " + _timeTillQuake);
        }
        _countdownFinished = true;
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
        while (true)
        {
            ShakeCamera(ShakeDuration, ShakeAmplitude, ShakeFrequency);
            StartCoroutine(FlapDoors(ShakeDuration));
            yield return new WaitForSeconds(ShakeDuration);
            // if the player is in the safezone, and the earthquake has gone long enough, stop it 
            if (_inSafeZone && shakes >= _minimumShakes)
            {
                break;
            }

            shakes++;
        }

        StopQuake();

        frontDoor.GetComponent<Clobberer>().enabled = false;
        //added 189
        backDoor.GetComponent<Clobberer>().enabled = false;
        //added 193
        bedroomDoor.GetComponent<Clobberer>().enabled = false;
        bedroomDoor.GetComponent<Clobberer>().aftershock = true;

        //_informationCanvas.ChangeText(textAfterQuake);
        //Systems.Objectives.Satisfy("SURVIVEQUAKE");
        //Systems.Objectives.Register(QuakeManager.Instance.leaveHouse, () => _leaveSatisfied = true);

        enableDoors.SetActive(false); // allow player to exit house
        
        quakes++;
    }

    public void TriggerQuake()
    {
        if (Quaking) return;

        Quaking = true;

        StopCoroutine(nameof(QuakeCountdown));
        StopCoroutine(nameof(AftershockTime));

        //_informationCanvas.ChangeText(textOnQuake);

        StartCoroutine(ShakeIt());
    }

    public void StopQuake()
    {
        if (!Quaking || quakes > 0) return;

        virtualCameraNoise.m_AmplitudeGain = 0f;
        ShakeElapsedTime = 0f;

        Quaking = false;

        TriggerCountdown(AftershockTime);
    }

    public void InSafeZone(bool status)
    {
        _inSafeZone = status;
    }

    public void PlayerInQuakeZone(bool status)
    {

        Debug.Log("Grace Period" + entranceGracePeriod);
        if (quakes > 0 && status == false)
        {
            TriggerCountdown(entranceGracePeriod);
        }
        if (status && (_countdownFinished || _timeTillQuake < entranceGracePeriod) && (_inQuakeZone != status))
            TriggerCountdown(entranceGracePeriod);

        _inQuakeZone = status;
    }

    public void ManualTriggerAftershock(float gracePeroid)
    {
        if (quakes > 0)
        {
            quakeOverride = true;
            TriggerCountdown(gracePeroid);
        }
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



