using System.Collections.Generic;

namespace Cleverbit.Case.Business
{
    public class RegionAncestor
    {
        public int Id { get; set; }
        public RegionAncestor Parent { get; set; }

        public IList<int> GetAncestorIds()
        {
            IList<int> result = Parent != null
                                    ? Parent.GetAncestorIds()
                                    : new List<int>();

            result.Add(Id);

            return result;
        }
    }




}
