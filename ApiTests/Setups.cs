using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace ApiTests
{
    public static class Setups
    {
        private static List<Fridge> _fridges;
        private static List<FridgeProduct> _fridgeProducts;
        private static List<Product> _products;
        public static IQueryable<Fridge> Fridges
        {
            get
            {
                if(_fridges == null)
                    _fridges = BuildFridges();

                return _fridges.AsQueryable();
            }
        }

        public static List<Fridge> FridgeList
        {
            get
            {
                if(_fridges == null)
                    _fridges = BuildFridges();

                return _fridges;
            }
        }

        private static IQueryable<FridgeProduct> FridgeProducts
        {
            get
            {
                if(_fridgeProducts == null)
                    _fridgeProducts = FridgeProductsSetup().ToList();

                return _fridgeProducts.AsQueryable();
            }
        }

        public static void FridgeSetup(ref Mock<IRepositoryManager> mock)
        {

        }

        public static void GetAllFridgesSetup(ref Mock<IRepositoryManager> mock)
        {
            mock.Setup(rep => rep.Fridge.GetAllFridgesAsync(false)).Returns(GetFridgeAll());
        }

        public static void GetByIdAsyncSetup(ref Mock<IRepositoryManager> mock, int id)
        {
            mock.Setup(rep => rep.Fridge.GetByIdAsync(id, It.IsAny<bool>())).Returns(GetFridgeById(id));
        }

        public static void PostFridgeSetup(ref Mock<IRepositoryManager> mock, CreateFridgeDto fridge)
        {
            var fr = new Fridge(){Id = 0,Name = fridge.Name, OwnerName = fridge.OwnerName, Model = fridge.Model};
            mock.Setup(rep => rep.Fridge.Create(fr)).Returns(AddFridge(fr));   
        }

        public static void DeleteFridgeSetup(ref Mock<IRepositoryManager> mock, Fridge fridge)
        {
            
            mock.Setup(rep => rep.Fridge.Delete(fridge)).Returns(DeleteFridge(fridge));
        }

        public static async Task<Fridge> GetFridgeById(int id)
        {
            return Fridges.FirstOrDefault(f => f.Id == id);
        }

        public static async Task<IEnumerable<Fridge>> GetFridgeAll()
        {
            return Fridges.ToList();
        }

        private static Fridge AddFridge(Fridge fridge)
        {
            FridgeList.Add(fridge);

            return fridge;
        }

        private static bool DeleteFridge(Fridge fridge)
        {
            return FridgeList.Remove(fridge);
        }

        private static List<Fridge> BuildFridges()
        {
            var f = new List<Fridge>();
            var fp = new List<FridgeProduct>(FridgeProductsSetup());
            var m = new List<FridgeModel>(FridgeModelSetup());

            f.Add(new Fridge()
            {
                Id = 1,
                Name = "Some name 1",
                OwnerName = "Owner Name 1",
                Model = 1,
                ModelNavigation = m[0]
            });
            f.Add(new Fridge()
            {
                Id = 2,
                Name = "Some name 2",
                OwnerName = "Owner Name 2",
                Model = 2,
                ModelNavigation = m[1]
            });
            f.Add(new Fridge()
            {
                Id = 3,
                Name = "Some name 3",
                OwnerName = "Owner Name 3",
                Model = 3,
                ModelNavigation = m[2]
            });

            return f;
        }

        public static IEnumerable<FridgeModel> FridgeModelSetup()
        {
            var fr = new List<FridgeModel>();
            fr.Add(new FridgeModel()
            {
                Id = 1,
                Name = "Atlant",
                Year = 2001
            });
            fr.Add(new FridgeModel()
            {
                Id = 2,
                Name = "SMEG",
                Year = 1996
            });
            fr.Add(new FridgeModel()
            {
                Id = 3,
                Name = "ICE",
                Year = 1986
            });

            return fr;
        }

        private static IEnumerable<FridgeProduct> FridgeProductsSetup()
        {
            var fp = new List<FridgeProduct>();
            var p = new List<Product>(ProductSetup());
            fp.Add(new FridgeProduct(){Fridge = 1,Product = 5,Quantity = 5, ProductNavigation = p[4]});
            fp.Add(new FridgeProduct(){Fridge = 2,Product = 2,Quantity = 2, ProductNavigation = p[1]});
            fp.Add(new FridgeProduct(){Fridge = 2,Product = 1,Quantity = 7, ProductNavigation = p[0]});
            fp.Add(new FridgeProduct(){Fridge = 1,Product = 2,Quantity = 2, ProductNavigation = p[1]});
            fp.Add(new FridgeProduct(){Fridge = 3,Product = 3,Quantity = 3, ProductNavigation = p[2]});
            fp.Add(new FridgeProduct(){Fridge = 2,Product = 4,Quantity = 1, ProductNavigation = p[3]});
            fp.Add(new FridgeProduct(){Fridge = 1,Product = 5,Quantity = 12, ProductNavigation = p[4]});
            fp.Add(new FridgeProduct(){Fridge = 2,Product = 2,Quantity = 2, ProductNavigation = p[1]});
            fp.Add(new FridgeProduct(){Fridge = 2,Product = 2,Quantity = 3, ProductNavigation = p[1]});
            fp.Add(new FridgeProduct(){Fridge = 3,Product = 1,Quantity = 6, ProductNavigation = p[0]});

            return fp;
        }

        public static IEnumerable<Product> ProductSetup()
        {
            var p = new List<Product>();
            p.Add(new Product()
            {
                Id = 1,
                Name = "Milk",
                DefaultQuantity = 2
            });
            p.Add(new Product()
            {
                Id = 2,
                Name = "Potato",
                DefaultQuantity = 4
            });
            p.Add(new Product()
            {
                Id = 3,
                Name = "Cheese",
                DefaultQuantity = 1
            });
            p.Add(new Product()
            {
                Id = 4,
                Name = "Bread",
                DefaultQuantity = 1
            });
            p.Add(new Product()
            {
                Id = 5,
                Name = "Apple",
                DefaultQuantity = 7
            });

            return p;
        }
    }
}