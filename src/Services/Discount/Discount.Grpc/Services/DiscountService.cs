using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService 
    : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountContext _context;
    private readonly ILogger<DiscountService> _logger;
    
    public DiscountService(DiscountContext context, ILogger<DiscountService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    // OVERRIDE
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _context
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
        
        _logger.LogInformation("Discount is retrieved from Coupons. ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        // normalde buildingblocks'tan çekilebilirdi ancak grpc hızlı olması için içinde gerekli
        var couponModal = coupon.Adapt<CouponModel>();
        return couponModal;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Discount is successfully created. ProductName : {productName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        
        _context.Coupons.Update(coupon);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Discount is successfully updated. ProductName : {productName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _context
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName : {request.ProductName} does not exist"));
        
        _context.Coupons.Remove(coupon);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Discount is successfully deleted. ProductName : {productName}", coupon.ProductName);
        
        return new DeleteDiscountResponse { Success = true };
    }
}