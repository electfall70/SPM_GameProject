using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttributeSystem : MonoBehaviour
{

    [SerializeField] private Text health;
    [SerializeField] private Text stamina;

    
    [SerializeField] private List<Attribute> activeAttributes = new List<Attribute>();
    [SerializeField] private List<Effect> activeEffects = new List<Effect>();
    //[SerializeField] private Effect.EFFECT_TAG effectTag = Effect.EFFECT_TAG.none;

    
    private Dictionary<Attribute.ATTRIBUTE_TYPE, Attribute> attributeByType = new Dictionary<Attribute.ATTRIBUTE_TYPE, Attribute>();
    private float stunTimer;

    private void Awake()
    {
        foreach (Attribute a in activeAttributes)
            attributeByType.Add(a.AttributeType, a);
    }

    /*Oklart om den h�r beh�vs??*/
    public void ApplyAttribute(Attribute.ATTRIBUTE_TYPE type, float value)
    {
        /*
        if (attributeByType[type])
            attributeByType[type].AttributeValue += value;
        */
        foreach(Attribute a in activeAttributes)
        {
            if (a.AttributeType == type)
                a.AttributeValue += value;
        }
        
    }

    public void ApplyEffect(Effect effect)
    {
        /*
        if (attributeByType[effect.AttributeType])
        {
            attributeByType[effect.AttributeType].AttributeValue = effect.AttributeValue;
        }
        */
        
        foreach (Attribute a in activeAttributes)
        {
            if (a.AttributeType == effect.AttributeType)
                a.AttributeValue = effect.AttributeValue;
        }
        
    }

    public void ActivateEffect(Effect effect)
    {
        activeEffects.Add(effect);
    }

    public float GetAttributeValue(Attribute.ATTRIBUTE_TYPE type)
    {
        return attributeByType[type].AttributeValue;
    }

    /*On Reload and checkpoint*/
    public void ResetAttributes()
    {
        /*
         foreach (var a in attributeByType.Keys)
            attributeByType[a].AttributeValue = attributeByType[a].MaxValue;
         */
        foreach (var a in activeAttributes)
            a.AttributeValue = a.MaxValue;
    }

    private void Update()
    {


        health.text = ((int)activeAttributes[0].AttributeValue).ToString();
        stamina.text = ((int)activeAttributes[1].AttributeValue).ToString();

        //Apply effects in activeEffects
        for(int i = 0; i<activeEffects.Count; i++)
        {
            switch (activeEffects[i].DurationType)
            {
                //Instant Effects like damage on Hit
                case Effect.EFFECT_DURATION_TYPE.instant:
                    ApplyEffect(activeEffects[i]);
                    activeEffects.Remove(activeEffects[i]);
                    break;
                
                //temporaryEffects like Stun
                case Effect.EFFECT_DURATION_TYPE.overtime:
                    activeEffects[i].Duration -= Time.deltaTime;
                    if (activeEffects[i].Duration > 0)
                        ApplyEffect(activeEffects[i]);
                    else
                        activeEffects.Remove(activeEffects[i]);
                    break;

                //Permanent FX like staminaRegen
                case Effect.EFFECT_DURATION_TYPE.permanent:
                    ApplyEffect(activeEffects[i]);
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
