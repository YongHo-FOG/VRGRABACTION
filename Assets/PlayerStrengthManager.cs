using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerStrengthManager : MonoBehaviour
{
    public float strength = 10f;
    public float baseSpeed = 1.5f;

    private ActionBasedContinuousMoveProvider moveProvider;

    void Start()
    {
        moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        var weightComp = args.interactableObject.transform.GetComponent<ObjectWeight>();
        if (weightComp == null) return;

        float weight = weightComp.weight;

        if (strength >= weight)
        {
            float slowFactor = Mathf.Clamp01(weight / strength);
            moveProvider.moveSpeed = baseSpeed * (1f - slowFactor);
        }
        else
        {
            // 너무 무거워서 들 수 없음 → 강제로 놓기
            if (args.interactorObject is XRBaseInteractor baseInteractor)
            {
                baseInteractor.interactionManager.SelectExit(baseInteractor, args.interactableObject);
                Debug.Log("너무 무거워서 들 수 없습니다.");
            }
        }
        
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        // 속도 원상 복구
        moveProvider.moveSpeed = baseSpeed;
    }
}
