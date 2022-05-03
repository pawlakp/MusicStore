using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class CartIndexViewModel
    {
       
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
        public int CartCount { get; set; }
        public ShippingDetails ShippingDetails { get; set; }
        public decimal CartTotal { get; set; }
        public int DeleteId { get; set; }
    }
}