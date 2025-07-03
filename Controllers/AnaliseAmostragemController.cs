using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using AgroTechSafra.API.Models;

namespace AgroTechSafra.API.Controllers
{
    [ApiController]
    [Route("api/analiseamostragem/upload")]
    public class AnaliseAmostragemController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var prompt = "Realize uma análise de amostragem de coleta em campo, com intuito de identificação de praga que está atacando a cultura mencionada nos dados. Envie a possível praga que está atacando a plantacão, com seu nome popular e nome científico e uma imagem da praga. ";

            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    BadDataFound = null,
                    MissingFieldFound = null
                }))
                {
                    var records = csv.GetRecordsAsync<AmostragemPragasCsvModel>();

                    await foreach (var record in records)
                    {
                        Console.WriteLine($"Plantacão: {record.NomeCultura}");
                    }

                }
            }
            return Ok();
        }
    }
}
