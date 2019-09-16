using UnityEngine;

[RequireComponent(typeof(OVRControllerHelper), typeof(LineRenderer))]
public class ViveHandInteractionLaserPointer : MonoBehaviour
{
    public Material m_blipMaterial;
    public bool m_isRightController = false;
    private GameObject m_targetObject;
    private bool m_isHoldingSomething = false;

    private RaycastHit m_raycast;

    private OVRControllerHelper m_remote;
    private LineRenderer m_lineRenderer;
    private Vector3 m_endLinePos;

    private float m_timeOut;

    private Transform m_blip;

    static bool hasTriggered = false;

    private bool m_controllerConnected
    {
        get { return OVRInput.IsControllerConnected(m_remote.m_controller); }
    }

    // ---------- ---------- ---------- ---------- ----------
    void Start ()
    {
        m_remote = GetComponent<OVRControllerHelper>();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_blip = (m_blip == null) ? GameObject.CreatePrimitive(PrimitiveType.Sphere).transform : m_blip;
        RestBlip();
    }


    // ---------- ---------- ---------- ---------- ----------
    void Update()
    {
        m_blip.gameObject.SetActive(gameObject.activeInHierarchy);

        if (m_controllerConnected)
        {
            if(m_blip.gameObject.activeInHierarchy == false){
                m_blip.gameObject.SetActive(true);
            }

            CheckObjectHit();

            if(CheckInput())
            {
                Mouledoux.Components.Mediator.instance.NotifySubscribers("lasertriggeron");
   
                if (CheckForInteractableObject(m_targetObject) != null)
                {
                    Mouledoux.Components.Mediator.instance.NotifySubscribers("oninteract");

                    Mouledoux.Components.Mediator.instance.NotifySubscribers
                        (m_targetObject.GetInstanceID().ToString() + "->oninteract");
                }
            }

            if (CheckLongInput())   // We are holding down the trigger
            {
                m_timeOut+= Time.deltaTime;
            }

            else if (CheckOffInput() && m_targetObject != null) // We have let go of the trigger, AND have a target object
            {
                if (CheckForInteractableObject(m_targetObject) != null && m_timeOut < 8f)
                {
                    OnObjectInteract();
                }
            }

            else if (CheckOffInput())
            {
                Mouledoux.Components.Mediator.instance.NotifySubscribers("lasertriggeroff");
                RestBlip();
            }

            else
            {
                m_timeOut = 0f;
            }
        }

        else{
            m_blip.gameObject.SetActive(false);
        }
        bool[] extraLazerConditions = { true };
        UpdateLaser(extraLazerConditions);
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckObjectHit()
    {
        if (Physics.Raycast(m_remote.transform.position, m_remote.transform.forward, out m_raycast))
        {
            if (m_targetObject != m_raycast.transform.gameObject && !m_isHoldingSomething && m_timeOut < 4f)
            {
                if (m_targetObject != null)
                {
                    Mouledoux.Components.Mediator.instance.NotifySubscribers("offhighlight");

                    //RestBlip();

                    Mouledoux.Components.Mediator.instance.NotifySubscribers
                        (m_targetObject.GetInstanceID().ToString() + "->offhighlight");
                }
                
                m_targetObject = m_raycast.transform.gameObject;

                Mouledoux.Components.Mediator.instance.NotifySubscribers("onhighlight");

                Mouledoux.Components.Mediator.instance.NotifySubscribers
                    (m_targetObject.GetInstanceID().ToString() + "->onhighlight");
            }
            
            m_endLinePos = m_raycast.point;
            return true;
        }

        else if (m_targetObject != null)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers("offhighlight");
            
            //RestBlip();

            Mouledoux.Components.Mediator.instance.NotifySubscribers
                (m_targetObject.GetInstanceID().ToString() + "->offhighlight");

            m_targetObject = null;
        }

        m_endLinePos = m_remote.transform.position + m_remote.transform.forward;
        RestBlip();
        return false;
    }


    // ---------- ---------- ---------- ---------- ----------
    public InteractableObject CheckForInteractableObject(GameObject go)
    {
        return go.GetComponent<InteractableObject>();
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckInput()
    {
        //m_hand.controller.TriggerHapticPulse();
        //return (m_hand.grabPinchAction.stateDown);
        if(m_isRightController){
            return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
        }
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckLongInput()
    {
        //m_hand.controller.TriggerHapticPulse();
        //eturn (m_hand.GetBestGrabbingType() == Valve.VR.InteractionSystem.GrabTypes.Pinch);
        if(m_isRightController){
            return OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        }
        return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
    }


    // ---------- ---------- ---------- ---------- ----------
    public bool CheckOffInput()
    {
        //m_hand.controller.TriggerHapticPulse();
        //return (m_hand.grabPinchAction.stateUp);
        if(m_isRightController){
            return OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger);
        }
        return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
    }


    // ---------- ---------- ---------- ---------- ----------
    public int OnObjectInteract()
    {
        InteractableObject io = m_targetObject.GetComponent<InteractableObject>();

        if (io.m_interactionType == InteractableObject.InteractionType.PICKUP && !io.m_lockedInPlace)
        {
            if (m_isHoldingSomething) return -1;
            io.m_lockedInPlace = true;

            StartCoroutine(HoldObject(m_raycast.collider));
        }
        
        else if (io.m_interactionType == InteractableObject.InteractionType.LONGINTERACT)
        {
            StartCoroutine(LongInteract(m_raycast.transform.gameObject));
        }

        else
        {
            StartCoroutine(OffInteract(m_raycast.transform.gameObject));
        }

        return 0;
    }


    // ---------- ---------- ---------- ---------- ----------
    public void UpdateLaser(bool[] extraConditions)
    {
        bool extraCondition = true;
        foreach(bool condition in extraConditions)
        {
            if (condition == false)
            {
                extraCondition = false;
                break;
            }
        }

        m_lineRenderer.enabled = m_controllerConnected && extraCondition;
        m_lineRenderer.SetPositions( new Vector3[] {m_remote.transform.position, m_remote.transform.position + m_remote.transform.forward});
        
        if(Vector3.Distance(m_raycast.point, transform.position) >= 25){
            RestBlip();
        }

        else{
            if(m_endLinePos == m_remote.transform.position + m_remote.transform.forward){
                RestBlip();
            }
            else{
                SetBlip(m_endLinePos, (m_raycast.point - transform.position).magnitude);
            }
        }
    }


    // ---------- ---------- ---------- ---------- ----------
    public System.Collections.IEnumerator HoldObject(Collider go)
    {
        Vector3 lastPos = Vector3.zero;
        Collider collider = go;
        Color lineColor = m_lineRenderer.endColor;

        collider.enabled = false;
        m_isHoldingSomething = true;

        bool canDrop = false;
        while (CheckInput() || canDrop ==  false)
        {
            go.transform.position = m_lineRenderer.GetPosition(m_lineRenderer.positionCount - 1);
            m_lineRenderer.endColor = canDrop ? Color.green : Color.red;

            yield return null;
        }

        
        Mouledoux.Components.Mediator.instance.NotifySubscribers("offinteract");

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (go.gameObject.GetInstanceID().ToString() + "->offinteract");

        go.transform.parent = m_raycast.transform;
        go.transform.position = m_raycast.point;

        collider.enabled = true;
        m_isHoldingSomething = false;
        m_lineRenderer.endColor = lineColor;


        if (!hasTriggered)
        {
            Mouledoux.Components.Mediator.instance.NotifySubscribers("trigger");
            hasTriggered = true;
        }
    }


    // ---------- ---------- ---------- ---------- ----------
    public System.Collections.IEnumerator LongInteract(GameObject go)
    {
        yield return new WaitWhile(() => (CheckLongInput()));


        Mouledoux.Components.Mediator.instance.NotifySubscribers("offinteract");

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (go.GetInstanceID().ToString() + "->offinteract");

        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (m_raycast.transform.gameObject.GetInstanceID().ToString() + "->offinteract");
    }


    // ---------- ---------- ---------- ---------- ----------
    public System.Collections.IEnumerator OffInteract(GameObject go)
    {
        yield return new WaitWhile(() => (CheckInput()));

        Mouledoux.Components.Mediator.instance.NotifySubscribers("offinteract");
        


        Mouledoux.Components.Mediator.instance.NotifySubscribers
            (go.GetInstanceID().ToString() + "->offinteract");
    }


    // ---------- ---------- ---------- ---------- ----------
    public void OnDestroy()
    {
        hasTriggered = false;
    }

    private void RestBlip(){
        m_blip.localScale = new Vector3(0, 0, 0);
        m_blip.GetComponent<Collider>().enabled = false;
        m_blip.GetComponent<Renderer>().material = m_blipMaterial;
    }

    private void SetBlip(Vector3 pos, float scale){
        m_blip.position = pos;
        var newScale = Mathf.Clamp(0.02f * scale, 0.2f, 0.4f);
        m_blip.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void BlipReadout(){
        print($"The blip's scale is {m_blip.localScale}.");
    }

    private void OnEnable() {
        if(m_blip != null){
            m_blip.gameObject.SetActive(true);
        }
    }

    private void OnDisable() {
        if(m_blip != null){
            m_blip.gameObject.SetActive(false);
        }
    }
}