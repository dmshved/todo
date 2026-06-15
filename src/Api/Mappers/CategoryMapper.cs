using ToDo.Api.Dtos;
using ToDo.Api.Entities;

namespace ToDo.Api.Mappers;

public static class CategoryMapper
{
    public static Category ToEntity(this CreateCategoryDto dto, string userId)
    {
        return new Category
        {
            Name = dto.Name,
            UserId = userId
        };
    }
    
    public static CategoryDto ToDto(this Category entity)
    {
        return new CategoryDto(
            entity.Id,
            entity.Name, 
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.UserId
        );
    }

    public static void UpdateFrom(this Category entity, UpdateCategoryDto dto)
    {
        entity.Name = dto.Name;
    }
}