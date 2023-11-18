using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;
using System.Reflection;
// Example data class
class MyDataClass
{
    public int OrderID { get; set; }

    public string? Product { get; set; } = string.Empty;

    public int QuantityOrdered { get; set; }

    public double PriceEach { get; set; }
    
    public string OrderDate { get; set; }
}

internal class Program
{
    private static void Main(string[] args)
    {

#if DEBUG
        string csvFilePath = "C:/Users/Ahar/Downloads/archive/Sales-Analysis-Dataset/Sales_May_2019.csv";

        List<MyDataClass> dataList = LoadCsvData<MyDataClass>(csvFilePath);

        Dictionary<int, List<MyDataClass>> SalesPerOrder = dataList.GroupBy(a=>a.OrderID).ToDictionary(a=>a.Key,b=>b.ToList());

        string connectionString = GetConnectionString();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connected to the database.");
                foreach (var Order in SalesPerOrder)
                {
                    InsertSalesData(connection, Order.Key);

                    foreach (var item in Order.Value)
                    {
                        InsertSalesDetailsData(connection, item);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
#endif
    }
    static void InsertSalesData(SqlConnection connection, int OrderID)
    {
        string insertQuery = "INSERT INTO SALES (ORDERID) VALUES (@Value1)";

        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@Value1", OrderID);

            int rowsAffected = command.ExecuteNonQuery();

        }
    }

    static void InsertSalesDetailsData(SqlConnection connection, MyDataClass item)
    {
        string insertQuery = "INSERT INTO SALESDETAILS (ORDERID, PRODUCT, QUANTITY, PRICE_EACHITEM) VALUES (@Value1, @Value2, @Value3, @Value4)";

        using (SqlCommand command = new SqlCommand(insertQuery, connection))
        {
            // Add parameters to prevent SQL injection
            command.Parameters.AddWithValue("@Value1", item.OrderID);
            command.Parameters.AddWithValue("@Value2", item.Product);
            command.Parameters.AddWithValue("@Value3", item.QuantityOrdered);
            command.Parameters.AddWithValue("@Value4", item.PriceEach);

            int rowsAffected = command.ExecuteNonQuery();
        }
    }
    static private string GetConnectionString()
    {
        // To avoid storing the connection string in your code,
        // you can retrieve it from a configuration file.
        return "Data Source=.;Initial Catalog=BMT;"
            + "Integrated Security=true;";
    }
    static List<T> LoadCsvData<T>(string filePath) where T : new()
    {
        List<T> dataList = new List<T>();

        using (TextFieldParser parser = new TextFieldParser(filePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header if your CSV file has one
            if (!parser.EndOfData)
            {
                parser.ReadLine();
            }

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();

                // Create an instance of your data class and populate its properties
                T dataItem = new T();

                // Assuming the order of fields in the CSV matches the order of properties in your class
                for (int i = 0; i < fields.Length; i++)
                {
                    SetProperty(dataItem, i, fields[i]);
                }

                dataList.Add(dataItem);
            }
        }

        return dataList;
    }

    static void SetProperty<T>(T obj, int index, string value)
    {
        var properties = typeof(T).GetProperties();
        if (index < properties.Length)
        {
            var property = properties[index];
            if (!SkipCondition(value, property))
            {
                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(obj, convertedValue);
            }

        }
    }

    private static bool SkipCondition(string value, PropertyInfo property)
    {
        return (value == "Order ID" && property.PropertyType == typeof(Int32)) || (value == string.Empty && property.PropertyType == typeof(Int32)) 
                || (value == string.Empty && property.PropertyType ==  typeof(Double)) || (value == "Quantity Ordered" && property.PropertyType == typeof(Int32))
                || (value == "Price Each" && property.PropertyType == typeof(Double));
    }
}