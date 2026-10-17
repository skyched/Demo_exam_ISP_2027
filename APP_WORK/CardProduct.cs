using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace APP_WORK
{
    public partial class CardProduct : UserControl
    {
        public int idProd;
        public string consist;
        public string prodName;
        public string manu;
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
            label1.Text = $"{manu} | {prodName}";

            label6.Visible = false;

            label8.Visible = false;

            label2.Text = totalSize > 5 ? "Количество товаров на складе: много" : "Количество товаров на складе: мало";

            label3.Text = $"Состав: {consist}";

            label4.Text = $"Категория: {cat}";

            if (priceFirst != 0)
            {
                label5.Text = "Цена: " + Decimal.Round(priceFirst, 2).ToString() + " руб.";
                label5.Font = new Font("Calibri", 10, FontStyle.Strikeout);

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
