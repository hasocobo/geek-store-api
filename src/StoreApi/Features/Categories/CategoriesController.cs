using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StoreApi.Common.DataTransferObjects.Categories;
using StoreApi.Entities;

namespace StoreApi.Features.Categories
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IServiceManager serviceManager, ILogger<CategoriesController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories()
        {
            var categories = await _serviceManager.CategoryService.GetCategoriesAsync();
            return Ok(categories);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> GetCategory(Guid id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            var categoryToReturn =
                await _serviceManager.CategoryService.CreateCategoryAsync(categoryCreateDto);
            
            return CreatedAtAction(nameof(GetCategory), new { id = categoryToReturn.Id }, categoryToReturn);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            await _serviceManager.CategoryService.UpdateCategoryAsync(id, categoryUpdateDto);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            await _serviceManager.CategoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}