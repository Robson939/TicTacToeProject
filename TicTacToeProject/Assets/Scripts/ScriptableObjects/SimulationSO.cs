using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimulationData", menuName = "ScriptableObjects/SimulationData", order = 1)]
public class SimulationSO : ScriptableObject
{
    public string tierName;
    public byte depth;
    public byte iterations;
}