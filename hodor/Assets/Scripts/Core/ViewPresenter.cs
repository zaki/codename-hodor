using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// ViewPresenter is a base class for all view presenters.
///
/// Generally speaking, view presenters are simply storage classes to interact
/// with a Unity UI. A view presenter exposes the UI prefab's components through
/// simple properties. UI logic should not be included in any subclasses.
public class ViewPresenter : MonoBehaviour
{
    private readonly string AppearTrigger    = "Appear";
    private readonly string DisappearTrigger = "Disappear";

    public bool AutoShow = true;
    private Animator UIAnimator;

    void Awake()
    {
        UIAnimator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        if (AutoShow) Show();
    }

    void Disable()
    {
        Hide();
    }

    public void Show(Action onFinished = null)
    {
        if (UIAnimator != null) StartCoroutine(ShowAsync(onFinished));
    }

    public void Hide(Action onFinished = null)
    {
        if (UIAnimator != null) StartCoroutine(HideAsync(onFinished));
    }

    protected IEnumerator ShowAsync(Action onFinished = null)
    {
        UIAnimator.SetTrigger(AppearTrigger);

        yield return new WaitForEndOfFrame();

        while (UIAnimator.IsInTransition(0) || UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        if (onFinished != null) onFinished();
    }

    protected IEnumerator HideAsync(Action onFinished = null)
    {
        UIAnimator.SetTrigger(DisappearTrigger);

        yield return new WaitForEndOfFrame();

        while (UIAnimator.IsInTransition(0) || UIAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        if (onFinished != null) onFinished();
    }
}
