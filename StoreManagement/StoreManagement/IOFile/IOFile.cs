﻿using StoreManagement.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace StoreManagement.IOFile
{
    public class IOFile
    {
        private static string? baseFolder = Environment.GetEnvironmentVariable("ASPNETCORE_FOLDER");
        private static string productFilePath = "product.json";
        private static string categoryFilePath = "category.json";
        private static string invoiceFilePath = "invoice.json";
        private static string goodsReceiptBillFilePath = "goodsReceiptBill.json";

        public static void checkFilePaths()
        {
            if (baseFolder == null)
            {
                return;
            }

            baseFolder = baseFolder.Trim();

            if (baseFolder == "")
            {
                return;

            }

            productFilePath = baseFolder + '/' + "product.json";
            categoryFilePath = baseFolder + '/' + "category.json";
            invoiceFilePath = baseFolder + '/' + "invoice.json";
            goodsReceiptBillFilePath = baseFolder + '/' + "goodsReceiptBill.json";

            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
        }

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
            checkFilePaths();
            Save(productFilePath, products);
        }
        public static List<Product> ReadProduct()
        {
            checkFilePaths();
            List<Product> products = Load<Product>(productFilePath);
            List<Category> categories = Load<Category>(categoryFilePath);

            foreach (Product product in products)
            {
                product.Category = categories.Find(x => x.Id == product.CategoryId);
            }

            return products;
        }

        public static void SaveCategories(List<Category> categories)
        {
            checkFilePaths();
            Save(categoryFilePath, categories);
        }
        public static List<Category> ReadCategory()
        {
            checkFilePaths();
            return Load<Category>(categoryFilePath);
        }

        public static void SaveInvoices(List<Invoice> invoices)
        {
            checkFilePaths();
            Save(invoiceFilePath, invoices);
        }
        public static List<Invoice> ReadInvoice()
        {
            checkFilePaths();
            List<Invoice> invoices = Load<Invoice>(invoiceFilePath);
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
            checkFilePaths();
            Save(goodsReceiptBillFilePath, goodsReceiptBills);
        }

        public static List<GoodsReceiptBill> ReadGoodsReceiptBill()
        {
            checkFilePaths();
            List<Product> products = ReadProduct();
            List<GoodsReceiptBill> goodsReceiptBill = Load<GoodsReceiptBill>(goodsReceiptBillFilePath);

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
