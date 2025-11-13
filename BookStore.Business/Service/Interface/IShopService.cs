using BookStore.Business.Dto;
using BookStore.Business.Helper;

namespace BookStore.Business.Service.Interface
{
    public interface IShopService
    {
        // CREATE
        public Task<ResponseMessage<int>> CreateShopAsync(CreateShopRequest createShopRequest, ThisUserObj userObj);

        // READ
        public Task<ResponseMessage<ShopResponse>> GetShopByIdAsync(int id);
        public Task<DynamicResponseModel<ShopResponse>> GetShopAsync(ShopFilter shopFilter, ThisUserObj userObj, PagingRequest pagingRequest);

        // UPDATE
        public Task<ResponseMessage<bool>> UpdateShopByIdAsync(int id, ThisUserObj thisUserObj, UpdateShopRequest updateShopRequest);

        // DELETE
        public Task<ResponseMessage<bool>> DeleteShopAsync(int id, ThisUserObj thisUserObj);
    }
}