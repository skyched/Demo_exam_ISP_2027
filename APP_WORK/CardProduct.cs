using APP_WORK.Properties;
using Npgsql;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace APP_WORK
{
    public partial class CardProduct : UserControl
    {
        public int idProd;
        public string sostav;
        public string prodName;
        public string proiz;
        public string photo;
        public int totalQuant;
        public int totalSize;
        public string cat;
        public decimal price;
        public decimal priceFirst;

        public CardProduct()
        {
            InitializeComponent();
        }
        public void SetLabels()
        {
            label1.Text = $"{prodName} / {proiz}";
            label2.Text = totalSize > 5 ? "Товаров на складе: много" : "Товаров на складе: мало";
            label3.Text = sostav;
            label4.Text = cat;
            if (priceFirst != 0)
            {
                label5.Text = "Цена: " + Decimal.Round(priceFirst, 2).ToString() + " руб.";
                label5.Font = new Font("Calibri", 12, FontStyle.Strikeout);
                label7.Text = "Цена со скидкой 25%: " + Decimal.Round(price, 2).ToString() + " руб.";
                label7.ForeColor = Color.Red;
            }
            else 
            {
                label5.Visible = false;
                label7.Text = "Цена: " + Decimal.Round(price, 2).ToString() + " руб.";
            }
            if (totalQuant <= 3)
            {
                this.BackColor = Color.Red;
            }
            pictureBox1.ImageLocation = Path.Combine(Application.StartupPath, @"../../Properties", photo);
        }
    }
}
