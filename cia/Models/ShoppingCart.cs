using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using PCLStorage;

using cia.Constants;

namespace cia.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long DateCreated { get; set; }


        #region pure getters

        public string ReceiptImageFilePath => Path.Combine(FileSystem.Current.LocalStorage.Path, Paths.ReceiptImagesFolder, Id.ToString());

        #endregion pure getters
    }
}
