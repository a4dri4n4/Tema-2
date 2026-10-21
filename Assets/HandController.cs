using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandController : MonoBehaviour
{
    public InputActionReference grabInput;
    public XRBaseInteractor interactor;   // Near-Far Interactor de pe același controller
    public float pointingDuration = 3f;
    public float blendTime = 0.1f;

    private Animator animator;
    private Coroutine returnRoutine;
    private bool grabbing;
    private bool thrownThisRelease;

    private void Awake() => animator = GetComponent<Animator>();

    private void OnEnable()
    {
        if (grabInput != null) grabInput.action.Enable();
        if (interactor != null) interactor.selectExited.AddListener(OnBallReleased);
    }

    private void OnDisable()
    {
        if (interactor != null) interactor.selectExited.RemoveListener(OnBallReleased);
    }

    private void Update()
    {
        if (grabInput == null) return;

        if (grabInput.action.WasPressedThisFrame())
        {
            grabbing = true;
            thrownThisRelease = false;
            Play("Fist");
        }
        else if (grabInput.action.WasReleasedThisFrame())
        {
            grabbing = false;
            StartCoroutine(HandleRelease());
        }
    }

    // Se apelează când dai drumul mingii (o arunci)
    private void OnBallReleased(SelectExitEventArgs args)
    {
        thrownThisRelease = true;
        Play("Pointing");
        returnRoutine = StartCoroutine(ReturnAfter(pointingDuration));
    }

    // Așteaptă un frame, ca să apuce să vină evenimentul de aruncare
    private IEnumerator HandleRelease()
    {
        yield return null;
        if (!thrownThisRelease) Play("Default");
        thrownThisRelease = false;
    }

    private void Play(string state)
    {
        if (returnRoutine != null) StopCoroutine(returnRoutine);
        animator.CrossFade(state, blendTime);
    }

    private IEnumerator ReturnAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.CrossFade(grabbing ? "Fist" : "Default", blendTime);
    }
}