using AutoMapper;
using BusinessLogic.AbstractLogic.Application;
using BusinessLogic.AbstractLogic.Product;
using BusinessLogic.Contracts;
using Context.Entities;
using DataTransferObjets.Configuration;
using DataTransferObjets.Dto;
using DataTransferObjets.Dto.In;
using DataTransferObjets.Dto.Out;
using DataTransferObjets.Dto.ViewsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Repository.GenericRepository.Interfaces;

namespace BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostEnvironment hostEnvironment;
        private readonly string relacionProperities = "Category,Mark";

        public ProductService(IMapper mapper, IUnitOfWork unitOfWork, IHostEnvironment hostEnvironment)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.hostEnvironment = hostEnvironment;
        }
        public async Task<bool> Add(ProductRequest requestDto, IFormFileCollection files, CancellationToken cancellationToken)
        {
            if (files.Count > 0)
            {
                string DirectoryPathImages = Path.Combine(hostEnvironment.ContentRootPath, $"wwwroot{StaticDefination.ImagenPath}");
                ProductLogic.CreateDirectoryImages(DirectoryPathImages);
                ResponseImagesDto response =  ProductLogic.SavePicture(new ImagesDto { Files = files, UploadPath = DirectoryPathImages });
                if (response.RequestResponse == true)
                    requestDto.ImageUrl = response.SavePath;

            }
            Product entity = mapper.Map<Product>(requestDto);
            await unitOfWork.ProductRepository.Create(entity, cancellationToken);
            int result = await unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            await unitOfWork.ProductRepository.Delete(id, cancellationToken);
            int result = await unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<IEnumerable<ProductResponse>> GetAll()
        {
            IEnumerable<Product?> data = await unitOfWork.ProductRepository.ReadAll();
            IEnumerable<ProductResponse> response = mapper.Map<IEnumerable<ProductResponse>>(data);
            return response;
        }

        public SelectViewItemViewModel GetAllDropdownList()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponse> GetById(int id)
        {
            Product? entity = await unitOfWork.ProductRepository.ReadById(x => x.Id.Equals(id), includeProperties: relacionProperities);
            ProductResponse responseDto = mapper.Map<ProductResponse>(entity);
            return responseDto;
        }

        public async Task<bool> Update(int id, IFormFileCollection files, ProductRequest requestDto, CancellationToken cancellationToken)
        {
            if (files.Count > 0)
            {
                string DirectoryPathImages = Path.Combine(hostEnvironment.ContentRootPath, $"wwwroot{StaticDefination.ImagenPath}");
                ProductLogic.CreateDirectoryImages(DirectoryPathImages);
                ProductLogic.EraseToWrite(DirectoryPathImages + requestDto.ImageUrl);
                ResponseImagesDto response = ProductLogic.SavePicture(new ImagesDto { Files = files, UploadPath = DirectoryPathImages });
                if (response.RequestResponse == true)
                    requestDto.ImageUrl = response.SavePath;

            }
            Product entity = mapper.Map<Product>(requestDto);
            await unitOfWork.ProductRepository.Update(id, entity, cancellationToken);
            int result = await unitOfWork.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> ValidateNameId(int id, string name)
        {
            IEnumerable<Product?> data = await unitOfWork.ProductRepository.ReadAll();
            IEnumerable<ProductResponse> response = mapper.Map<IEnumerable<ProductResponse>>(data);
            return GenericValidation.ValidateDuplicateNameField(response, id, name);
        }
    }
}
