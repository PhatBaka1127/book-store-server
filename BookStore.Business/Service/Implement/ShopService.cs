using System.IO.Compression;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Business.Dto;
using BookStore.Business.Helper;
using BookStore.Business.Service.Interface;
using BookStore.Data.Entity;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Business.Service.Implement
{
    public class ShopService : IShopService
    {
        private readonly IRepository<Shop> _shopRepository;
        private readonly IMapper _mapper;

        public ShopService(IRepository<Shop> shopRepository,
                            IMapper mapper)
        {
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<int>> CreateShopAsync(CreateShopRequest createShopRequest, ThisUserObj userObj)
        {
            if (userObj.role != (int)RoleEnum.SELLER)
                throw new ForbiddenException("Forbidden");

            var entity = _mapper.Map<Shop>(createShopRequest);
            entity.CreatedBy = userObj.userId;

            _shopRepository.Add(entity);
            await _shopRepository.SaveChangesAsync();

            return new ResponseMessage<int>()
            {
                message = "Create shop successfully",
                result = true,
                value = entity.Id
            };
        }

        public async Task<ResponseMessage<bool>> DeleteShopAsync(int id, ThisUserObj thisUserObj)
        {
            var exitedShop = await _shopRepository.FindAsync(id, isTracking: true);
            if (exitedShop == null)
                throw new NotFoundException("Shop not found");

            exitedShop.Status = 0;
            exitedShop.UpdatedDate = DateTime.UtcNow;
            exitedShop.UpdatedBy = thisUserObj.userId;

            _shopRepository.Update(exitedShop);
            await _shopRepository.SaveChangesAsync();

            return new ResponseMessage<bool>()
            {
                message = "Update shop successfully",
                result = true,
                value = true,
            };
        }

        public async Task<DynamicResponseModel<ShopResponse>> GetShopAsync(ShopResponse shopFilter, ThisUserObj userObj, PagingRequest pagingRequest)
        {
             var (total, data) = _shopRepository.GetTable()
                .DynamicFilter(shopFilter)
                .ProjectTo<ShopResponse>(_mapper.ConfigurationProvider)
                .PagingIQueryable(pagingRequest.page, pagingRequest.pageSize,
                                  PageConstant.LIMIT_PAGING, PageConstant.DEFAULT_PAPING);

            return new DynamicResponseModel<ShopResponse>
            {
                metaData = new MetaData
                {
                    page = pagingRequest.page,
                    size = pagingRequest.pageSize,
                    total = total
                },
                results = await data.ToListAsync()
            };
        }

        public async Task<ResponseMessage<ShopResponse>> GetShopByIdAsync(int id)
        {
            var existedShop = await _shopRepository.GetByIdAsync(id);
            if (existedShop == null)
                throw new NotFoundException("Shop not found");
            return new ResponseMessage<ShopResponse>()
            {
                message = "Shop found",
                result = true,
                value = _mapper.Map<ShopResponse>(existedShop)
            };
        }

        public async Task<ResponseMessage<bool>> UpdateShopByIdAsync(int id, ThisUserObj thisUserObj, UpdateShopRequest updateShopRequest)
        {
            var existedShop = await _shopRepository.GetByIdAsync(id);
            if (existedShop == null)
                throw new NotFoundException("Shop not found");
            if (existedShop.Users.Any(x => x.Id == thisUserObj.userId))
                throw new ForbiddenException("Forbidden");

            _mapper.Map(updateShopRequest, existedShop);
            existedShop.UpdatedBy = thisUserObj.userId;

            _shopRepository.Update(existedShop);
            await _shopRepository.SaveChangesAsync();

            return new ResponseMessage<bool>()
            {
                message = "Update sucessfully",
                result = true,
                value = true
            };
        }
    }
}