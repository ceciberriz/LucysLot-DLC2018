using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   // import syntax.Expression;

    public interface ISemanticValue
    {
        int GetID();
        bool Update(ISemanticValue that);
        ISemanticValue SClone();
    }
