using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Business;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        IProductDal _productDal;
        private ICategoryService _categoryService;
       

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;

        }

        //Claim
        //[SecuredOperation("admin,editor,product.add")]
        [ValidationAspect(typeof(ProductValidator))] //ValidationTool.Validate(new ProductValidator(), product);
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result= BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),CheckIfCategoryCountLimitCorrect());

            if (result!= null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);

            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            return new SuccessResult();
        }

        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            //Business codes
            //if (DateTime.Now.Hour == 23)
            //{
            //    return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            //}

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.ProductListed);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int CategoryId) //Aynı kategoride 10'dan fazla ürün olamaz.
        {
            var result = _productDal.GetAll(p => p.CategoryId == CategoryId).Count;
            if (result > 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);

            }

            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName) //Aynı isimde ürün eklenemez.
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);

            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryCountLimitCorrect() //Categori sayısı 15'ten fazla ise ürün eklenemez.
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }

    }
}
