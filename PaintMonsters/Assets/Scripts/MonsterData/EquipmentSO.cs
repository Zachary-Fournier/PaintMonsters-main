using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Equipment")]
public class EquipmentSO : ScriptableObject
{
    [SerializeField]
    [Tooltip("Equipment name.")]
    private string displayName = "";
    internal string DisplayName => displayName;

    [SerializeField]
    [Tooltip("For UI displaying of item.")]
    private Sprite displaySprite = null;
    public Sprite DisplaySprite => displaySprite;

    public enum EquipmentSlot { Head, Body, Arms, Legs, Weapon };
    [SerializeField]
    [Tooltip("What part of the body is this equiped to?")]
    private EquipmentSlot slot = EquipmentSlot.Head;
    public EquipmentSlot Slot => slot;


    [Header("Add or subtract from the base stats seen in MonsterFightDirector.")]
    [Range(-10, 10)]
    [SerializeField]
    private int healthMod = 0;
    [SerializeField]
    [Range(-10, 10)]
    private int powerMod = 0, speedMod = 0, luckMod = 0;

    //This could be an equipment feature itself, but this would require each stat variation being its own
    //file instance, which is annoying.
    internal void applyBaseStatModifications(MonsterFightDirector mfd)
    {
        mfd.Health += healthMod;
        mfd.Power += powerMod;
        mfd.Speed += speedMod;
        mfd.Luck += luckMod;
    }

    [SerializeField]
    [Tooltip("List of other things this piece of equipment does.")]
    private List<EquipmentFeatureSO> features = new List<EquipmentFeatureSO>();
    public ReadOnlyCollection<EquipmentFeatureSO> Features => features.AsReadOnly();

}
