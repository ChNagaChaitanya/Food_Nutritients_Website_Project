using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataGov_API_Intro_6.Models
{


        public class FoodRoot
        {
            [Key]
            public Guid id { get; set; }
            public List<Food_Item> foodItems { get; set; }
        }

        public class Food_Item
        {
            public Food_Item(string description)
            {
            
                this.description = description;
            
            }

        
            public Food_Item()
            {

            }
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key]
            public int fdcId { get; set; }
            public string description { get; set; }

            public List<Food_Nutrient> foodNutrients { get; set; }

            
        }

        public class Food_Nutrient
        {
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key]
            public int fdcId { get; set; }
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key]
            public int number { get; set; }
            public float amount { get; set; }

            
            public Food_Item food_item { get; set; }
            public Nutrient nutrient { get; set; }

        }

        public class Nutrient
        {
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key]
            public int number { get; set; }
            public string name { get; set; }

            public string unitName { get; set; }

            public string nutrient_type { get; set; }

            public List<Food_Nutrient> foodNutrients { get; set; }

        }
    
}
