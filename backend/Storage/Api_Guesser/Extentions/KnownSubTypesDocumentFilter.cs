using DataLayer_Guesser.AdditionalModels;
using DataLayer_Guesser.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Shared_Guesser.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace Api_Guesser.Extentions
{
    public class KnownSubTypesDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            context.SchemaGenerator.GenerateSchema(typeof(MessageTypes), context.SchemaRepository);
            context.SchemaGenerator.GenerateSchema(typeof(UserStatus), context.SchemaRepository);
            context.SchemaGenerator.GenerateSchema(typeof(UserType), context.SchemaRepository);
        }
    }

    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                var a = Enum.GetNames(context.Type).ToList();
                for (int i = 0; i < a.Count; i++)
                {
                    model.Enum.Add(new OpenApiString(a[i] + " = " + i));
                }
            }
        }
    }
}
