using System;
using UnityEngine;
using UnityEngine.Windows.WebCam;


namespace JavacLMD
{


    public interface IEffect<TTarget>
    {
        void Apply(TTarget target);
        void Cancel();
    }

    [Serializable]
    public class DamageEffect : IEffect<IDamageable>
    {
        public void Apply(IDamageable target)
        {
            target.TakeDamage(5);
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class HealEffect : IEffect<IHealable>
    {
        public float healAmount;
        
        public virtual void Apply(IHealable target)
        {
            target.ReceiveHeal(healAmount);
        }

        public virtual void Cancel()
        {
            
        }
    }

    [Serializable]
    public class HealOverTimeEffect : HealEffect
    {
        public float duration;
        public float tickInterval;
        
        //timer
        private IHealable targetReference;

        public override void Apply(IHealable target)
        {
            targetReference = target;
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        void OnInterval() => targetReference?.ReceiveHeal(healAmount);
        void OnStop() => Cancel();


    }

    public interface IDamageable
    {
        void TakeDamage(float amount);
    }

    public interface IHealable
    {
        void ReceiveHeal(float amount);
    }
}
