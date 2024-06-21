using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Omini.Opme.Api.Dtos;
using Omini.Opme.Api.Tests.Extensions;

namespace Omini.Opme.Api.Tests.Controllers.V1;

public class ItemsV1ControllerTests : IntegrationTest
{
    [Fact]
    public async void Should_CreateItem_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateCommand().Generate();

        //act
        var response = await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeItem);
        var itemOutputDto = (await response.GetJsonAsync<ResponseDto<ItemOutputDto>>()).Data;

        //assert
        response.StatusCode.Should().Be(StatusCodes.Status201Created);

        fakeItem.Should().BeEquivalentTo(itemOutputDto,
            options =>
                options.Excluding(p => p.Code)
        );
    }

    [Fact]
    public async void Should_UpdateItem_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateCommand().Generate();

        //act
        var item = (await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        var itemUpdateCommand = ItemFaker.GetFakerItemUpdateCommand().Generate();
        itemUpdateCommand.Code = item.Code;

        var updateItemResponse = await TestClient.Request($"/api/v1/items/{item.Code}").AsAuthenticated().PutJsonAsync(itemUpdateCommand);
        var updateItemData = (await updateItemResponse.GetJsonAsync<ResponseDto<ItemOutputDto>>()).Data;
        var itemAfterUpdate = (await TestClient.Request($"/api/v1/items/{item.Code}").AsAuthenticated().GetAsync().ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        //assert
        updateItemResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        itemAfterUpdate.Should().BeEquivalentTo(updateItemData,
            options => options.Excluding(p => p.AnvisaDueDate));
        itemUpdateCommand.Should().BeEquivalentTo(updateItemData);
    }

    [Fact]
    public async void Should_DeleteItem_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateCommand().Generate();

        //act
        var item = (await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        var deleteItemResponse = await TestClient.Request($"/api/v1/items/{item.Code}").AsAuthenticated().DeleteAsync();
        var deleteItemData = (await deleteItemResponse.GetJsonAsync<ResponseDto<ItemOutputDto>>()).Data;
        var itemAfterDeleteResponse = await TestClient.Request($"/api/v1/items/{item.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();

        //assert
        deleteItemResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
        fakeItem.Should().BeEquivalentTo(deleteItemData,
            options => options.Excluding(p => p.AnvisaDueDate).Excluding(p => p.Code));

        itemAfterDeleteResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async void Should_GetItemById_When_ValidDataProvided()
    {
        //arrange
        var fakeItem = ItemFaker.GetFakerItemCreateCommand().Generate();

        //act
        var item = (await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;
        var itemResponse = await TestClient.Request($"/api/v1/items/{item.Code}").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
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
        var fakeFirstItem = ItemFaker.GetFakerItemCreateCommand().Generate();
        var fakeSecondItem = ItemFaker.GetFakerItemCreateCommand().Generate();

        //act
        var firstItem = (await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeFirstItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;
        var secondItem = (await TestClient.Request("/api/v1/items").AsAuthenticated().PostJsonAsync(fakeSecondItem).ReceiveJson<ResponseDto<ItemOutputDto>>()).Data;

        var itemsResponse = await TestClient.Request($"/api/v1/items").AsAuthenticated().AllowAnyHttpStatus().GetAsync();
        var itemsData = (await itemsResponse.GetJsonAsync<ResponseDto<List<ItemOutputDto>>>()).Data;

        //assert
        itemsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);

        itemsData.Should().ContainEquivalentOf(firstItem, options => options.Excluding(p => p.AnvisaDueDate));
        itemsData.Should().ContainEquivalentOf(secondItem, options => options.Excluding(p => p.AnvisaDueDate));
    }
}