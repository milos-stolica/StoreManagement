using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StoreManagement.API.Common;
using StoreManagement.API.Entities;
using StoreManagement.API.Models;
using StoreManagement.API.Repositories;
using StoreManagement.API.Repositories.Extensions;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Services.OrderingService
{
    public class OrderingService : IOrderingService
    {
        #region Fields

        private readonly IProductsRepository productsRepository;
        private readonly IOrderDetailsFactory orderDetailsFactory;
        private readonly IDictionary<EntityStatus, IErrorMessageFactory<OrderedProductDetails>> errorMassageFactories;
        private readonly IMapper mapper;

        #endregion Fields

        #region Constructor

        public OrderingService(IProductsRepository productsRepository, 
                               IOrderDetailsFactory orderDetailsFactory, 
                               IDictionary<EntityStatus, IErrorMessageFactory<OrderedProductDetails>> errorMassageFactories,
                               IMapper mapper)
        {
            this.productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            this.orderDetailsFactory = orderDetailsFactory ?? throw new ArgumentNullException(nameof(orderDetailsFactory));
            this.errorMassageFactories = errorMassageFactories ?? throw new ArgumentNullException(nameof(errorMassageFactories));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(orderDetailsFactory));
        }

        #endregion Constructor

        #region IOrderingService methods

        public void MakeOrder(IEnumerable<OrderCreationDTO> orderList, Action<OrderDetails> orderFinished)
        {
            ArgumentValidator.ThrowIfNull(orderList, nameof(orderList));
            ArgumentValidator.ThrowIfNull(orderFinished, nameof(orderList));

            Dictionary<string, float> skuToOrderedQuantity = orderList.ToDictionary(keySelector: order => order.ProductSku,
                                                                                    elementSelector: order => order.Quantity);

            IEnumerable<ExtendedEntity<Product>> products = productsRepository.DecreaseProductsQuantity(skuToOrderedQuantity);

            //if at least one product not found, create empty list with appropriate status
            if (products.FilterBy(EntityStatus.NotFound).Any())
            {
                orderFinished(CreateOrderDetails(OrderStatus.NotExistingProducts, new List<OrderedProductDetails>()));
                return;
            }

            //if at least one product is out of stock (or there is less than required) create list containing only these products
            IEnumerable<ExtendedEntity<Product>> insufficientQuantityProducts = products.FilterBy(EntityStatus.UnprocessableUpdate);
            if (insufficientQuantityProducts.Any())
            {
                orderFinished(CreateOrderDetails(OrderStatus.OutOfStock, Combine(insufficientQuantityProducts, skuToOrderedQuantity)));
                return;
            }

            orderFinished(CreateOrderDetails(OrderStatus.Successful, Combine(products, skuToOrderedQuantity)));
        }

        public OrderUnprocessableDTO GenerateError(OrderDetails order)
        {
            ArgumentValidator.ThrowIfNull(order, nameof(order));
            ArgumentValidator.ThrowIfNullOrEmpty(order.ProductsDetails, nameof(order.ProductsDetails));

            IEnumerable<string> messages = order.ProductsDetails.Select(product => GenerateErrorMessage(product));

            return new OrderUnprocessableDTO()
            {
                Messages = messages
            };
        }

        public OrderReceiptDTO GenerateReceipt(OrderDetails order)
        {
            ArgumentValidator.ThrowIfNull(order, nameof(order));
            ArgumentValidator.ThrowIfNullOrEmpty(order.ProductsDetails, nameof(order.ProductsDetails));
           
            IEnumerable<OrderLineDTO> orderLines = mapper.Map<IEnumerable<OrderLineDTO>>(order.ProductsDetails);

            return new OrderReceiptDTO()
            {
                OrderLines = orderLines,
                Currency = orderLines.First().Currency,
                TotalPrice = MathWrapper.Round(orderLines.Sum(orderLine => orderLine.Price), 2),
                Timestamp = DateTime.Now
            };
        }

        #endregion IOrderingService methods

        #region Private methods

        private IEnumerable<OrderedProductDetails> Combine(IEnumerable<ExtendedEntity<Product>> products, Dictionary<string, float> skuToOrderedQuantity)
        {
            Action<OrderedProductDetails> afterMapAction = productDetails =>
            {
                productDetails.OrderedQuantity = skuToOrderedQuantity[productDetails.Sku];
                productDetails.Status = products.First(product => product.Entity.Sku == productDetails.Sku).StatusCode;
            };

            //use Product -> OrderedProductDetails mapper and put additional info not contained in Product after map
            return mapper.Map<IEnumerable<OrderedProductDetails>>(products.ToEntities(),
                      opt => opt.AfterMap((src, dest) => dest.ToList().ForEach(orderedProd => afterMapAction(orderedProd))));
        }

        private OrderDetails CreateOrderDetails(OrderStatus status, IEnumerable<OrderedProductDetails> productsDetails)
        {
            return orderDetailsFactory.CreateOrderDetails(status, productsDetails);
        }

        private string GenerateErrorMessage(OrderedProductDetails product)
        {
            IErrorMessageFactory<OrderedProductDetails> errorFactory;
            if(errorMassageFactories.TryGetValue(product.Status, out errorFactory))
            {
                return errorFactory.CreateErrorMessage(product);
            }

            return new FallbackErrorFactory().CreateErrorMessage(product);
        }

        #endregion Private methods
    }
}
