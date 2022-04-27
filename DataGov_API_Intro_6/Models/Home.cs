using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataGov_API_Intro_6.Models
{


        

        public class Food_Item
        {
            public Food_Item(string description, List<Food_Nutrient> fn)
            {
            
                this.description = description;
                this.foodNutrients = fn;
            
            }

        
            public Food_Item()
            {

            }
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public Guid fdcId { get; set; }
            [Required]
            public string description { get; set; }
            
            public List<Food_Nutrient> foodNutrients { get; set; }

            
        }

        public class Food_Nutrient
        {
            public Food_Nutrient(float amt, string type)
            {
            
                this.nutrient.name = type;

                this.nutrient.nutrient_type = type;
                this.amount = amt;
            
            }

            public Food_Nutrient()
            {

            }
            //[DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key, Column(Order = 1)]
            public Guid fdcId { get; set; }
            //[DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key, Column(Order = 2)]
            public Guid number { get; set; }
            
            [Required]
            public float amount { get; set; }

            [ForeignKey("fdcId")]
            public Food_Item food_item { get; set; }
            [ForeignKey("number")]
            public Nutrient nutrient { get; set; }

        }

        public class Nutrient
        {

            public Nutrient()
            {

            }
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Key]
            public Guid number { get; set; }

            [Required]
            public string name { get; set; }

            public string unitName { get; set; }

            public string nutrient_type { get; set; }

            public List<Food_Nutrient> foodNutrients { get; set; }

        }
    
}
