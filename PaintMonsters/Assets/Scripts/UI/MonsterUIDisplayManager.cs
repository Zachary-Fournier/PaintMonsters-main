using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUIDisplayManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameDisplay = null; 
    public string nameText { get { return nameDisplay.text; } set { nameDisplay.text = value; } }

    [SerializeField]
    private TextMeshProUGUI healthDisplay = null;//don't make getters and setters for this, use the functions below

    [SerializeField]
    private UIShaker healthDisplayShaker = null;

    [SerializeField]
    private Image equipment1 = null, equipment2 = null, equipment3 = null, equipment4 = null;

    public void setEquipmentVisuals(ReadOnlyCollection<EquipmentSO> equipment){

        equipment1.sprite = equipment[0].DisplaySprite;
        equipment2.sprite = equipment[1].DisplaySprite;
        equipment3.sprite = equipment[2].DisplaySprite;
        equipment4.sprite = equipment[3].DisplaySprite;
    }

    /*
     * animating health display
     */

    private static readonly float HP_CHANGE_PER_SECOND = 18.0f;
    private static readonly int HP_CHANGE_FOR_MAX_SHAKE = 10;

    private Coroutine HealthChangeCoroutine = null;

    private float currentHealthValueFloat = 0;

    public void setHealthTextFormated(int value)
    {
        healthDisplay.text = "♥" + value;
    }

    public void setHealthInstant(int value)
    {
        if (HealthChangeCoroutine != null) {

            StopCoroutine(HealthChangeCoroutine);
            HealthChangeCoroutine = null;
        }

        //healthDisplayShaker.shakeMe(Mathf.Clamp01(Mathf.Abs(currentHealthValueFloat - value) / HP_CHANGE_FOR_MAX_SHAKE));

        currentHealthValueFloat = value;
        setHealthTextFormated(value);
    }

    public void setHealthAnimated(int target)
    {
        if (HealthChangeCoroutine != null)
            StopCoroutine(HealthChangeCoroutine);

        healthDisplayShaker.shakeMe(Mathf.Clamp01(Mathf.Abs(currentHealthValueFloat - target) / HP_CHANGE_FOR_MAX_SHAKE));

        HealthChangeCoroutine = StartCoroutine(healthChangeCoroutine(target));
    }

    private IEnumerator healthChangeCoroutine(int target)
    {
        if(target < currentHealthValueFloat)
            do
            {
                currentHealthValueFloat -= Time.deltaTime * HP_CHANGE_PER_SECOND;
                setHealthTextFormated((int)(currentHealthValueFloat + 0.5f));

                yield return null;//(wait until next frame)

            } while (target < currentHealthValueFloat);

        else
            do
            {
                currentHealthValueFloat += Time.deltaTime * HP_CHANGE_PER_SECOND;
                setHealthTextFormated((int)(currentHealthValueFloat + 0.5f));

                yield return null;//(wait until next frame)

            } while (target > currentHealthValueFloat);

        currentHealthValueFloat = target;
        setHealthTextFormated(target);

        HealthChangeCoroutine = null;

    }
}
