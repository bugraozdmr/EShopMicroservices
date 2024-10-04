namespace BuildingBlocks.Pagination;

public record PaginationRequest(int pageindex = 0, int pagesize = 10);