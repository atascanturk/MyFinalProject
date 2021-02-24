using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Product added.";
        public static string ProductNameInvalid = "Invalid product name.";
        public static string ProductListed = "Product listed.";
        public static string MaintenanceTime = "Maintenance Time";
        public static string ProductCountOfCategoryError = "You cannot add any product as products are more than 10 in this category.";
        public static string ProductNameAlreadyExists = "There is a product which has same name.";

        public static string CategoryLimitExceded =
            "You cannot add any product as count of categories  are more than 15. ";
    }
}
