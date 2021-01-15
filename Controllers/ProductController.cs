using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using ProductManagementApi.Services;

namespace ProductManagementApi.Controllers
{

    [ApiController]
    public class ProductController : ControllerBase
    {

        private ApplicationDbContext _context;


        public ProductController( ApplicationDbContext context)
        {
            _context = context;

        }


        [HttpGet("api/[controller]")]
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }


        [HttpGet("api/[controller]/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Select(x => x.Id == id);

            if (product == null)
                return NotFound();
            return Ok(product);
        }


        [HttpPost("api/[controller]/Create")]
        public ActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Some parameters are not well defined");

            _context.Products.Add(product);
            _context.SaveChanges();

            return Created(HttpContext.Request.Scheme + "//" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + product.Id, product);
        }


        [HttpPost("api/[controller]/Update/{id}")]
        public ActionResult UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Some parameters are not well defined");

            var productInDb = _context.Products.SingleOrDefault(x => x.Id == product.Id);

            if (productInDb == null)
                return NotFound("This product does not exist in this current database");

            productInDb.Name = product.Name;
            productInDb.Price = product.Price;
            productInDb.isDisabled = product.isDisabled;
            productInDb.DateCreated = product.DateCreated;

            _context.SaveChanges();

            return Ok(productInDb);
        }


        [HttpDelete("api/[controller]/Delete/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound("Product does not exist in the current database");

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok("Product deleted");
        }


        [HttpPut("api/[controller]/Disable/{id}")]
        public ActionResult DisableProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(x => x.Id == id);

            if (product == null)
                return NotFound("Product does not exist");

            if (product.isDisabled)
                return Ok("product is already disabled");

            product.isDisabled = !product.isDisabled;

            _context.SaveChanges();
            return Ok("Product Disabled");
        } 


        [HttpGet("api/[controller]/Sum")]
        public ActionResult ProductSum()
        {
            var productsPrice = _context.Products
                .Where(x => DateTime.Now.Day - x.DateCreated.Day <= 7)
                .Select(x => x.Price)
                .Sum();

            return Ok(productsPrice);
        }

        [HttpGet("api/[controller]/DisabledProducts")]
        public ActionResult DisabledProducts()
        {
            var disabledProducts = _context.Products
                .OrderByDescending(x => x.isDisabled == true);

            return Ok(disabledProducts);
        }
    }
}
