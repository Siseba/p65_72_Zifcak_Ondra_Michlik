using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http.Headers;

namespace p65_72_Zifcak_Ondra_Michlik
{
    public partial class Form_Domov : Form
    {
        string idPouzivatela = Form_Login.idPrihlasenehoPouzivatela;

        string users_subor = "users.csv";
        public double stavUctuPouzivatela;

        public Form_Domov()
        {
            InitializeComponent();
            panel_Domov.Enabled = true;
            panel_Domov.Visible = true;

            panel_Vklad.Enabled = false;
            panel_Vklad.Visible = false;

            panel_Vyber.Enabled = false;
            panel_Vyber.Visible = false;
        
            panel_Prevod_Na_Ucet.Enabled = false;
            panel_Prevod_Na_Ucet.Visible = false;

            panel_Profil.Enabled = false;
            panel_Profil.Visible = false;

        }

        private void pictureBox_Odhlasenie_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Boli ste úspešne odhlásený");
            this.Hide();
            Form_Login login = new Form_Login();
            login.ShowDialog();
            this.Close();
        }

        private void zistenieStavuUctu() { 
            StreamReader udaje = new StreamReader(users_subor,Encoding.UTF8);
            string riadok;
            while((riadok = udaje.ReadLine()) != null) {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[0].Trim().ToLower() == idPouzivatela.ToString().ToLower()) {
                    stavUctuPouzivatela = Convert.ToDouble(hodnoty[9]);
                }
            }
            udaje.Close();
        }

        private void pictureBox_Vklad_Click(object sender, EventArgs e)
        {
            zistenieStavuUctu();

            this.Text = "Vklad";

            label_Vklad_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";
            textBox_Vklad.Text = "";
            panel_Domov.Visible = false;
            panel_Vklad.Visible = true;
            panel_Vklad.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Vklad_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            textBox_Vklad.Text = "";
            panel_Domov.Visible = true;
            panel_Vklad.Visible = false;
            panel_Vklad.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void pictureBox_Vyber_Click(object sender, EventArgs e)
        {
            this.Text = "Vyber";

            zistenieStavuUctu();
            label_Vyber_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";
            textBox_Vyber.Text = "";
            panel_Domov.Visible = false;
            panel_Vyber.Visible = true;
            panel_Vyber.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Vyber_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            textBox_Vyber.Text = "";
            panel_Domov.Visible = true;
            panel_Vyber.Visible = false;
            panel_Vyber.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void pictureBox_Prevod_Na_Ucet_Click(object sender, EventArgs e)
        {
            zistenieStavuUctu();

            this.Text = "Prevod";

            label_Prevod_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";

            textBox_Prevod_Cislo_Odosielatel.Text = "";
            textBox_Prevod_Cislo_Prijemcu.Text = "";
            textBox_Prevod_Suma.Text = "";

            panel_Domov.Visible = false;
            panel_Prevod_Na_Ucet.Visible = true;
            panel_Prevod_Na_Ucet.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Prevod_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            textBox_Prevod_Cislo_Odosielatel.Text = "";
            textBox_Prevod_Cislo_Prijemcu.Text = "";
            textBox_Prevod_Suma.Text = "";

            panel_Domov.Visible = true;
            panel_Prevod_Na_Ucet.Visible = false;
            panel_Prevod_Na_Ucet.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void pictureBox_Profil_Click(object sender, EventArgs e)
        {
            this.Text = "Profil";

            panel_Domov.Visible = false;
            panel_Profil.Visible = true;
            panel_Profil.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Profil_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            panel_Domov.Visible = true;
            panel_Profil.Visible = false;
            panel_Profil.Enabled = false;
            panel_Domov.Enabled = true;
        }
    }
}
