using System;
using RPG.CoreFeatures;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70f;
        float healthPoints = -1f;
        bool isDead = false;


        private void Start() 
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);


            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0){Die(); AwardExperience(instigator);}
        }

        public float GetHealthPoints(){return healthPoints;}

        public float GetMaxHealthPoints(){return GetComponent<BaseStats>().GetStat(Stat.Health);}


        public void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            BaseStats baseStats = GetComponent<BaseStats>();
            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }


        public object CaptureState()
        {
            return healthPoints;
        }


        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0) { Die(); }
        }


        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("Death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    } 
}

