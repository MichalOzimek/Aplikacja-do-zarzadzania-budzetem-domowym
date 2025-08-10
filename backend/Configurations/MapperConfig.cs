using AutoMapper;
using ProjectSoftwareWorkshop.Data;
using ProjectSoftwareWorkshop.Models.Category;
using ProjectSoftwareWorkshop.Models.Purchase;
using ProjectSoftwareWorkshop.Models.Shop;
using ProjectSoftwareWorkshop.Models.Account;
using ProjectSoftwareWorkshop.Models.Income;

namespace ProjectSoftwareWorkshop.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        // CreateMap<OneType, AnotherType>().ReverseMap();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        
        CreateMap<Shop, ShopDto>().ReverseMap();

        CreateMap<Purchase, PurchaseDto>().ReverseMap();
        CreateMap<Purchase, PurchaseCreateDto>().ReverseMap();

        CreateMap<Account, AccountDto>().ReverseMap();
        CreateMap<Income, IncomeDto>().ReverseMap();
    }
}