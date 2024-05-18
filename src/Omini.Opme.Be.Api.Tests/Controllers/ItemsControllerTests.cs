using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Be.Api.Dtos;
using Omini.Opme.Be.Api.Tests.Extensions;

namespace Omini.Opme.Be.Api.Tests.Controllers;

public class ItemsControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreateItem_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateDto().Generate();

        //act
        var response = await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem);
        var itemOutputDto = (await response.GetJsonAsync<ResponseDto<ItemOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakeItem.Should().BeEquivalentTo(itemOutputDto,
            options =>
                options.Excluding(p => p.Id)
        );
    }

    [Fact]
    public async void Should_UpdateItem_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateDto().Generate();

        //act
        var item = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        var itemUpdateDto = ItemFaker.GetFakerItemUpdateDto().Generate();
        itemUpdateDto.Id = item.Id;

        var updateItemResponse = await TestClient.Request($"/api/items/{item.Id}").AsAuthenticated().PutJsonAsync(itemUpdateDto);
        var itemAfterUpdate = (await TestClient.Request($"/api/items/{item.Id}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        //assert
        updateItemResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        itemAfterUpdate.Should().BeEquivalentTo(itemUpdateDto,
            options => options.Excluding(p => p.AnvisaDueDate));
    }

    [Fact]
    public async void Should_DeleteItem_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateDto().Generate();

        //act
        var item = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        var deleteItemResponse = await TestClient.Request($"/api/items/{item.Id}").AsAuthenticated().DeleteAsync();
        var itemAfterDeleteResponse = await TestClient.Request($"/api/items/{item.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteItemResponse.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        itemAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetItemById_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateDto().Generate();

        //act
        var item = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;
        var itemResponse = await TestClient.Request($"/api/items/{item.Id}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var itemData = (await itemResponse.GetJsonAsync<ResponseDto<ItemOutputDto>>()).Data;

        //assert
        itemResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        itemData.Should().BeEquivalentTo(item,
            options => options.Excluding(p => p.AnvisaDueDate));
    }

    [Fact]
    public async void Should_GetItems_When_ValidDataProvided()
    {
        //arrange
        var fakeFirstItem = ItemFaker.GetFakerItemCreateDto().Generate();
        var fakeSecondItem = ItemFaker.GetFakerItemCreateDto().Generate();

        //act
        var firstItem = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeFirstItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;
        var secondItem = (await TestClient.Request("/api/items").AsAuthenticated().PostJsonAsync(fakeSecondItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        var itemsResponse = await TestClient.Request($"/api/items").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var itemsData = (await itemsResponse.GetJsonAsync<ResponseDto<List<ItemOutputDto>>>()).Data;

        //assert
        itemsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        itemsData.Should().ContainEquivalentOf(firstItem, options=> options.Excluding(p=>p.AnvisaDueDate)); 
        itemsData.Should().ContainEquivalentOf(secondItem, options=> options.Excluding(p=>p.AnvisaDueDate));
    }
}