using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//import syntax.Expression;

public class TruthValue : ISemanticValue 
{
    public enum T { True, False, Unknown }
    public enum UpdateInfo { Updated, NoChange, Warning }
    private T value;
    private UpdateInfo UInfo;

    public TruthValue()
    {
            value = T.Unknown;
    }

    public TruthValue(T value)
    {
            this.value = value;
    }

    public bool IsUnknown()
    {
        return value == T.Unknown;
    }

    public bool IsTrue()
    {
        return value == T.True;
    }

    public bool IsFalse()
    {
        return value == T.False;
    }


    public void Clear()
    {
        value = T.Unknown;
    }

    public UpdateInfo Add(bool val)
    {
        bool hasChanged = false;
        if (val)
        {
            if (value == T.Unknown)
            {
                value = T.True;
                hasChanged = true;
            }
            if (value == T.False)
            {
                return UpdateInfo.Warning;
            }
        }
        else
        {
            if (value == T.Unknown)
            {
                value = T.False;
                hasChanged = true;
            }
            if (value == T.True)
            {
                return UpdateInfo.Warning;
            }
        }
        return hasChanged ? UpdateInfo.Updated : UpdateInfo.NoChange;
    }

    public int GetID()
    {
        if (value == T.Unknown) return 0;
        if (value == T.True) return 1;
        else return 2;
    }

    public String toString()
    {
        if (value == T.Unknown) return "U";
        if (value == T.True) return "T";
        else return "F";
    }

    public UpdateInfo update(ISemanticValue that)
    {   
        if (!(that.GetType() == typeof(TruthValue)))
        {
            return UpdateInfo.NoChange;
        }
        TruthValue other = (TruthValue)that;
        if (other.IsTrue())
        {
            return Add(true);
        }
        if (other.IsFalse())
        {
            return Add(false);
        }
        return UpdateInfo.NoChange;
    }

    public ISemanticValue SClone()
    {
        return (ISemanticValue) new TruthValue(this.value);
    }

    public bool Update(ISemanticValue that)
    {
        throw new NotImplementedException();
    }
}