using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Monster Data")]
public class MonsterDataSO : ScriptableObject
{
    [SerializeField]
    [Tooltip("User's drawn character sprite.")]
    private Sprite standingSprite = null;
    public Sprite StandingSprite => standingSprite;

    [SerializeField]
    [Tooltip("Monster's stat and ability effecting equipment decided on by the user.")]
    private List<EquipmentSO> equipment = new List<EquipmentSO>();
    public ReadOnlyCollection<EquipmentSO> Equipment => equipment.AsReadOnly();

}
