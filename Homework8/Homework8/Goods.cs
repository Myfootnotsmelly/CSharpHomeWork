﻿using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{
    //货物类 
   /* [Serializable]
    public enum string : int
    {
        wine, gun, fish
    }*/
    [Serializable]
    public class Goods
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }

        public Goods()
        {
            ID = Guid.NewGuid().ToString();
        }
        public Goods(string type) : this()
        {
            this.Type = type;
        }
        public Goods(string type, double price):this()
        {
            this.Type = type;
            this.Price = price;
        }
        public override string ToString()
        {
            return Type + "";
        }
        public override bool Equals(object obj)
        {
            Goods m = obj as Goods;
            return m != null && m.Type == Type;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
