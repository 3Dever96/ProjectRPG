using System.Collections.Generic;
using UnityEngine;

namespace ProjectRPG.Characters.Combat
{
    public class PlayerStats : CharacterStats
    {
        public List<string> skills = new List<string>();

        public bool canRegenSp;
        public bool lockRegenSp;

        private void Awake()
        {
            stats.Add("HP", 144f);
            stats.Add("MP", 96f);
            stats.Add("SP", 96f);
            stats.Add("ATK", 48f);
            stats.Add("DEF", 48f);
            stats.Add("M.ATK", 48f);
            stats.Add("M.DEF", 48f);
            stats.Add("AGI", 24f);
            stats.Add("LUK", 24f);
            stats.Add("WIS", 24f);
            stats.Add("CHA", 24f);
            stats.Add("TAC", 24f);

            currentHP = stats["HP"];
            currentMP = stats["MP"];
            currentSP = stats["SP"];
        }

        private void Update()
        {
            if ((canRegenSp && currentSP < stats["SP"]) || lockRegenSp)
            {
                currentSP = Mathf.Clamp(currentSP + 15f * Time.deltaTime, 0, stats["SP"]);

                if (currentSP == stats["SP"])
                {
                    lockRegenSp = false;
                }
            }
        }
    }
}
