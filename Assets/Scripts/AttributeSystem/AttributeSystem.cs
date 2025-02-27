//Author: Rickard Lindgren

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttributeSystem : HitComponent
{

    //[SerializeField] private Text health;
    //[SerializeField] private Text stamina;

    
    [SerializeField] private List<Attribute> activeAttributes = new List<Attribute>();
    [SerializeField] private List<Effect> activeEffects = new List<Effect>();
    //[SerializeField] private Effect.EFFECT_TAG effectTag = Effect.EFFECT_TAG.none;

    
    private Dictionary<Attribute.ATTRIBUTE_TYPE, Attribute> attributeByType = new Dictionary<Attribute.ATTRIBUTE_TYPE, Attribute>();
    private Dictionary<string, Effect> effectByName = new Dictionary<string, Effect>();
    private float stunTimer;

    private void Awake()
    {
        foreach (Attribute a in activeAttributes)
            attributeByType.Add(a.AttributeType, a);
    }

    /*Oklart om den h�r beh�vs??*/
    public void ApplyAttribute(Attribute.ATTRIBUTE_TYPE type, float value)
    {
        
        if (attributeByType[type])
            attributeByType[type].AttributeValue += value;
    }

    public override void HandleHit(HitInfo info)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyEffect(Effect effect)
    {
        
        if (attributeByType[effect.AttributeType])
        {
            attributeByType[effect.AttributeType].AttributeValue = effect.AttributeValue;
        }
        
    }

    /*Get effectinfo from event and send that effect here*/
    public void ActivateEffect(Effect effect)
    {
        effectByName.Add(effect.name, effect);
    }

    public float GetAttributeValue(Attribute.ATTRIBUTE_TYPE type)
    {
        return attributeByType[type].AttributeValue;
    }

    /*On Reload and checkpoint*/
    public void ResetAttributes()
    {
        
         foreach (var a in attributeByType.Keys)
            attributeByType[a].Reset();

    }

    private void Update()
    {
       
        /*DEN H�R KOMMER INTE BEH�VAS SENARE, �r bara nu f�r att kunna l�gga till. detta kommer g�ras i addEffect senare*/
        for(int i = 0; i < activeEffects.Count; i++)
        {
            effectByName.Add(activeEffects[i].name, activeEffects[i]);
            activeEffects.Remove(activeEffects[i]);
        }
        /*DEN H�R KOMMER INTE BEH�VAS SENARE, �r bara nu f�r att kunna l�gga till. detta kommer g�ras i addEffect senare*/



        /*Here for testing*/
        //health.text = ((int)activeAttributes[0].AttributeValue).ToString();
        //stamina.text = ((int)activeAttributes[1].AttributeValue).ToString();

        foreach(Effect effect in effectByName.Values)
        {
            switch (effect.DurationType)
            {
                //Instant Effects like damage on Hit
                case Effect.EFFECT_DURATION_TYPE.instant:
                    ApplyEffect(effectByName[effect.name]);
                    effectByName[effect.name].Reset();
                    effectByName.Remove(effect.name);
                    break;

                //temporaryEffects like Stun
                case Effect.EFFECT_DURATION_TYPE.overtime:
                    effectByName[effect.name].Duration -= Time.deltaTime;
                    if (effectByName[effect.name].Duration > 0)
                        ApplyEffect(effectByName[effect.name]);
                    else
                    {
                        effectByName[effect.name].Reset();
                        effectByName.Remove(effect.name);
                    }

                    break;

                //Permanent FX like staminaRegen
                case Effect.EFFECT_DURATION_TYPE.permanent:
                    ApplyEffect(effectByName[effect.name]);
                    break;
            }
        }
        
        /*
        //checks if any effect is stun effect
        foreach(Effect e in activeEffects)
        {
            if(e.EffectTag == Effect.EFFECT_TAG.stun)
            {
                effectTag = e.EffectTag;
            }
        }
        */
    }

    
}
