using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kck_api.Model
{
    public class CategoryModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public CategoryModel() { }

        public CategoryModel(string name)
        {
            this.Name = name;
        }
    }
}
