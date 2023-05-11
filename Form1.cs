using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AracListesi 
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (File.Exists(temp))
            {
                string jsondata = File.ReadAllText(temp);
                araclar = JsonSerializer.Deserialize<List<Arac>>(jsondata);
            }
            ShowList();
        }

        List<Arac> araclar = new List<Arac>()
        {
            new Arac()
            { 
                Plaka = "45 önr 45",
                Marka = "tesla",
                Model = "2023",
                Yakıt = "elektrik",
                Renk = "mavi",
                Vites = "otomatik",
                KasaTipi = "haxpack",
                Aciklama = "temiz kullanıldı ",
            },
            new Arac()
            {
                Plaka = "05 ggc 40",
                Marka = "range rover",
                Model = "XC90",
                Yakıt = "benzin",
                Renk = "yeşil",
                Vites = "otomatik",
                KasaTipi = "sedan",
                Aciklama = "uzun yol aracı",
            }
        };

        public void ShowList()
        {
            listView1.Items.Clear();
            foreach (Arac arac in araclar)
            {
                AddPersonToListView(arac);
            }
        }

        public void AddPersonToListView(Arac arac)
        {
            ListViewItem item = new ListViewItem(new string[]
                {
                    arac.Plaka,
                    arac.Marka,
                    arac.Model,
                    arac.Yakıt,
                    arac.Renk,
                    arac.Vites,
                    arac.KasaTipi,
                    arac.Aciklama,
                });

            item.Tag = arac;
            listView1.Items.Add(item);
        }

        void EditPersonOnListView(ListViewItem pItem, Arac arac)
        {
            pItem.SubItems[0].Text = arac.Plaka;
            pItem.SubItems[1].Text = arac.Marka;
            pItem.SubItems[2].Text = arac.Model;
            pItem.SubItems[3].Text = arac.Yakıt;
            pItem.SubItems[4].Text = arac.Renk;
            pItem.SubItems[5].Text = arac.Vites;
            pItem.SubItems[6].Text = arac.KasaTipi;
            pItem.SubItems[7].Text = arac.Aciklama;

            pItem.Tag = arac;
        }

        private void AddCommand(object sender, EventArgs e)
        {
            FrmArac frm = new FrmArac()
            {
                Text="Araç Ekle",
                StartPosition = FormStartPosition.CenterParent,
                arac = new Arac()
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                araclar.Add(frm.arac);
                AddPersonToListView(frm.arac);
            }
        }

        private void EditCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem pItem = listView1.SelectedItems[0];
            Arac secili=pItem.Tag as Arac;

            FrmArac frm = new FrmArac()
            {
                Text = "Araç Düzenle",
                StartPosition = FormStartPosition.CenterParent,
                arac =  Clone(secili),
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.arac;
                EditPersonOnListView(pItem, secili);
            }
        }

        Arac Clone(Arac arac) 
        {
            return new Arac()
            {
                Plaka = arac.Plaka,
                Marka = arac.Marka,
                Model = arac.Model,
                Yakıt = arac.Yakıt,
                Renk = arac.Renk,
                Vites = arac.Vites,
                KasaTipi = arac.KasaTipi,
                Aciklama = arac.Aciklama,
            };
        }

        private void DeleteCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem pItem = listView1.SelectedItems[0];
            Arac secili=pItem.Tag as Arac;

           var sonuc= MessageBox.Show($"Seçili Araç silinsin mi?\n\n{secili.Plaka}",
                "Silmeyi Onayla", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if(sonuc == DialogResult.Yes)
            {
                araclar.Remove(secili);
                listView1.Items.Remove(pItem);
            }

        }

        private void SaveCommand(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı|*.json|Xml Formatı|*.xml",
            };

            if(sf.ShowDialog() == DialogResult.OK)
            {
                if(sf.FileName.EndsWith("json"))
                {
                    string data = JsonSerializer.Serialize(araclar);
                    File.WriteAllText(sf.FileName, data);
                }
                else if (sf.FileName.EndsWith("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Arac>));
                    serializer.Serialize(sw, araclar);
                    sw.Close();
                }
            }
        }

        private void LoadCommand(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json, Xml Formatları|*.json;*.xml",
            };

            if(of.ShowDialog() == DialogResult.OK)
            {
                if (of.FileName.ToLower().EndsWith("json"))
                {
                    string jsondata = File.ReadAllText(of.FileName);
                    araclar = JsonSerializer.Deserialize<List<Arac>>(jsondata);
                }
                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                    StreamReader sr = new StreamReader(of.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Arac>));
                    araclar = (List<Arac>)serializer.Deserialize(sr);
                    sr.Close();
                }

                ShowList();
            }
        }

        string temp = Path.Combine(Application.CommonAppDataPath, "data");

        protected override void OnClosing(CancelEventArgs e)
        {
            string data = JsonSerializer.Serialize(araclar);
            File.WriteAllText(temp, data);

            base.OnClosing(e);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }
    }


    [Serializable]
    public class Arac
    {

       
        [Category("Bilgiler"), DisplayName("Plaka")]
        public string Plaka { get; set; }

        [Category("Bilgiler"), DisplayName("Marka")]
        public string Marka { get; set; }

        [Category("Bilgiler"), DisplayName("Model")]
        public string Model { get; set; }

        [Category("Bilgiler"), DisplayName("Yakıt")]
        public string Yakıt { get; set; }

        [Category("Bilgiler"), DisplayName("Renk")]
        public string Renk { get; set; }

        [Category("Bilgiler"), DisplayName("Vites")]
        public string Vites { get; set; }


        [Category("Bilgiler"), DisplayName("Kasa Tipi")]
        public string KasaTipi { get; set; }

        [Category("Bilgiler"), DisplayName("Açıklama")]
        public string Aciklama { get; set; }


    }
}
