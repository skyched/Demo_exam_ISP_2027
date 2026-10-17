using APP_WORK.Properties;
using Npgsql;
using System.Collections.Generic;
using System.Windows.Forms;

namespace APP_WORK
{
    public partial class MainForm : Form
    {
        List<CardProduct> products = new List<CardProduct>(); //тут храним созданные карточки
        List<int> IdProdSales = new List<int>(); //тут храним айдишники товаров, на которые будут скидки

        public MainForm()
        {
            InitializeComponent();

            Sales();

            OutProd();
        }

        public List<CardProduct> Products()
        {

            products.Clear();

            using (var con = new NpgsqlConnection(Resources.conn))
            {
                con.Open();
                var sql = @"SELECT id_dubl, d_cat.title, image, prod.title, d_manu.title,  consist, price,
                    sum(stock.quantity_availabe),
                    count(d_size.id)
	                    FROM public.prod
	                    join public.d_cat ON d_cat.id = prod.cat_id
	                    join public.d_manu ON d_manu.id = prod.manu_id
	                    join public.stock ON stock.prod_id = prod.id_dubl
	                    join public.d_size ON d_size.id = stock.size_id
	                    group by id_dubl, d_cat.title, image, prod.title, d_manu.title,  consist, price";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    using (var rez = cmd.ExecuteReader())
                    {
                        while (rez.Read())
                        {
                            var product = new CardProduct()
                            {
                                idProd = rez.GetInt32(0),
                                cat = rez.GetString(1),
                                photo = rez.IsDBNull(2) ? "picture.png" : rez.GetString(2),
                                prodName = rez.GetString(3),
                                manu = rez.GetString(4),
                                consist = rez.GetString(5),
                                price = rez.GetDecimal(6),
                                totalQuant = rez.GetInt32(7),
                                totalSize = rez.GetInt32(8)
                            };
                            if (IdProdSales.Contains(product.idProd))
                            {
                                product.price *= 0.75m;
                                product.priceFirst = rez.GetDecimal(6);
                            }
                            product.SetLabels();
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public List<int> Sales()
        {

            IdProdSales.Clear();

            using (var con = new NpgsqlConnection(Resources.conn))
            {

                con.Open();

                var sql = @"SELECT prod_id
	               FROM orders
	                where date_trunc('month',start_date) = 
	                (select date_trunc('month',MAX(start_date))  from public.orders)
	                except
	                SELECT prod_id
	                FROM orders
	                where date_trunc('month',start_date) = 
	                (select date_trunc('month',MAX(start_date)) - interval '1 month' from public.orders)";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    using (var rez = cmd.ExecuteReader())
                    {
                        while (rez.Read())
                        {
                            IdProdSales.Add(rez.GetInt32(0));
                        }
                    }
                }
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
                product.Margin = new Padding(0, 20, 0, 20);

                flowLayoutPanel1.Controls.Add(product);
            }

            flowLayoutPanel1.ResumeLayout();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            OutProd();
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}