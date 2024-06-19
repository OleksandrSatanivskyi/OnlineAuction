using OnlineAuc.Domain.Auctions;
using OnlineAuc.Models.Auctions;

namespace OnlineAuc.Services
{
    public interface IAuctionService
    {
        Task Create(CreateAuctionModel model);
        Task<List<AuctionItem>> GetAll(Guid userId);
        Task Remove(Guid id, AuctionType type);
        Task<ClassicAuction> GetClassic(Guid id);
        Task<PointAuction> GetPoint(Guid id);
        Task AddPointSegment(Guid wheelId, string title, uint points, string colorHex = "");
        Task DeletePointSegment(Guid id);
        Task UpdatePointWheelOptions(UpdatePointAuctionOptionsModel model);
        Task AddClassicSegment(Guid wheelId, string title, string colorHex);
        Task DeleteClassicSegment(Guid id);
        Task UpdateClassicWheelOptions(UpdateClassicAuctionOptionsModel model);
    }
}
