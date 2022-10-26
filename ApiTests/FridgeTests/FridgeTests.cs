using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Entities;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using ApiTests;
using API.DTOs;
using API.Services.Logics;
using API.Errors;

namespace API.Tests.FridgeTests
{
    public class FridgeTests
    {
        [Fact]
        public void Get_WithoutParams_ListOfFridges()
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryManager>();
            var rep = new MockRepository(MockBehavior.Strict);
            
            Setups.GetAllFridgesSetup(ref mockRepo);

            var fridgeService = new FridgeService(mockRepo.Object);
            var controller = new FridgeController(fridgeService);
           
            //Act
            var result = controller.Get().Result;
            var objectResult = (OkObjectResult)result?.Result;
            var list = ((IEnumerable<Fridge>)objectResult?.Value)?.ToList();

            //Assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<Fridge>>>(result);
            Assert.IsAssignableFrom<OkObjectResult>(result?.Result);
            
            Assert.Equal(Setups.FridgeList.Count(), list.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void GetById_ExistingId_Fridge(int id)
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryManager>();
            Setups.GetByIdAsyncSetup(ref mockRepo, id);

            var fridgeService = new FridgeService(mockRepo.Object);
            var controller = new FridgeController(fridgeService);

            //Act
            var result = controller.GetById(id).Result;
            var objectResult = (OkObjectResult)result?.Result;
            var fridge = (Fridge)objectResult.Value;

            //Assert
            Assert.IsAssignableFrom<ActionResult<Fridge>>(result);
            Assert.IsAssignableFrom<OkObjectResult>(result.Result);

            Assert.Equal(fridge.Name, (Setups.GetFridgeById(id).Result.Name));
        }

        [Fact]
        public void GetById_UnexistigId_NotFound()
        {
            //Arrange
            var id = -1;
            var mockRepo = new Mock<IRepositoryManager>();
            Setups.GetByIdAsyncSetup(ref mockRepo, id);

            var fridgeService = new FridgeService(mockRepo.Object);
            var controller = new FridgeController(fridgeService);
            ApiException ex = null;

            //Act
            try
            {
                var result = controller.GetById(id).Result;
            }
            catch(AggregateException e)
            {
                ex = (ApiException)e.InnerException;
            }
            
            //Assert
            Assert.Equal(404, ex?.StatusCode);
        }

        [Fact]
        public void PostFridge_CreateFridgeDto_SavedFridge()
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryManager>();
            
            var fridge =  new CreateFridgeDto(){ Name = "d", OwnerName="d", Model=1};
            Setups.PostFridgeSetup(ref mockRepo, fridge);
            Setups.GetByIdAsyncSetup(ref mockRepo, 0);

            var fridgeService = new FridgeService(mockRepo.Object);
            var controller = new FridgeController(fridgeService);

            //Act
            var result = controller.PostFridge(fridge);

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            Assert.Equal("d", Setups.GetFridgeById(0).Result.Name);
        }

        [Fact]
        public void DeleteFridge_ExistingId_True()
        {
            //Arrange
            var mockRepo = new Mock<IRepositoryManager>();
            var fridge =  new Fridge(){Id = 0, Name = "d", OwnerName="d", Model=1};
            var fridgeDto = new CreateFridgeDto(){Name = fridge.Name, OwnerName = fridge.OwnerName, Model = (int)fridge.Model};
            Setups.PostFridgeSetup(ref mockRepo, fridgeDto);
            Setups.DeleteFridgeSetup(ref mockRepo, fridge);
            Setups.GetByIdAsyncSetup(ref mockRepo, 0);

            var fridgeService = new FridgeService(mockRepo.Object);
            var controller = new FridgeController(fridgeService);

            //Act
            controller.PostFridge(fridgeDto);
            var result = controller.DeleteFridge(0).Result;
            var objectResult = (OkObjectResult)result.Result;

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
            Assert.True(!(bool)objectResult?.Value);
        }
    }
}