using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField]float duration = 5.0f;
    [SerializeField] GameObject GM;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator camAnimator;
    [SerializeField] CanvasGroup canvasGroup;
    private GameObject floor;
    private ColorAdjustments colorAdjustments;
    private bool selectedStart = false;

    private void Awake()
    {
        floor = this.gameObject;
    }

    private void Start()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            StartCoroutine(ChangeSaturation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!selectedStart)
            floor.transform.Rotate(new Vector3(0, -1, 0) * 4 * Time.deltaTime);
    }

    IEnumerator ChangeSaturation()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            float currentTime = 0f;
            while (currentTime <= duration)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(-100, 0, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            }
            while (currentTime >= 0)
            {
                colorAdjustments.saturation.value = Mathf.Lerp(-100, 0, currentTime / duration);
                currentTime -= Time.deltaTime;
                yield return null;
            }
        }
    }

    public void startTheGame()
    {
        int isDed = Animator.StringToHash("isDed");
        int camMove = Animator.StringToHash("CamMove");
        playerAnimator.SetBool(isDed, true);
        camAnimator.SetBool(camMove, true);
        selectedStart = true;
        StartCoroutine(FadeCanvasGroup());
        StartCoroutine(launchGame());
    }
    private IEnumerator FadeCanvasGroup()
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = 0f;
        float percentageComplete = 0f;

        while (percentageComplete < 1)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / 1;
            float currentValue = Mathf.Lerp(1, 0, percentageComplete);

            canvasGroup.alpha = currentValue;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator launchGame()
    {
        SceneSwitch switchscene = GM.GetComponent<SceneSwitch>();
        yield return new WaitForSeconds(5);
        switchscene.SwitchScene(1);
    }
}
