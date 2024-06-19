using OnlineAuc.Domain.Auctions;
using OnlineAuc.Models.Auctions;

namespace OnlineAuc.Services
{
    public interface IGuestService
    {
        Task AddClassicSegment(Guid id, string title, string colorHex, HttpContext httpContext);
        Task AddPointSegment(Guid id, string title, uint points, string colorHex, HttpContext httpContext);
        Task Create(CreateAuctionModel model, HttpContext httpContext);
        Task DeleteClassicSegment(Guid id, HttpContext httpContext);
        Task DeletePointSegment(Guid id, HttpContext httpContext);
        Task<List<AuctionItem>> GetAll(HttpContext httpContext);
        Task<ClassicAuction> GetClassic(Guid id, HttpContext httpContext);
        Task<PointAuction> GetPoint(Guid id, HttpContext httpContext);
        Task Remove(Guid id, AuctionType type, HttpContext httpContext);
        Task UpdateClassicWheelOptions(UpdateClassicAuctionOptionsModel model, HttpContext httpContext);
        Task UpdatePointWheelOptions(UpdatePointAuctionOptionsModel model, HttpContext httpContext);
    }
}
