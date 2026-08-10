using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnipersScripts.Behaviors
{

    ///<summary>
    ///Used to populate and hold a list of all enemies in the game
    ///</summary>
    internal class EnemyRequiresNestOverrideInternal
    {
        internal static List<EnemyType> allEnemyTypes = new List<EnemyType>();

        public static IEnumerator populateEnemyList()
        {            
            yield return new WaitForSeconds(.5f); // wait just in case LLL or DawnLib are registering enemies
            allEnemyTypes.Clear();
            SelectableLevel allEnemiesLevel = UnityEngine.Object.FindObjectOfType<QuickMenuManager>().testAllEnemiesLevel;
            getEnemies(allEnemiesLevel.DaytimeEnemies);
            getEnemies(allEnemiesLevel.Enemies);
            getEnemies(allEnemiesLevel.OutsideEnemies);
        }
        private static void getEnemies(List<SpawnableEnemyWithRarity> enemyTypes)
        {
            foreach (SpawnableEnemyWithRarity enemy in enemyTypes)
            {
                allEnemyTypes.Add(enemy.enemyType);
            }
        }
    }

    /// <summary>
    /// Used to spawn enemies that usually require a nest
    /// </summary>
    [AddComponentMenu("SnipersScripts/EnemyRequiresNestOverride")]
    public class EnemyRequiresNestOverride : MonoBehaviour
    {
        [Tooltip("Events to run while enemy does not require nest.")]
        public UnityEngine.Events.UnityEvent<EnemyType> onEnemyDoesNotRequireNest;

        /// <summary>
        /// Sets the enemy type for which a nest requirement override is toggled.
        /// </summary>
        /// <param name="enemyType">The enemy type to toggle the nest requirement for.</param>
        public void ToggleEnemyNestRequirement(EnemyType enemyType)
        {
            foreach (EnemyType enemy in EnemyRequiresNestOverrideInternal.allEnemyTypes)
            {
                if (enemy == enemyType)
                {
                    StartCoroutine(CycleEnemyNestRequiement(enemy));
                    return;
                }
            }
        }

        /// <summary>
        /// Takes an enemy name and finds the corresponding EnemyType to toggle the nest requirement for.
        /// </summary>
        /// <param name="enemyName">The 'EnemyName' to toggle the nest requirement for.</param>
        public void ToggleEnemyNestRequirement(String enemyName)
        {
            foreach (EnemyType enemy in EnemyRequiresNestOverrideInternal.allEnemyTypes)
            {
                if (enemy.enemyName == enemyName)
                {
                    ToggleEnemyNestRequirement(enemy);
                    return;
                }
            }
        }

        private IEnumerator CycleEnemyNestRequiement(EnemyType enemy)
        {
            if (enemy != null)
            {
                bool currentNestRequirement = enemy.requireNestObjectsToSpawn;
                enemy.requireNestObjectsToSpawn = false;
                onEnemyDoesNotRequireNest?.Invoke(enemy);
                yield return new WaitForSeconds(0.5f); // Wait for a short duration to allow any events to complete that may require the nest requirement being false
                enemy.requireNestObjectsToSpawn = currentNestRequirement;
            }
        }
    }
}
