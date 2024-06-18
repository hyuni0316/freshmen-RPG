using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentSituation", menuName = "CurrentSituation/create new current situation")]
public class CurrentSituation : ScriptableObject
{
    [SerializeField] public bool HakmoonInfo;
    [SerializeField] public bool HakmoonBattle;
    [SerializeField] public bool PoscoInfo;
    [SerializeField] public bool PoscoBattle;
    [SerializeField] public bool AsanInfo;
    [SerializeField] public bool AsanBattle;
    [SerializeField] public bool BossBattle;
}
