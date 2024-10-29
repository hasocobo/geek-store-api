﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _serviceManager.CategoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] Category category)
        {
            await _serviceManager.CategoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
    }
}