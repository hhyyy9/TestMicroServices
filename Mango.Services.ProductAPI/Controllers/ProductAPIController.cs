using AutoMapper;
using Mango.Service.ProductAPI.Data;
using Mango.Service.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
	[ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
	{
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public ProductAPIController(AppDbContext appDbContext,IMapper autoMapper)
		{
            _appDbContext = appDbContext;
            _mapper = autoMapper;
            _responseDto = new();
        }

		[HttpGet]
		public ResponseDto Get()
		{
			try
			{
				var productList = _appDbContext.Products.ToList();
				_responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
			}

			return _responseDto;
		}

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product obj = _appDbContext.Products.First(u => u.ProductId == id);
                _responseDto.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _appDbContext.Products.Add(obj);
                _appDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto ProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(ProductDto);
                _appDbContext.Products.Update(obj);
                _appDbContext.SaveChanges();
                _responseDto.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product obj = _appDbContext.Products.First(u => u.ProductId == id);
                _appDbContext.Products.Remove(obj);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}

