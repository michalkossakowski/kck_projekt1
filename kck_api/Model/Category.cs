using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kck_api.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category() { }

        public Category(string name)
        {
            this.Name = name;
        }
    }
}
