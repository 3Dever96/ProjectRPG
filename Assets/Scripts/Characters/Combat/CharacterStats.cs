using System.Collections.Generic;
using UnityEngine;

namespace ProjectRPG.Characters.Combat
{
    public abstract class CharacterStats : MonoBehaviour
    {
        public Dictionary<string, float> stats = new Dictionary<string, float>();

        public float currentHP;
        public float currentMP;
        public float currentSP;
    }
}
