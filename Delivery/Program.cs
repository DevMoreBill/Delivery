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
    private Courier Courier;

    public HomeDelivery(string address, Courier courier) : base(address)
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
    private PickPoint PickPoint;

    public PickPointDelivery(string address, PickPoint pickPoint) : base(address)
    {
        PickPoint = pickPoint;
    }

    public override void DisplayAddress()
    {
        Console.WriteLine("Доставка в пункт выдачи: {0}", Address);
        Console.WriteLine("Название пункта выдачи: {0}", PickPoint.Name);
    }
}

// Доставка в розничный магазин
class ShopDelivery : Delivery
{
    private Shop Shop;

    public ShopDelivery(string address, Shop shop) : base(address)
    {
        Shop = shop;
    }

    public override void DisplayAddress()
    {
        Console.WriteLine("Доставка в розничный магазин: {0}", Address);
        Console.WriteLine("Название магазина: {0}", Shop.Name);
    }
}

// Класс курьера
class Courier
{
    public string Name { get; private set; }

    public Courier(string name)
    {
        Name = name;
    }
}

// Класс пункта выдачи
class PickPoint
{
    public string Name { get; private set; }

    public PickPoint(string name)
    {
        Name = name;
    }
}

// Класс магазина
class Shop
{
    public string Name { get; private set; }

    public Shop(string name)
    {
        Name = name;
    }
}

// Класс заказа
class Order
{
    private static int NextOrderNumber = 1;

    public int Number { get; private set; }
    public Delivery Delivery { get; private set; }
    public Customer Customer { get; private set; }
    public List<Product> Items { get; private set; } = new List<Product>();
    public OrderStatus Status { get; private set; } = OrderStatus.Processing;
    public double TotalCost { get; private set; } = 0;

    // Статический метод генерации номера заказа
    public static int GenerateOrderNumber()
    {
        return NextOrderNumber++;
    }

    // Конструктор класса Order
    public Order(Delivery delivery, Customer customer)
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

// Статусы заказов
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
        // Создание нового заказа
        Order order = new Order(new HomeDelivery("Улица Ленина, 123", new Courier("Иван Иванов")),
                               new Customer("Иван Петров", "petrov@mail.com", "1234567890", "Улица Пушкина, 1"));

        // Добавление товаров в заказ
        order.AddItem(new Product("Товар 1", "Описание товара 1", 100.0, 1));
        order.AddItem(new Product("Товар 2", "Описание товара 2", 50.0, 2));

        // Вывод информации о заказе
        order.DisplayOrderInfo();

        Console.ReadKey();
    }
}
