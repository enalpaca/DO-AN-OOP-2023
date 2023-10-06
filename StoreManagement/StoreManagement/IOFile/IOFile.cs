﻿using StoreManagement.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace StoreManagement.IOFile
{
    public class IOFile
    {
        // https://stackoverflow.com/questions/58003293/dotnet-core-system-text-json-unescape-unicode-string
        public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public static void Save<T>(string fileName, List<T> list)
        {
            string json = JsonSerializer.Serialize(list, jsonSerializerOptions);
            File.WriteAllText(fileName, json);
        }

        public static List<T> Load<T>(string fileName)
        {

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                if (json != "")
                {
                    var list = JsonSerializer.Deserialize<List<T>>(json);
                    return list != null ? list : new List<T>();
                }
            }

            return new List<T>();
        }

        public static void SaveProducts(List<Product> products)
        {
            Save("product.json", products);
        }
        public static List<Product> ReadProduct()
        {
            List<Product> products = Load<Product>("product.json");
            List<Category> categories = Load<Category>("category.json");

            foreach (Product product in products)
            {
                product.Category = categories.Find(x => x.Id == product.CategoryId);
            }

            return products;
        }

        public static void SaveCategories(List<Category> categories)
        {
            Save("category.json", categories);
        }
        public static List<Category> ReadCategory()
        {
            return Load<Category>("category.json");
        }

        public static void SaveInvoices(List<Invoice> invoices)
        {
            Save("invoice.json", invoices);
        }
        public static List<Invoice> ReadInvoice()
        {
            List<Invoice> invoices = Load<Invoice>("invoice.json");
            List<Product> products = ReadProduct();

            foreach (Invoice invoice in invoices)
            {
                foreach (InvoiceProduct invoiceProductItem in invoice.ProductItems)
                {
                    Product? foundProduct = products.Find(x => x.Id == invoiceProductItem.Id);
                    if (foundProduct != null)
                    {
                        invoiceProductItem.Name = foundProduct.Name;
                        invoiceProductItem.Price = foundProduct.Price;
                    }
                }
            }
            return invoices;
        }

        public static void SaveGoodsReceiptBills(List<GoodsReceiptBill> goodsReceiptBills)
        {
            Save("goodsReceiptBill.json", goodsReceiptBills);
        }

        public static List<GoodsReceiptBill> ReadGoodsReceiptBill()
        {
            List<Product> products = ReadProduct();
            List<GoodsReceiptBill> goodsReceiptBill = Load<GoodsReceiptBill>("goodsReceiptBill.json");

            foreach (GoodsReceiptBill bill in goodsReceiptBill)
            {
                Product? foundProduct = products.Find(x => x.Id == bill.ProductItem.Id);
                if (foundProduct != null)
                {
                    bill.ProductItem.Name = foundProduct.Name;
                }
            }

            return goodsReceiptBill;
        }
    }
}
