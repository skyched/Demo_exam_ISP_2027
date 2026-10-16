using APP_WORK.Properties;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using APP_WORK.Properties;

namespace APP_WORK
{
    public partial class MainForm : Form
    {
        List<CardProduct> products = new List<CardProduct>();
        List<int> IdProdSales = new List<int>();
        public MainForm()
        {
            InitializeComponent();
            Sales();
            OutProd();
        }
        public List<CardProduct> Products() 
        {
            var con = new NpgsqlConnection(Resources.conn);
            con.Open();
            var sql = @"SELECT id_dubl, _cat.title, image, __prod.title, _manu.title,  consist, price,
                    sum(__stock.quantity_availabe),
                    count(_size.id)
	                    FROM public.__prod
	                    join public._cat ON _cat.id = __prod.cat_id
	                    join public._manu ON _manu.id = __prod.manu_id
	                    join public.__stock ON __stock.prod_id = __prod.id_dubl
	                    join public._size ON _size.id = __stock.size_id
	                    group by id_dubl, _cat.title, image, __prod.title, _manu.title,  consist, price";
            var cmd = new NpgsqlCommand(sql, con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new CardProduct() 
                {
                    idProd = reader.GetInt32(0),
                    cat = reader.GetString(1),
                    photo = reader.IsDBNull(2) ? "picture.png" : reader.GetString(2),
                    prodName = reader.GetString(3),
                    proiz = reader.GetString(4),
                    sostav = reader.GetString(5),
                    price = reader.GetDecimal(6),
                    totalQuant = reader.GetInt32(7),
                    totalSize = reader.GetInt32(8)
                };
                if (IdProdSales.Contains(product.idProd))
                {
                    product.price *= 0.75m;
                    product.priceFirst = reader.GetDecimal(6);
                }
                product.SetLabels();
                products.Add(product);
            }
            return products;
        }
        public List<int> Sales() 
        {
            var con = new NpgsqlConnection(Resources.conn);
            con.Open();
            var sql = @"SELECT prod_id
	                FROM public.__order
	                where date_trunc('month',start_date) = 
	                (select date_trunc('month',MAX(start_date))  from public.__order)
	                except
	                SELECT prod_id
	                FROM public.__order
	                where date_trunc('month',start_date) = 
	                (select date_trunc('month',MAX(start_date)) - interval '1 month' from public.__order)";
            var cmd = new NpgsqlCommand(sql, con);
            var reader = cmd.ExecuteReader();
            while (reader.Read()) 
            {
                IdProdSales.Add(reader.GetInt32(0));
            }
            return IdProdSales; 
        }
        public void OutProd() 
        {
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();
            List<CardProduct> cards = Products();
            foreach (CardProduct product in cards)
            {
                flowLayoutPanel1.Controls.Add(product);
            }
            flowLayoutPanel1.ResumeLayout();
        }
    }
}