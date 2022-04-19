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
        FoodRoot foodRoot = new FoodRoot();

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
                    foodItems = JsonConvert.DeserializeObject<List<Food_Item>>(foodData);
                    
                    dynamic item = JsonConvert.DeserializeObject(foodData);
                    if (!item.Equals("") && !foodItems.Equals("") && item.Count > 0)
                    {

                        for (int i = 0; i < item.Count; i++)
                        {
                            if (dbContext.Food_Nutrients.Where(c => c.fdcId.Equals(food_Nutrient.fdcId)).Count() == 0)
                            {
                                food_Nutrient.fdcId = foodItems[i].fdcId;
                            }
                            foreach (var x in item[i].foodNutrients)
                            {
                                int id = x.number;
                                string name = x.name;
                                string unitName = x.unitName;
                                float amount = x.amount;
                                if (dbContext.Nutrients.Where(c => c.number.Equals(nutrient.number)).Count() == 0)
                                {
                                nutrient.number = id;
                                nutrient.name = name;
                                nutrient.unitName = unitName;
                                nutrients.Add(nutrient);
                                
                                    dbContext.Nutrients.Add(nutrient);
                                }
                                if (dbContext.Food_Nutrients.Where(c => c.fdcId.Equals(food_Nutrient.fdcId)).Count() == 0)
                                {
                                    food_Nutrient.number = id;
                                

                                food_Nutrient.nutrient = nutrient;
                                food_Nutrient.nutrient.number = id;
                                food_Nutrient.nutrient.name = name;
                                food_Nutrient.nutrient.unitName = unitName;
                                food_Nutrient.amount = amount;
                                foodNutrients.Add(food_Nutrient);
                                
                                    dbContext.Food_Nutrients.Add(food_Nutrient);
                                }
                            }
                            if (dbContext.Food_Items.Where(c => c.fdcId.Equals(foodItems[i].fdcId)).Count() == 0)
                            {
                                foodItems[i].foodNutrients = (foodNutrients);
                            }
                            if (dbContext.Food_Items.Where(c => c.fdcId.Equals(foodItems[i].fdcId)).Count() == 0)
                            {
                                for (int j = 0; j < foodNutrients.Count; j++)
                            {
                                foodItems[i].foodNutrients[j].nutrient = foodNutrients[j].nutrient;
                                foodItems[i].foodNutrients[j].nutrient.number = foodNutrients[j].nutrient.number;
                                foodItems[i].foodNutrients[j].nutrient.name = foodNutrients[j].nutrient.name;
                                foodItems[i].foodNutrients[j].nutrient.unitName = foodNutrients[j].nutrient.unitName;
                            }
                             dbContext.Food_Items.Add(foodItems[i]); }
                        }
                        dbContext.SaveChangesAsync();
                    }
                    
                    
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