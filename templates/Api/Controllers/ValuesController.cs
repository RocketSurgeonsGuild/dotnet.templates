using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Rocket.Surgery.Api
{
    [Route("/shapes")]
    public class PolymorphicTypesController
    {
        // [HttpPost]
        // public ActionResult<Shape> CreateShape(Shape shape)
        // {
        //     return shape;
        // }

        [HttpGet]
        public ActionResult<IEnumerable<Shape>> GetShapes()
        {
            return new Shape[] {
            new Rectangle()
            {
                Name = "Rect",
                Height = 10,
                Width = 20
            },
            new Circle()
            {
                Name = "Cir",
                Radius = 50
            }
            };
        }
    }

    public abstract class Shape
    {
        public string Name { get; set; }
    }

    public class Rectangle : Shape
    {
        public int Height { get; set; }

        public int Width { get; set; }
    }

    public class Circle : Shape
    {
        public int Radius { get; set; }
    }

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FromQueryParamsController
    {
        [HttpGet("addresses/validate")]
        public IActionResult ValidateAddress([FromQuery]Address address)
        {
            return new NoContentResult();
        }

        [HttpGet("zip-codes/validate")]
        public IActionResult ValidateZipCodes([FromQuery]IEnumerable<string> zipCodes)
        {
            return new NoContentResult();
        }
    }

    public class Address
    {
        /// <summary>
        /// 3-letter ISO country code
        /// </summary>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// Name of city
        /// </summary>
        [DefaultValue("Seattle")]
        public string City { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
