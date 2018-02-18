using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual { //: ISemanticValue {

    private int id;
    //private Expression name;

    public Individual(int id)
    {
        this.id = id;

    }

    public int GetID()
    {
        return id;
    }

    public int HashCode()
    {
        return id;
    }

    //public void SetName(Expression e)
    //{
      //  this.name = e;
    //}

    public bool Equals(System.Object o)
    {
        return (o.GetType() == typeof(Individual)) && (((Individual)o).id == this.id);
    }

    public bool Update(ISemanticValue that)
    {
        return false;
    }
//
//    public ISemanticValue sClone()
//    {
//        //return new Individual(id);
//    }
}





//import syntax.Expression;

