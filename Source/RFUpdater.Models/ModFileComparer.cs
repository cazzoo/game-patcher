using System;
using System.Collections.Generic;

namespace RFUpdater.Models
{
    public class ModFileComparer : IEqualityComparer<ModFile>
    {
        public bool Equals(ModFile x, ModFile y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.FileName == y.FileName && x.FilePath == y.FilePath && x.FileHash == y.FileHash;
        }

        public int GetHashCode(ModFile obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashFileName = obj.FileName == null ? 0 : obj.FileName.GetHashCode();

            //Get hash code for the Code field.
            int hashFileHash = obj.FileHash.GetHashCode();

            //Calculate the hash code for the product.
            return hashFileName ^ hashFileHash;
        }
    }
}