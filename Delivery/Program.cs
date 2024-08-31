using System;
using System.Collections.Generic;

// Базовый класс доставки
abstract class Delivery
{
    protected string Address;

    public Delivery(string address)
    {
        Address = address;
    }

    public virtual void DisplayAddress()
    {
        Console.WriteLine("Адрес доставки: {0}", Address);
    }

    public string GetDeliveryInfo()
    {
        return "Дополнительная информация о доставке";
    }
}

// Доставка на дом
class HomeDelivery : Delivery
{
    private DeliveryPoint Courier;

    public HomeDelivery(string address, DeliveryPoint courier) : base(address)
    {
        Courier = courier;
    }

    public override void DisplayAddress()
    {
        Console.WriteLine("Доставка на дом: {0}", Address);
        Console.WriteLine("Курьер: {0}", Courier.Name);
    }

    public string GetCourierName()
    {
        return Courier.Name;
    }
}

// Доставка в пункт выдачи
class PickPointDelivery : Delivery
{
    private DeliveryPoint DeliveryPoint;

    public PickPointDelivery(string address, DeliveryPoint deliveryPoint) : base(address)
    {
        DeliveryPoint = deliveryPoint;
    }

    public override void DisplayAddress()
    {
        Console.WriteLine("Доставка в пункт выдачи: {0}", Address);
        Console.WriteLine("Название пункта выдачи: {0}", DeliveryPoint.Name);
    }
}

// Доставка в розничный магазин
class ShopDelivery : Delivery
{
    private DeliveryPoint Shop;

    public ShopDelivery(string address, DeliveryPoint shop) : base(address)
    {
        Shop = shop;
    }

    public override void DisplayAddress()
    {
        Console.WriteLine("Доставка в розничный магазин: {0}", Address);
        Console.WriteLine("Название магазина: {0}", Shop.Name);
    }
}

// Базовый класс для всех точек доставки
abstract class DeliveryPoint
{
    public string Name { get; private set; }

    protected DeliveryPoint(string name)
    {
        Name = name;
    }
}

// Курьер наследуется от DeliveryPoint
class Courier : DeliveryPoint
{
    public Courier(string name) : base(name) { }
}

// Пункт выдачи наследуется от DeliveryPoint
class PickPoint : DeliveryPoint
{
    public PickPoint(string name) : base(name) { }
}

// Магазин наследуется от DeliveryPoint
class Shop : DeliveryPoint
{
    public Shop(string name) : base(name) { }
}

// Класс заказа
class Order<TDelivery> where TDelivery : Delivery
{
    private static int NextOrderNumber = 1;

    public int Number { get; private set; }
    public TDelivery Delivery { get; private set; }
    public Customer Customer { get; private set; }
    public List<Product> Items { get; private set; } = new List<Product>();
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public double TotalCost { get; private set; } = 0;

    // Статический метод генерации номера заказа
    public static int GenerateOrderNumber()
    {
        return NextOrderNumber++;
    }

    // Конструктор класса Order
    public Order(TDelivery delivery, Customer customer)
    {
        Number = GenerateOrderNumber();
        Delivery = delivery;
        Customer = customer;
    }

    // Добавление товара в заказ
    public void AddItem(Product product)
    {
        Items.Add(product);
        TotalCost += product.Price * product.Quantity;
    }

    // Вывод информации о заказе
    public void DisplayOrderInfo()
    {
        Console.WriteLine("Номер заказа: {0}", Number);
        Console.WriteLine("Статус заказа: {0}", Status);
        Console.WriteLine("Клиент: {0}", Customer.Name);
        Delivery.DisplayAddress();
        Console.WriteLine("Товары в заказе:");
        foreach (Product product in Items)
        {
            Console.WriteLine("- {0} (x{1})", product.Name, product.Quantity);
        }
        Console.WriteLine("Общая стоимость: {0}", TotalCost);
    }
}

// Класс клиента
class Customer
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }

    public Customer(string name, string email, string phoneNumber, string address)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
    }
}

// Класс товара
class Product
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double Price { get; private set; }
    public int Quantity { get; private set; }

    public Product(string name, string description, double price, int quantity)
    {
        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
    }
}

// Перечисление статусов заказа
enum OrderStatus
{
    Pending, // Ожидает обработки
    Processing, // Обрабатывается
    Shipped, // Отправлен
    Delivered, // Доставлен
    Cancelled // Отменен
}


class Program
{
    static void Main(string[] args)
    {
        // Создание нового заказа с доставкой на дом
        Order<HomeDelivery> homeDeliveryOrder = new Order<HomeDelivery>(
            new HomeDelivery("Улица Ленина, 123", new Courier("Иван Иванов")),
            new Customer("Иван Петров", "petrov@example.com", "1234567890", "Улица Пушкина, 1")
        );

        // Добавление товаров в заказ
        homeDeliveryOrder.AddItem(new Product("Товар 1", "Описание товара 1", 100.0, 1));
        homeDeliveryOrder.AddItem(new Product("Товар 2", "Описание товара 2", 50.0, 2));

        // Вывод информации о заказе
        homeDeliveryOrder.DisplayOrderInfo();

        Console.WriteLine();

        // Создание нового заказа с доставкой в пункт выдачи
        Order<PickPointDelivery> pickPointDeliveryOrder = new Order<PickPointDelivery>(
            new PickPointDelivery("Улица Пушкина, 1", new PickPoint("Пункт выдачи №1")),
            new Customer("Петр Сидоров", "sidoro@example.com", "9876543210", "Улица Лермонтова, 2")
        );

        // Добавление товаров в заказ
        pickPointDeliveryOrder.AddItem(new Product("Товар 3", "Описание товара 3", 200.0, 3));
        pickPointDeliveryOrder.AddItem(new Product("Товар 4", "Описание товара 4", 150.0, 1));

        // Вывод информации о заказе
        pickPointDeliveryOrder.DisplayOrderInfo();

        Console.ReadKey();
    }
}
