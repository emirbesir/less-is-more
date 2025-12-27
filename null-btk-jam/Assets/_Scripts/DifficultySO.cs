using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DifficultySO", menuName = "ScriptableObjects/DifficultySO", order = 1)]
public class DifficultySO : ScriptableObject
{
    public List<int> maxDeathsAllowedInLevel;
}
