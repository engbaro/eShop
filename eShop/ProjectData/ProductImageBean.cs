using eShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eShop.ProjectData
{
    public class ProductImageBean
    {
        public Product Product { get; set; }
        public ProductImage ProductImage { get; set; }
    }
}