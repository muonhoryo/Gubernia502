using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class hitPointSystem : MonoBehaviour
{
    public takingDamageObjData takingDamageObjData;
    protected List<hitPointSystem> hunters = new List<hitPointSystem> { };
    public virtual int shieldDurability { get; set; } = 0;
    public int hitPoint=0;
    protected virtual void death(float rotation)
    {
        if (hunters.Count > 0)
        {
            for (int i = 0; i < hunters.Count; i++)
            {
                hunters[i].targetEliminated();
            }
        }
    }
    abstract public void takeExplosiveDamage(int dmg, int rezist,float bulletAngle,Vector3 hitPos);
    abstract public void takeNormalDamage(int dmg, int rezist,float bulletAngle, Vector3 hitPos);
    abstract public void takeNormalDamage(int dmg, float bulletAngle, Vector3 hitPos);
    abstract public void takeExplosiveDamage(int dmg, float bulletAngle, Vector3 hitPos);
    protected void spawnPerpendicularEffectOnCube(float bulletAngle,Vector3 hitPos,GameObject effect)
    {
        float hitAngle = Gubernia502.angleFromDirection((new Vector3(hitPos.x, transform.position.y, hitPos.z) - transform.position).normalized);
        if ((hitAngle + transform.eulerAngles.y) % 45 == 0 || (bulletAngle + transform.eulerAngles.y) % 90 == 0)
        {
            Instantiate(effect, hitPos, Quaternion.Euler(0, (bulletAngle + 180) % 360, 0));
        }
        else
        {
            float axis;
            if (hitAngle < 45 || hitAngle > 315)
            {
                axis = transform.eulerAngles.y;
            }
            else
            {
                axis = (float)(((int)(hitAngle - transform.eulerAngles.y + 45)) / 90) * 90 + transform.eulerAngles.y;
            }
            Instantiate(effect, hitPos, Quaternion.Euler(0, axis, 0));
        }
    }
    protected void spawnReflectedEffectOnCube(float bulletAngle, Vector3 hitPos,GameObject effect)
    {
        float hitAngle = Gubernia502.angleFromDirection((new Vector3(hitPos.x, transform.position.y, hitPos.z) - transform.position).normalized);
        if ((hitAngle+transform.eulerAngles.y) % 45 == 0||(bulletAngle+transform.eulerAngles.y)%90==0)
        {
            Instantiate(effect, hitPos, Quaternion.Euler(0, (bulletAngle+180)%360, 0));
        }
        else
        {
            float axis;
            float newAngle;
            if (hitAngle < 45 || hitAngle > 315)
            {
                axis = transform.eulerAngles.y;
            }
            else
            {
                axis = (float)(((int)(hitAngle - transform.eulerAngles.y + 45)) / 90) * 90 + transform.eulerAngles.y;
            }
            newAngle = axis * 2 - (bulletAngle + 180) % 360;
            if (newAngle < 0)
            {
                newAngle += 360;
            }
            Instantiate(effect, hitPos, Quaternion.Euler(0, newAngle, 0));
        }
    }
    protected void spawnReflectedEffectOnCapsule(float bulleAngle,Vector3 hitPos,GameObject effect)
    {
        Instantiate(effect, new Vector3(hitPos.x, transform.position.y, hitPos.z), Quaternion.Euler(0, (bulleAngle + 180) % 360, 0));
    }
    public virtual void targetEliminated()
    {

    }
    public virtual void becameTarget(hitPointSystem hunter)
    {
        hunters.Add(hunter);
    }
    public virtual void huntEnd(hitPointSystem hunter)
    {
        if (hunters.Contains(hunter))
        {
            hunters.Remove(hunter);
        }
    }
}
