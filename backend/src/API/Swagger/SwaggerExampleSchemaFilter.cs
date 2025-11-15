using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfluencerMarketplace.API.Swagger
{
    public class SwaggerExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
                return;

            // Add examples for common types
            foreach (var property in schema.Properties)
            {
                if (property.Value.Type == "string")
                {
                    switch (property.Key.ToLower())
                    {
                        case "email":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("user@example.com");
                            break;
                        case "password":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("SecurePassword123!");
                            break;
                        case "firstname":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("John");
                            break;
                        case "lastname":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("Doe");
                            break;
                        case "phonenumber":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("+977-9841234567");
                            break;
                        case "title":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("Summer Campaign 2024");
                            break;
                        case "description":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("Promote our new product line through social media");
                            break;
                        case "currency":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("NPR");
                            break;
                        case "paymentmethod":
                            property.Value.Example = new Microsoft.OpenApi.Any.OpenApiString("Wallet");
                            break;
                    }
                }
                else if (property.Value.Type == "number" && property.Key.ToLower().Contains("amount"))
                {
                    property.Value.Example = new Microsoft.OpenApi.Any.OpenApiDouble(10000.00);
                }
                else if (property.Value.Type == "number" && property.Key.ToLower().Contains("budget"))
                {
                    property.Value.Example = new Microsoft.OpenApi.Any.OpenApiDouble(50000.00);
                }
                else if (property.Value.Type == "integer" && property.Key.ToLower().Contains("followers"))
                {
                    property.Value.Example = new Microsoft.OpenApi.Any.OpenApiInteger(50000);
                }
            }
        }
    }
}

