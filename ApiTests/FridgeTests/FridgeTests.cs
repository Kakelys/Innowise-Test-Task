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

namespace API.Tests.FridgeTests
{
    public class FridgeTests
    {
        [Fact]
        public void GetAll()
        {
            var mockRepo = new Mock<IRepositoryManager>();
            var rep = new MockRepository(MockBehavior.Strict);
            
            Setups.GetAllFridgesSetup(ref mockRepo);

            var controller = new FridgeController(mockRepo.Object);
           
            var result = controller.Get().Result;
            var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<Fridge>>>(result);
            var actionValue = Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
            
            var list = ((IEnumerable<Fridge>)actionValue?.Value).ToList();
            
            Assert.Equal(Setups.FridgeList.Count(), list.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void GetById(int id)
        {
            var mockRepo = new Mock<IRepositoryManager>();
            Setups.GetByIdAsyncSetup(ref mockRepo, id);

            var controller = new FridgeController(mockRepo.Object);

            var result = controller.GetById(id).Result;
            var actionResult = Assert.IsAssignableFrom<ActionResult<Fridge>>(result);
            var actionValue = Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);

            var fridge = (Fridge)actionValue?.Value;

            Assert.Equal(fridge.Name, (Setups.GetFridgeById(id).Result.Name));
        }

        [Fact]
        public void NotFoundGetById()
        {
            var id = -1;
            var mockRepo = new Mock<IRepositoryManager>();
            Setups.GetByIdAsyncSetup(ref mockRepo, id);

            var controller = new FridgeController(mockRepo.Object);

            var result = controller.GetById(id).Result;

            Assert.IsAssignableFrom<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void PostFridge()
        {
            var mockRepo = new Mock<IRepositoryManager>();
            
            var fridge =  new CreateFridgeDto(){ Name = "d", OwnerName="d", Model=1};
            Setups.PostFridgeSetup(ref mockRepo, fridge);
            Setups.GetByIdAsyncSetup(ref mockRepo, 0);

            var controller = new FridgeController(mockRepo.Object);

            var result = controller.PostFridge(fridge);
            var actionValue = Assert.IsAssignableFrom<OkResult>(result.Result);
            
            Assert.Equal("d", Setups.GetFridgeById(0).Result.Name);
        }

        [Fact]
        public void DeleteFridge()
        {
            var mockRepo = new Mock<IRepositoryManager>();
            var fridge =  new Fridge(){Id = 0, Name = "d", OwnerName="d", Model=1};
            var fridgeDto = new CreateFridgeDto(){Name = fridge.Name, OwnerName = fridge.OwnerName, Model = (int)fridge.Model};
            Setups.PostFridgeSetup(ref mockRepo, fridgeDto);
            Setups.DeleteFridgeSetup(ref mockRepo, fridge);
            Setups.GetByIdAsyncSetup(ref mockRepo, 0);

            var controller = new FridgeController(mockRepo.Object);

            controller.PostFridge(fridgeDto);
            var result = controller.DeleteFridge(0).Result;

            var actionValue = Assert.IsAssignableFrom<OkObjectResult>(result.Result);

            Assert.True((bool)actionValue.Value);
        }

        

        
    }
}