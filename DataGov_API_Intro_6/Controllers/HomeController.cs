using DataGov_API_Intro_6.Models;
using Microsoft.AspNetCore.Mvc;
using DataGov_API_Intro_6.DataAccess;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataGov_API_Intro_6.Controllers
{
    public class HomeController : Controller
    {
        HttpClient httpClient;
        //FoodRoot foodRoot = new FoodRoot();

        static string BASE_URL = "https://api.nal.usda.gov/fdc/v1";
        static string API_KEY = "aaXHJ2E5Mz6bPzI5rOs813sn7OW8U8N4Gz3BkT11"; //Add your API key here inside ""

        

        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        
        public async Task<IActionResult> Index()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string FOODS_LIST_API_PATH = BASE_URL + "/foods/list";
            string foodData = "";

            List<Food_Item> foodItems = new List<Food_Item>();
            List<int> nutrientids = new List<int>();
            List<Nutrient> nutrients = new List<Nutrient>();
            List<Food_Nutrient> foodNutrients = new List<Food_Nutrient>();
            //Food_Item foodItem = null;

            Food_Item foodItem = new Food_Item();
            Food_Nutrient food_Nutrient = new Food_Nutrient();
            Nutrient nutrient = new Nutrient();



            //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
            httpClient.BaseAddress = new Uri(FOODS_LIST_API_PATH);

            try
            {
                //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                //                                        .GetAwaiter().GetResult();
                HttpResponseMessage response = httpClient.GetAsync(FOODS_LIST_API_PATH)
                                                        .GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    foodData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!foodData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    //foodItems = JsonConvert.DeserializeObject<List<Food_Item>>(foodData);
                    
                    dynamic item = JsonConvert.DeserializeObject(foodData);
                    if (!item.Equals("") && item.Count > 0)
                    {
                        //nutrientids = dbContext.Nutrients.Select(nutrient => nutrient.number).ToList();
                        for (int i = 0; i < item.Count; i++)
                        {
                            if (item[i].description != null)
                            {
                                foodItem.description = item[i].description;
                                foodItem.fdcId = Guid.NewGuid();
                                
                                //int foodid = item[i].fdcId;
                                if (dbContext.Food_Items.ToList().Count==0 || 
                                    ((dbContext.Food_Items.ToList().Count != 0) && (dbContext.Food_Items.ToList().Where(p => p.description == foodItem.description).ToList().Count == 0)))
                                {
                                    dbContext.AddRange(foodItem);
                                    await dbContext.SaveChangesAsync();
                                }

                                //food_Nutrient.fdcId = foodItem.fdcId;

                                foreach (var x in item[i].foodNutrients)
                                {
                                    if (x.number == "203" || x.number == "205" || x.number == "204"
                                        || x.number == "269" || x.number == "208")
                                    {
                                        int id = x.number;
                                        string name = x.name;
                                        string unitName = x.unitName;
                                        float amount = x.amount;
                                        switch (id)
                                        {
                                            case 203:
                                                nutrient.nutrient_type = "Protein";
                                                break;
                                            case 205:
                                                nutrient.nutrient_type = "Carbohydrate";
                                                break;
                                            case 204:
                                                nutrient.nutrient_type = "Fat";
                                                break;
                                            case 269:
                                                nutrient.nutrient_type = "Sugar";
                                                break;
                                            default:
                                                nutrient.nutrient_type = "Energy";
                                                break;
                                        }


                                        nutrient.number = Guid.NewGuid();
                                        nutrient.name = name;
                                        nutrient.unitName = unitName;
                                        nutrients.Add(nutrient);

                                        if (dbContext.Nutrients.ToList().Count == 0 ||
                                    ((dbContext.Nutrients.ToList().Count != 0) && (dbContext.Nutrients.ToList().Where(p => p.name == nutrient.name).ToList().Count == 0)))
                                        {
                                            dbContext.Add(nutrient);
                                            await dbContext.SaveChangesAsync();
                                        }

                                        
                                        food_Nutrient = new Food_Nutrient { fdcId=foodItem.fdcId,
                                            number=nutrient.number,amount = amount, food_item = foodItem, 
                                            nutrient=new Nutrient { nutrient_type = nutrient.nutrient_type, name = nutrient.name, unitName = nutrient.unitName } };
                                        foodNutrients.Add(food_Nutrient);

                                        

                                        if (dbContext.Food_Nutrients.ToList().Count == 0 ||
                                    ((dbContext.Food_Nutrients.ToList().Count != 0) && (dbContext.Food_Nutrients.ToList().Where(p => p.nutrient.name == food_Nutrient.nutrient.name &&
                                        p.food_item.description == food_Nutrient.food_item.description).ToList().Count == 0)))
                                        {
                                            dbContext.Add(food_Nutrient);
                                            await dbContext.SaveChangesAsync();
                                        }

                                        nutrient = new Nutrient();
                                        

                                    }
                                }
                                
                                foodItem.foodNutrients = foodNutrients;
                                foodItems.Add(foodItem);
                                dbContext.UpdateRange(foodNutrients);
                                dbContext.UpdateRange(foodItem);
                                nutrients = new List<Nutrient>();
                                foodNutrients = new List<Food_Nutrient>();
                                foodItem = new Food_Item();
                            }
                        }
                        



                    }

                 
                    
                    
                   

                    await dbContext.SaveChangesAsync();
                    }
                    
                    
                

                
               // await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return View("Index",foodItems);
        }

        //Create Record
        public IActionResult CreateRecord(string food_name, float prot,
                                              float carb, float fat, float sugar, float energy, string search)
        {
            try
            {
                
                string[] nutrienttype = new string[] { "Protein", "Carbohydrate", "Fat", "Sugar", "Energy" };
                float[] nutrientamt = new float[] { prot, carb, fat, sugar, energy };
                string[] nutrientunit = new string[] { "G", "G", "G", "G", "KCAL" };
                var nres = dbContext.Food_Items.Where(x => x.description.ToLower() == food_name.ToLower())
                        .Select(x => new Food_Item(x.description, x.foodNutrients)).ToList();
                if (nres.Count==0)
                {
                    List<Food_Nutrient> fn = new List<Food_Nutrient>();
                    for (int i = 0; i < nutrienttype.Length; i++)
                    {
                        Nutrient nutrient = new Nutrient();
                        nutrient.name = nutrienttype[i];
                        nutrient.unitName = nutrientunit[i];
                        nutrient.nutrient_type = nutrienttype[i];
                        Food_Nutrient foodnut = new Food_Nutrient();
                        foodnut.nutrient = nutrient;
                        foodnut.amount = nutrientamt[i];

                        fn.Add(foodnut);

                    }
                    if (food_name != null)
                    {
                        Food_Item cr = new Food_Item(food_name, fn)
                        {
                            description = food_name,
                            foodNutrients = fn

                        };
                        dbContext.Food_Items.Add(cr);
                        dbContext.SaveChanges();
                        ViewBag.Message = String.Format("Food Record Added ");
                    }
                }
                
                if (search != null)
                {
                    SearchRecord(search);
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message.
                Console.WriteLine(e.Message);
            }
            return View();
        }



        //READ
        public IActionResult SearchRecord(string search)
        {
            try
            {
                
                
                if (search != null)
                {
                    var nres = dbContext.Food_Items.Where(x => x.description.ToLower().Contains(search.Trim().ToLower())).
                        Select(x => new Food_Item(x.description, x.foodNutrients)).ToList();
                    var res2 = dbContext.Food_Nutrients.Join(dbContext.Nutrients,
                        a => a.number, b => b.number, (a, b) => new { b.nutrient_type,a.number, b.unitName}).Distinct()
                        .ToList();


                    if (nres.Count != 0)
                    {
                        List<string> foodnames = new List<string>();
                        List<Food_Item> finalFi = new List<Food_Item>();
                        for (int j = 0; j < nres.Count; j++)
                        {

                            var food_name = nres[j].description;
                            var food_nutr = nres[j].foodNutrients;

                            List<Food_Nutrient> fn = new List<Food_Nutrient>();
                            for (int i = 0; i < food_nutr.Count; i++)
                            {
                                var nutdet = res2.Find(item => item.number == food_nutr[i].number);
                                if (nutdet != null)
                                {
                                    Nutrient nutrient = new Nutrient();
                                    nutrient.name = nutdet.nutrient_type;
                                    nutrient.unitName = nutdet.unitName;
                                    nutrient.nutrient_type = nutdet.nutrient_type;
                                    Food_Nutrient foodnut = new Food_Nutrient();
                                    foodnut.nutrient = nutrient;
                                    foodnut.amount = food_nutr[i].amount;

                                    fn.Add(foodnut);
                                }

                            }
                            Food_Item cr = new Food_Item(food_name, fn)
                            {
                                description = food_name,
                                foodNutrients = fn

                            };
                            foodnames.Add(food_name);
                            finalFi.Add(cr);
                            
                           
                            ViewBag.FoodNames = String.Join(",", foodnames.Select(d => d));

                        }
                        return View(finalFi);

                    }


                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message.
                Console.WriteLine(e.Message);
            }
            return View();
        }

        //UPDATE
        public IActionResult UpdateRecords(string food_name, float prot,
                                            float carb, float fat, float sug, float eng)
        {
            try
            {
                if (food_name != null)
                {
                    var res1 = dbContext.Food_Items.Where(x => x.description == food_name).FirstOrDefault();

                    if (res1 != null)
                    {

                        float[] nutrientamt = new float[] { prot, carb, fat, sug, eng };
                        List<Food_Nutrient> fn = new List<Food_Nutrient>();
                        for (int i = 0; i < nutrientamt.Length; i++)
                        {

                            string temp = res1.foodNutrients[i].nutrient.nutrient_type;

                            switch (temp)
                            {
                                case "Protein":
                                    fn[i].amount = prot;
                                    break;
                                case "Carbohydrate":
                                    fn[i].amount = carb;
                                    break;
                                case "Fat":
                                    fn[i].amount = fat;
                                    break;
                                case "Sugar":
                                    fn[i].amount = sug;
                                    break;
                                default:
                                    fn[i].amount = eng;
                                    break;
                            }


                        }
                        res1.foodNutrients = fn;
                        dbContext.SaveChanges();
                        ViewBag.Message = String.Format("Nutrient values Updated for the food item " + food_name);

                    }
                    return View(res1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View();
        }

        //DELETE
        public IActionResult DeleteRecords(string food_name)
        {
            try
            {
                if (food_name != null)
                {

                    Food_Item cr3 = new Food_Item();
                    cr3 = dbContext.Food_Items.Where(x => x.description == food_name).FirstOrDefault();
                    if (cr3 != null)
                    {
                        dbContext.Food_Items.Remove(cr3);
                        dbContext.SaveChanges();
                        ViewBag.Message = String.Format("Food Item Record for "+ food_name +" is Deleted");

                    }

                    return View(cr3);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View();
        }

        //Analyze Record
        public IActionResult Analyze()
        {
            try
            {

                var nres  = dbContext.Food_Items.Select(x => new Food_Item(x.description, x.foodNutrients)).ToList();
                var res2 = dbContext.Food_Nutrients.Join(dbContext.Nutrients,
                        a => a.number, b => b.number, (a, b) => new { b.nutrient_type, a.number, b.unitName }).Distinct()
                        .ToList();
                if (nres.Count != 0)
                {
                    List<string> foodnames = new List<string>();
                    List<Food_Item> finalFi = new List<Food_Item>();
                    for (int j=0;j< nres.Count;j++)
                    {

                        var food_name = nres[j].description;
                        var food_nutr = nres[j].foodNutrients;
                        
                        List<Food_Nutrient> fn = new List<Food_Nutrient>();
                        for (int i = 0; i < food_nutr.Count; i++)
                        {
                            var nutdet = res2.Find(item => item.number == food_nutr[i].number);
                            if (nutdet != null) {
                                Nutrient nutrient = new Nutrient();
                                nutrient.name = nutdet.nutrient_type;
                                nutrient.unitName = nutdet.unitName;
                                nutrient.nutrient_type = nutdet.nutrient_type;
                                Food_Nutrient foodnut = new Food_Nutrient();
                                foodnut.nutrient = nutrient;
                                foodnut.amount = food_nutr[i].amount;

                                fn.Add(foodnut);
                            }

                        }
                        Food_Item cr = new Food_Item(food_name, fn)
                        {
                            description = food_name,
                            foodNutrients = fn

                        };
                        foodnames.Add(food_name);
                        finalFi.Add(cr);
                        
                        
                        ViewBag.FoodNames = String.Join(",", foodnames.Select(d => d));

                    }
                    return View(finalFi);
                
                }
                
                
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message.
                Console.WriteLine(e.Message);
            }
            return View();
        }


        //Fetching the view of AboutUs
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}




public class Rootobject
{
    public Meta meta { get; set; }
    public object[][] data { get; set; }
}

public class Meta
{
    public View view { get; set; }
}

public class View
{
    public string id { get; set; }
    public string name { get; set; }
    public string assetType { get; set; }
    public string attribution { get; set; }
    public string attributionLink { get; set; }
    public int averageRating { get; set; }
    public string category { get; set; }
    public int createdAt { get; set; }
    public string description { get; set; }
    public string displayType { get; set; }
    public int downloadCount { get; set; }
    public bool hideFromCatalog { get; set; }
    public bool hideFromDataJson { get; set; }
    public string licenseId { get; set; }
    public bool newBackend { get; set; }
    public int numberOfComments { get; set; }
    public int oid { get; set; }
    public string provenance { get; set; }
    public bool publicationAppendEnabled { get; set; }
    public int publicationDate { get; set; }
    public int publicationGroup { get; set; }
    public string publicationStage { get; set; }
    public int rowsUpdatedAt { get; set; }
    public string rowsUpdatedBy { get; set; }
    public int tableId { get; set; }
    public int totalTimesRated { get; set; }
    public int viewCount { get; set; }
    public int viewLastModified { get; set; }
    public string viewType { get; set; }
    public Approval[] approvals { get; set; }
    public Column[] columns { get; set; }
    public Grant[] grants { get; set; }
    public License license { get; set; }
    public Metadata metadata { get; set; }
    public Owner owner { get; set; }
    public Query query { get; set; }
    public string[] rights { get; set; }
    public Tableauthor tableAuthor { get; set; }
    public string[] tags { get; set; }
    public string[] flags { get; set; }
}

public class License
{
    public string name { get; set; }
    public string termsLink { get; set; }
}

public class Metadata
{
    public Custom_Fields custom_fields { get; set; }
    public string[] availableDisplayTypes { get; set; }
}

public class Custom_Fields
{
    public DataQuality DataQuality { get; set; }
    public CommonCore CommonCore { get; set; }
}

public class DataQuality
{
    public string UpdateFrequency { get; set; }
    public string GeographicCoverage { get; set; }
}

public class CommonCore
{
    public string ContactEmail { get; set; }
    public string Footnotes { get; set; }
    public string ContactName { get; set; }
    public string ProgramCode { get; set; }
    public string Publisher { get; set; }
    public string BureauCode { get; set; }
}

public class Owner
{
    public string id { get; set; }
    public string displayName { get; set; }
    public string screenName { get; set; }
    public string type { get; set; }
    public string[] flags { get; set; }
}

public class Query
{
}

public class Tableauthor
{
    public string id { get; set; }
    public string displayName { get; set; }
    public string screenName { get; set; }
    public string type { get; set; }
    public string[] flags { get; set; }
}

public class Approval
{
    public int reviewedAt { get; set; }
    public bool reviewedAutomatically { get; set; }
    public string state { get; set; }
    public int submissionId { get; set; }
    public string submissionObject { get; set; }
    public string submissionOutcome { get; set; }
    public int submittedAt { get; set; }
    public int workflowId { get; set; }
    public Submissiondetails submissionDetails { get; set; }
    public Submissionoutcomeapplication submissionOutcomeApplication { get; set; }
    public Submitter submitter { get; set; }
}

public class Submissiondetails
{
    public string permissionType { get; set; }
}

public class Submissionoutcomeapplication
{
    public int endedAt { get; set; }
    public int failureCount { get; set; }
    public int startedAt { get; set; }
    public string status { get; set; }
}

public class Submitter
{
    public string id { get; set; }
    public string displayName { get; set; }
}

public class Column
{
    public int id { get; set; }
    public string name { get; set; }
    public string dataTypeName { get; set; }
    public string fieldName { get; set; }
    public int position { get; set; }
    public string renderTypeName { get; set; }
    public Format format { get; set; }
    public string[] flags { get; set; }
    public string description { get; set; }
    public int tableColumnId { get; set; }
    public Cachedcontents cachedContents { get; set; }
}

public class Format
{
    public string view { get; set; }
    public string align { get; set; }
}

public class Cachedcontents
{
    public string non_null { get; set; }
    public object largest { get; set; }
    public string _null { get; set; }
    public Top[] top { get; set; }
    public object smallest { get; set; }
    public string cardinality { get; set; }
}

public class Top
{
    public object item { get; set; }
    public string count { get; set; }
}

public class Grant
{
    public bool inherited { get; set; }
    public string type { get; set; }
    public string[] flags { get; set; }
}