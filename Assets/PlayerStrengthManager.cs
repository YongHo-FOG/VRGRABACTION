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
            // �ʹ� ���ſ��� �� �� ���� �� ������ ����
            if (args.interactorObject is XRBaseInteractor baseInteractor)
            {
                baseInteractor.interactionManager.SelectExit(baseInteractor, args.interactableObject);
                Debug.Log("�ʹ� ���ſ��� �� �� �����ϴ�.");
            }
        }
        
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        // �ӵ� ���� ����
        moveProvider.moveSpeed = baseSpeed;
    }
}
