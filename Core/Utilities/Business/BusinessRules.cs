using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }

            return null;
        }

        //public static List<IResult> Run1(params IResult[] logics)  //List ile de yapılabilir.
        //{
        //    List<IResult> results = new List<IResult>();
        //    foreach (var logic in logics)
        //    {
        //        if (!logic.Success)
        //        {
        //            results.Add(logic);
        //        }
        //    }

        //    return results;
        //}
    }
}
