using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEditor;
using Random = UnityEngine.Random;

public class QuakeManager : MonoBehaviour
{
    public static QuakeManager Instance;

    public QuakeSafeZoneManager quakeSafeZoneManager;

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
    private bool hasBeenUnderCover = false;

    [Tooltip("How many moves can the player make after the earthquake before they die?")]
    public int turnsTillAftershock = 5;
    private int exitHouseTurn;
    private bool exitedHouse = false;
    private bool isAftershockTime = false;
    [Tooltip("Should an aftershock trigger and kill the player if they step into the house?")]
    public bool automaticAftershock = false;


    // door shaking objects
    private GameObject[] doors;
    private Rigidbody[] bodies;
    private Clobberer[] clobberers;
    
    private bool quaking;

    private GameObject player;
    private PlayerMover playerMoverScript;
    private PlayerDeath playerDeathScript;
	private GameObject canvas;

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
		canvas = GameObject.Find("Death UI Canvas");
        player = GameObject.FindGameObjectWithTag("Player");
        playerDeathScript = player.GetComponent<PlayerDeath>();
        playerDeathScript.DeactivateKillScreen(canvas);
        
        playerMoverScript = player.GetComponent<PlayerMover>();
        

        doors = GameObject.FindGameObjectsWithTag("Door");
        bodies = Array.ConvertAll(doors, d => d.GetComponent(typeof(Rigidbody)) as Rigidbody);
        clobberers = Array.ConvertAll(doors, d => d.transform.gameObject.GetComponent(typeof(Clobberer)) as Clobberer);

        //_informationCanvas = GameObject.Find("MiniGameClose").transform.Find("GUI").GetComponent<GuiDisplayer>().GetBanner();
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        initialTurn = GlobalControls.TurnNumber;
    }

    void Update()
    {
        // if we're not already ready for a quake or the first quake has already been completed
        if (!isQuakeTime && !firstQuakeCompleted && !automaticAftershock)
        {
            CheckForQuakeStart();
        } 
        // if we're in the middle of the first quake
        else if (quaking && !automaticAftershock)
        {
            if (!isUnderCover)
            {
                CheckForQuakeDeath();
                CheckForUnderCover();
            }
            else if (!hasBeenUnderCover)
            {
                StartCoroutine(nameof(UnderCoverCountdown), secondsUnderCover);
                hasBeenUnderCover = true;
            }
        }
        // if we're not in the middle of the quake, the player hasn't exited the house, and there hasn't already been an aftershock
        else if (!quaking && !quakeSafeZoneManager.playerInSafeZone && (!isAftershockTime || automaticAftershock))
        {
            CheckForAftershockStart();
        }

        if ((isQuakeTime && !quaking) || (adminMode && Input.GetKeyDown("p"))|| (isAftershockTime && !quaking))
        {
            TriggerQuake();
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
        UnlockDoors();
        int shakes = 1;
        while (quaking)
        {
            ShakeCamera(ShakeDuration, ShakeAmplitude, ShakeFrequency);
            StartCoroutine(FlapDoors(ShakeDuration));
            yield return new WaitForSeconds(ShakeDuration);

            shakes++;
        }
    }

    private void UnlockDoors()
    {
        foreach (GameObject door in doors)
        {
            door.layer = LayerMask.NameToLayer("Door");
            door.GetComponent<Rigidbody>().constraints = 0;
        }
    }

    public void CheckForQuakeStart()
    {
        if (GlobalControls.TurnNumber >= initialTurn + turnsTillQuakeStart)
        {
            isQuakeTime = true;
            Debug.Log("Earthquake!");
        }
    }

    public void CheckForUnderCover()
    {
        if (playerMoverScript.underTable)
        {
            isUnderCover = true;
        }
    }
    
    public void CheckForQuakeDeath()
    {
        if (GlobalControls.TurnNumber >= quakeStartTurn + turnsTillDeath && !quakeSafeZoneManager.playerInSafeZone)
        {
            //TODO kill player
            Debug.Log("You were crushed by a falling object!");
            playerDeathScript.KillPlayer(this.gameObject, 0);

        }
    }

    public void CheckForAftershockStart()
    {
        if (GlobalControls.TurnNumber >= underCoverTurn + turnsTillAftershock || automaticAftershock)
        {
            isAftershockTime = true;
            Debug.Log("Aftershock!");
            //TODO kill player
            Debug.Log("You were killed in an aftershock!");
            playerDeathScript.KillPlayer(this.gameObject, 1);
        }
    }

    public void TriggerQuake()
    {
        if (quaking) return;

        quaking = true;

        quakeStartTurn = GlobalControls.TurnNumber;
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
        underCoverTurn = GlobalControls.TurnNumber;

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



