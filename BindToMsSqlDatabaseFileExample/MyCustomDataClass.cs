using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindToMsSqlDatabaseFileExample
{
    public class TestData
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

        public static List<TestData> GetData(int Count)
        {
            List<TestData> ds = new List<TestData>();
            string[] names = new string[] { "A", "B", "C", "D" };
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Count; i++)
            {
                ds.Add(new TestData() { Name = names[r.Next(names.Length)], Date = DateTime.Today.AddDays(i), Value = r.Next(30) + 20 });
            }
            return ds;
        }
    }
    public class MonthData
    {
        public int Month { get; set; }
        public double Value { get; set; }
        public MonthData()
        {
            
        }
        // HACK to pass parameters to the class https://supportcenter.devexpress.com/ticket/details/t481543/web-designer-it-is-impossible-to-connect-parameters-of-the-object-data-source-to
        public static List<MonthData> GetData(int Year)
        {
            var Values = Form1.UoW.ExecuteSproc("GetRandomCurrencyValues", Year);
            List<MonthData> ValuesList = new List<MonthData>(); ;
            foreach (var Row in Values.ResultSet[0].Rows)
            {
                MonthData monthData = new MonthData();
                monthData.Month = (int)Row.Values[0];
                monthData.Value = double.Parse(Row.Values[1].ToString());
                ValuesList.Add(monthData);
            }
            //this.MonthData = ValuesList;
            return ValuesList;
        }
    }
    public  class MyCustomDataClass:INotifyPropertyChanged
    {
        public MyCustomDataClass()
        {
            this.Year = 2023;
        }
        
        public BindingList<MonthData> MonthData
        {
            get => monthData;
            set
            {
                if (monthData == value)
                    return;
                monthData = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(MonthData)));
            }
        }
        
        public BindingList<MonthData> RefreshData(int year)
        {
            var Values = Form1.UoW.ExecuteSproc("GetRandomCurrencyValues", year);
            BindingList<MonthData> ValuesList = new BindingList<MonthData>(); ;
            foreach (var Row in Values.ResultSet[0].Rows)
            {
                MonthData monthData = new MonthData();
                monthData.Month=(int)Row.Values[0];
                monthData.Value = double.Parse(Row.Values[1].ToString());
                ValuesList.Add(monthData);
            }
            //this.MonthData = ValuesList;
            return ValuesList;
        }

        BindingList<MonthData> monthData;
        int year;

        public int Year
        {
            get => year;
            set
            {
                if (year == value)
                    return;
                year = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Year)));
                RefreshData(Year);
            }

        }
        
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
