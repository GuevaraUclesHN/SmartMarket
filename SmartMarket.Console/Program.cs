// See https://aka.ms/new-console-template for more information

/*
 * Este archivo no debe cambiarse.
 */
using SmartMarket.Logic;

var stockService = new StockService();
const string stockItem =
    "{name : 'Apple', price : 1.99, producedOn : '2021-01-01', providerId : '514C10F2-BE0A-43BD-8DA2-21AE99B0F88B', providerName : 'Apple Inc.', membershipDeal: {quantity: 2, price: 3.99}}";
var addResult = await stockService.AddStockItemAsync(stockItem);
Console.WriteLine($"Adding stock item result: {addResult}");